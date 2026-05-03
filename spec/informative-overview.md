# HexGame architecture (informative overview)

> **This document is informative only.** The normative specification is [hexgame-architecture.md](hexgame-architecture.md). If this overview disagrees with that document, trust the normative spec.

---

# HexGame Architecture

HexGame is a hexagonal, authoritative-core architecture for games.

The game is modeled as an application core. Inputs are represented as commands, the simulation owns authoritative state, each tick advances the game state, and the result of that tick is exposed as snapshots and effects. Rendering, input, networking, persistence, tooling, and hosting technologies are replaceable adapters around the core.

HexGame treats the game engine as infrastructure, not as the owner of the application.

## Purpose

HexGame exists to make games easier to reason about, test, host, and adapt across different runtimes.

The architecture is intended for games where one or more of the following are desirable:

```text
single-player and multiplayer sharing the same simulation code
replaceable rendering technologies
headless dedicated servers
automated simulation tests
bot/playtest runners
external launchers or editors
clean separation between game rules and engine objects
```

The core idea is simple:

```text
Commands in.
Authoritative simulation advances.
Snapshots and effects out.
Engines are adapters.
```

## Architectural Principle

A HexGame application is built around an authoritative game core.

The core owns:

```text
game state
rules
simulation
command handling
tick progression
snapshot projection
effect emission
save/load model
```

The adapters own:

```text
rendering
input devices
audio playback
network transport
windowing
launcher UI
editor UI
persistence implementation
diagnostics
hosting
```

The core must not depend on any specific game engine, rendering framework, UI framework, or network stack.

## Conceptual Model

A game frame is modeled as an application interaction.

```text
Input Adapter
    ↓
Player/System Commands
    ↓
Game Application
    ↓
Domain Simulation
    ↓
Game Snapshot + Effects
    ↓
Presentation / Network / Audio Adapters
```

In local single-player, the frontend may call the application core directly in-process.

In multiplayer, clients send commands to a server host. The server runs the same application core authoritatively and distributes snapshots or deltas back to clients.

The same game core can therefore support:

```text
local single-player
local co-op host
dedicated multiplayer server
remote graphical client
headless simulation runner
automated test harness
editor preview mode
replay playback
```

## Relationship to Existing Patterns

HexGame is not a completely new architectural invention. It is a game-specific composition of established patterns:

```text
Hexagonal Architecture / Ports and Adapters
Clean Architecture
Server-authoritative simulation
Command-based input modeling
Snapshot-based presentation
Presentation Model / View Model
Application service orchestration
```

The game-specific contribution is the explicit framing of the game loop as an authoritative application boundary.

Game engines become adapters.
Scenes become projections.
Input devices become command sources.
Audio and particles become effects.
Servers and clients become hosts around the same core.

## Layer Model

A typical HexGame solution contains the following layers.

```text
Game.Domain
  Pure game rules, state, entities, value objects, and deterministic simulation logic.

Game.Application
  Start, tick, save, load, command orchestration, snapshot projection, and effect emission.

Game.Contracts
  Commands, snapshots, effects, identifiers, launch options, protocol DTOs.

Game.Infrastructure
  Persistence, file systems, asset catalogs, configuration, diagnostics, external services.

Game.Frontend.Stride / Game.Frontend.Raylib / Game.Frontend.Avalonia
  Rendering, input mapping, UI presentation, audio execution, scene synchronization.

Game.Host.Local
  Local single-player or local co-op host.

Game.Host.Server
  Headless authoritative server host.

Game.Launcher.Avalonia
  Launcher, settings, profile selection, world selection, editor entry point.

Game.Runtime
  Process entry point and composition root.
```

The dependency direction is inward.

```text
Domain
  ← Application
    ← Frontends / Infrastructure / Hosts
```

The domain does not reference Stride, Raylib, Avalonia, ASP.NET, sockets, databases, or UI frameworks.

## The Application Core

The application core exposes the game as a small set of operations.

```csharp
public interface IGameApplication
{
    GameStartResult Start(GameStartRequest request);

    GameFrameResult Tick(GameFrameRequest request);

    GameSaveResult Save(GameSaveRequest request);

    GameLoadResult Load(GameLoadRequest request);
}
```

A frame request contains time and commands.

```csharp
public sealed record GameFrameRequest(
    GameTick Tick,
    IReadOnlyList<PlayerCommand> Commands);
```

A frame result contains the presentation state and side effects.

```csharp
public sealed record GameFrameResult(
    GameSnapshot Snapshot,
    IReadOnlyList<GameEvent> Events,
    IReadOnlyList<GameEffect> Effects);
```

The application is the boundary. Everything outside it is replaceable.

## Commands

Input is normalized into commands before it reaches the core.

The domain should not know about keyboards, controllers, mouse buttons, touch screens, or network packets.

Instead of this:

```text
Key W is down
Mouse left was clicked
Gamepad button A was pressed
```

the core receives this:

```text
Move forward
Use selected item
Interact with target
Open pause menu
Cast ability
Confirm action
```

Example:

```csharp
public abstract record PlayerCommand(PlayerId PlayerId, long Sequence);

public sealed record MoveCommand(
    PlayerId PlayerId,
    long Sequence,
    MovementIntent Movement) : PlayerCommand(PlayerId, Sequence);

public sealed record InteractCommand(
    PlayerId PlayerId,
    long Sequence,
    EntityId TargetId) : PlayerCommand(PlayerId, Sequence);
```

The frontend is responsible for translating physical input into commands.

## Authoritative State

The simulation owns the authoritative state.

Engine objects are not game state. They are projections of game state.

This means the following are part of the core:

```text
player position
health
inventory
quest progress
world facts
AI state
combat state
resource state
RNG state
```

The following are not authoritative state:

```text
Stride Entity
Raylib texture
Avalonia control
camera shake animation object
particle emitter instance
sound instance
UI element
```

Those are adapter-side representations.

## Ticks

A tick advances the authoritative simulation.

```csharp
public readonly record struct GameTick(
    TimeSpan Total,
    TimeSpan Delta);
```

The tick should be deterministic where practical. The core should avoid direct access to:

```text
DateTime.Now
Random.Shared
file IO
network IO
engine physics as source of truth
global mutable state
```

For multiplayer-capable games, local single-player should behave like an in-process authoritative server. This prevents the local version and server version from becoming separate games.

## Snapshots

A snapshot is a read model of the current game state.

It is designed for presentation, networking, replay, debugging, or inspection.

```csharp
public sealed record GameSnapshot(
    WorldView World,
    PlayerView Player,
    HudView Hud,
    CameraView Camera);
```

A rendering adapter consumes snapshots and updates its scene accordingly.

```csharp
public interface IScenePresenter<in TSnapshot>
{
    void Present(TSnapshot snapshot);
}
```

The snapshot is not necessarily the full domain state. It is the state the outside world needs to observe.

## Effects

Effects represent side effects emitted by the core but executed by adapters.

Examples:

```text
play sound
spawn particle
show notification
trigger camera shake
start music cue
send analytics event
publish game event
```

Example model:

```csharp
public abstract record GameEffect;

public sealed record PlaySoundEffect(
    string SoundKey,
    float Volume = 1) : GameEffect;

public sealed record SpawnParticleEffect(
    string ParticleKey,
    Position3 Position) : GameEffect;

public sealed record CameraShakeEffect(
    float Intensity,
    TimeSpan Duration) : GameEffect;
```

The domain decides that an effect should happen.
The frontend decides how to execute it.

## Rendering as Frontend

Rendering is treated like a frontend over an API.

A Stride, Raylib, MonoGame, Avalonia, web, console, or test frontend can consume the same snapshots and effects.

A Stride frontend, for example, may maintain a cache of engine entities keyed by domain IDs.

```csharp
public sealed class EntityScenePresenter : IScenePresenter<GameSnapshot>
{
    private readonly Dictionary<EntityId, Entity> _entities = [];

    public void Present(GameSnapshot snapshot)
    {
        foreach (var view in snapshot.World.Entities)
        {
            var entity = GetOrCreate(view.Id);
            entity.Transform.Position = view.Transform.Position.ToStride();
            entity.Transform.Rotation = view.Transform.Rotation.ToStride();
        }
    }
}
```

Stride entities are therefore similar to DOM nodes in a browser frontend: they are synchronized from application state but are not the source of truth.

## Local and Multiplayer Modes

HexGame makes local and multiplayer modes variations of hosting.

Local single-player:

```text
Input Adapter
  → in-process GameApplication
  → Snapshot/Effects
  → Renderer/Audio
```

Multiplayer:

```text
Client Input Adapter
  → network commands
  → authoritative server GameApplication
  → snapshots/events/deltas
  → remote clients
```

The core simulation is shared.

Only the edges diverge:

```text
local transport vs network transport
local frontend vs remote client
in-process host vs dedicated server host
direct snapshot access vs serialized protocol
```

## Server Authority

In multiplayer, the server is authoritative.

Clients send intent.
The server validates commands.
The server advances the simulation.
Clients render the resulting state.

This avoids splitting the game into separate client and server rule sets.

The same principle can be used locally by running the authoritative core in-process.

## Testing

HexGame allows game logic to be tested without a window, renderer, GPU, or engine runtime.

A test can call the application core directly.

```csharp
[Fact]
public void Tick_WhenPlayerMovesForward_UpdatesPlayerPosition()
{
    var game = TestGameApplication.Create();

    game.Start(new GameStartRequest(WorldId: "test-world"));

    var result = game.Tick(new GameFrameRequest(
        new GameTick(TimeSpan.Zero, TimeSpan.FromMilliseconds(16)),
        [
            new MoveCommand(
                PlayerId: new PlayerId("player-1"),
                Sequence: 1,
                Movement: MovementIntent.Forward)
        ]));

    result.Snapshot.Player.Position.Z.Should().BeGreaterThan(0);
}
```

This makes core simulation tests fast, deterministic, and independent of rendering technology.

## Adapter Examples

A HexGame application may have many adapters around the same core.

```text
Stride frontend
  3D rendering, game UI, audio, input

Raylib frontend
  lightweight prototype rendering

Avalonia frontend
  launcher, editor, diagnostics, admin UI

ASP.NET host
  dedicated server, HTTP admin API, matchmaking, diagnostics

Console host
  simulation runner, replay validator, bot runner

Test host
  deterministic scenario tests

Replay host
  consumes recorded command streams and reproduces sessions
```

## Stride Lite as an Adapter Layer

When using Stride, a thin helper layer can reduce engine ceremony without making the engine own the game.

Such a layer may contain:

```text
code-only Stride bootstrapping
scene setup helpers
entity factories
input normalization
UI host helpers
effect executors
scene presenter base types
DI integration
runtime options
```

This layer should not contain game rules.

It exists to make Stride easier to use as an adapter.

## Constraints

A HexGame implementation should follow these constraints:

```text
The domain must not reference rendering, input, networking, or hosting frameworks.

Engine objects must not be persisted as authoritative game state.

Input must be normalized into commands before reaching the core.

Rendering must consume snapshots, not mutate domain state directly.

Effects must be emitted by the core and executed at the edge.

Local single-player should use the same application core as multiplayer.

The composition root decides which adapters are active.

Testing the core must not require a renderer or game engine runtime.
```

## Anti-Patterns

The following work against HexGame:

```text
Putting game rules inside engine components
Using scene objects as the source of truth
Letting UI mutate domain state directly
Persisting engine entities
Reading keyboard state inside domain logic
Playing sounds directly from domain entities
Using client-side state as authoritative multiplayer state
Duplicating local and server simulation logic
Making the rendering framework a dependency of the core
```

## Summary

HexGame is a game architecture where the game is an authoritative application core and all technologies around it are adapters.

It is especially suited for games that need clean simulation logic, replaceable rendering, strong testability, multiplayer compatibility, headless hosting, or external tooling.

The defining loop is:

```text
Commands enter.
The authoritative core advances.
Snapshots and effects leave.
Adapters present, transport, persist, or execute the result.
```

The engine does not own the game.

The game owns the game.
