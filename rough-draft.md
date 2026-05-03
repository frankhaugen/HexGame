# HexGame Architecture Specification

## 1. Status

**Specification name:** HexGame Architecture
**Short name:** HexGame
**Version:** 0.1-draft
**Domain:** Game architecture
**Primary pattern family:** Hexagonal Architecture / Ports and Adapters
**Secondary pattern family:** Authoritative simulation, command processing, snapshot projection

## 2. Definition

HexGame is a hexagonal, authoritative-core architecture for games.

A HexGame system models the game as an application core. Inputs are normalized into commands. The core owns authoritative simulation state. Each tick advances that state and produces snapshots, events, and effects. Rendering, input, networking, persistence, tooling, and hosting technologies are adapters around the core.

The engine does not own the game.
The game owns the game.

## 3. Core Principle

A HexGame implementation MUST separate authoritative game logic from presentation, hosting, transport, and infrastructure concerns.

The core MUST own:

```text
state
rules
simulation
command handling
tick progression
snapshot projection
event emission
effect emission
save/load semantics
```

Adapters MAY own:

```text
rendering
input devices
audio playback
windowing
network transport
serialization
persistence implementation
launcher UI
editor UI
diagnostics
hosting
asset loading
```

The game core MUST NOT depend on a specific engine, rendering framework, UI framework, database, socket implementation, or host process.

## 4. Architectural Model

A HexGame frame follows this conceptual flow:

```text
Input Adapter
    ↓
Commands
    ↓
Game Application Core
    ↓
Domain Simulation
    ↓
Snapshot + Events + Effects
    ↓
Presentation / Audio / Network / Persistence Adapters
```

The core interaction is:

```text
Commands in.
Authoritative state advances.
Snapshots and effects out.
Adapters handle the outside world.
```

## 5. Required Layers

A HexGame solution SHOULD contain these logical layers.

### 5.1 Domain Layer

The domain layer contains pure game concepts.

It SHOULD contain:

```text
entities
value objects
rules
simulation state
domain services
deterministic systems
state transitions
```

It MUST NOT depend on:

```text
Stride
Unity
Raylib
MonoGame
Avalonia
ASP.NET
database clients
network sockets
file systems
windowing APIs
input APIs
```

### 5.2 Application Layer

The application layer orchestrates game use cases.

It SHOULD contain:

```text
start game
tick game
pause/resume
save/load orchestration
command validation
command dispatch
snapshot projection
event collection
effect collection
```

The application layer MAY depend on the domain layer.

The application layer MUST NOT depend on rendering/input-specific technologies.

### 5.3 Contracts Layer

The contracts layer defines stable boundary models.

It SHOULD contain:

```text
commands
snapshots
events
effects
ids
launch options
save/load requests
network DTOs
protocol messages
```

Contracts SHOULD be simple, serializable, and technology-neutral.

### 5.4 Adapter Layers

Adapter layers connect external technologies to the core.

Examples:

```text
Game.Frontend.Stride
Game.Frontend.Raylib
Game.Frontend.Avalonia
Game.Transport.SignalR
Game.Transport.Udp
Game.Persistence.Sqlite
Game.Host.Server
Game.Host.Local
```

Adapters MAY depend on engine and infrastructure libraries.

Adapters MUST NOT become the source of authoritative game truth.

### 5.5 Runtime / Composition Root

The runtime composes the system.

It SHOULD own:

```text
dependency injection
configuration
logging
host lifetime
adapter selection
runtime mode selection
process startup
shutdown
```

The composition root decides whether the game runs as:

```text
local single-player
local co-op host
remote graphical client
dedicated server
editor preview
simulation runner
test harness
replay runner
```

## 6. Dependency Rule

Dependencies MUST point inward.

```text
Domain
  ← Application
    ← Adapters / Infrastructure / Hosts / Runtime
```

Forbidden dependency examples:

```text
Domain → Stride
Domain → Raylib
Domain → Avalonia
Domain → ASP.NET
Domain → FileSystem
Domain → Socket
Application → Renderer
Application → Window
Application → Keyboard
```

Allowed dependency examples:

```text
Stride Adapter → Application
Raylib Adapter → Contracts
Server Host → Application
Persistence Adapter → Contracts
Runtime → all composed services
```

## 7. Application Core Contract

A HexGame core SHOULD expose a small application-facing contract.

Example:

```csharp
public interface IGameApplication
{
    GameStartResult Start(GameStartRequest request);

    GameFrameResult Tick(GameFrameRequest request);

    GameSaveResult Save(GameSaveRequest request);

    GameLoadResult Load(GameLoadRequest request);
}
```

The exact shape MAY vary, but a HexGame application MUST provide an explicit boundary through which commands enter and snapshots/effects leave.

## 8. Commands

Commands represent player, system, AI, script, replay, or network intent.

Commands MUST be technology-neutral.

The core MUST NOT receive raw device state such as:

```text
keyboard key state
mouse button state
controller button state
touch gesture state
network packet state
engine input event args
```

Instead, adapters MUST translate external input into commands.

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

Commands SHOULD contain intent, not presentation details.

Good:

```text
MoveForward
InteractWithTarget
UseSelectedItem
FirePrimaryWeapon
OpenInventory
```

Bad:

```text
WKeyDown
LeftMousePressed
GamepadButtonA
StrideInputEvent
RaylibKeyCode
```

## 9. Authoritative State

The authoritative state is the true game state.

It MAY include:

```text
player state
world state
health
inventory
quest state
AI state
combat state
economy state
simulation timers
RNG state
physics state, if core-owned
```

It MUST NOT be represented by engine objects such as:

```text
Stride Entity
Unity GameObject
Raylib Texture
Avalonia Control
MonoGame SpriteBatch
sound instance
particle emitter instance
UI element
camera animation object
```

Engine-side objects are projections, caches, or presentation resources.

They are not authoritative state.

## 10. Tick

A tick advances authoritative state.

A tick SHOULD include elapsed simulation time.

Example:

```csharp
public readonly record struct GameTick(
    TimeSpan Total,
    TimeSpan Delta);
```

A frame request SHOULD include tick data and pending commands.

```csharp
public sealed record GameFrameRequest(
    GameTick Tick,
    IReadOnlyList<PlayerCommand> Commands);
```

The tick function SHOULD be deterministic where practical.

The core SHOULD avoid direct use of:

```text
DateTime.Now
Random.Shared
Guid.NewGuid, except at controlled boundaries
file IO
network IO
database IO
engine physics as hidden state
global mutable state
thread-local mutable state
```

When randomness is required, the core SHOULD use explicit random state or an injected deterministic random source.

## 11. Snapshot

A snapshot is a read model of game state intended for external consumers.

Snapshots MAY be consumed by:

```text
renderers
network clients
debug tools
replay systems
tests
bot runners
editor previews
spectator views
```

Example:

```csharp
public sealed record GameSnapshot(
    WorldView World,
    PlayerView Player,
    HudView Hud,
    CameraView Camera);
```

A snapshot MUST NOT expose mutable authoritative state directly.

A snapshot SHOULD be stable, serializable, and presentation-oriented.

The snapshot MAY be:

```text
full-state
partial-state
delta-based
interest-filtered
player-specific
debug-enhanced
```

## 12. Events

Events describe meaningful things that occurred during the tick.

Examples:

```text
PlayerDied
ItemCollected
QuestCompleted
ProjectileHit
DoorOpened
ShipDocked
RoundStarted
```

Events MAY be used for:

```text
logging
replay
analytics
network replication
achievements
debugging
application workflows
```

Events SHOULD describe facts that happened, not instructions for a specific frontend.

## 13. Effects

Effects describe side effects requested by the core and executed by adapters.

Examples:

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

The core MAY emit effects.

Adapters MUST execute effects.

The domain MUST NOT directly:

```text
play audio
spawn engine particles
shake engine camera
show UI notifications
write files
send packets
call analytics APIs
```

## 14. Rendering Adapter

A rendering adapter consumes snapshots and updates a presentation model.

A rendering adapter MAY use:

```text
Stride
Raylib
MonoGame
Unity
Godot
Avalonia
WebGPU
console output
headless debug views
```

A rendering adapter MUST NOT be the source of authoritative state.

A renderer SHOULD maintain projection caches keyed by stable domain IDs.

Example:

```csharp
public interface IScenePresenter<in TSnapshot>
{
    void Present(TSnapshot snapshot);
}
```

Example:

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

In this model, engine entities are equivalent to frontend DOM nodes: they are synchronized from application state, not treated as application state.

## 15. Input Adapter

An input adapter translates raw input into commands.

Example:

```csharp
public interface IInputMapper
{
    IReadOnlyList<PlayerCommand> ReadCommands(PlayerId playerId);
}
```

A Stride input adapter MAY read Stride input APIs.

A Raylib input adapter MAY read Raylib input APIs.

A bot input adapter MAY generate commands from AI policy.

A replay input adapter MAY read commands from a recorded stream.

All of them SHOULD emit the same command model.

## 16. Networking

In multiplayer mode, the server SHOULD be authoritative.

Clients SHOULD send commands or intent.

The server SHOULD:

```text
receive commands
validate commands
advance the authoritative core
emit snapshots/events/deltas
replicate results to clients
```

Clients SHOULD:

```text
collect input
send commands
receive snapshots/events/deltas
render predicted or confirmed state
execute allowed local effects
```

The server MUST NOT trust client-side authoritative state.

Client-side prediction MAY be used, but the server remains authoritative.

## 17. Local Single-Player

Local single-player SHOULD use the same application core as multiplayer.

A local single-player host SHOULD be conceptually equivalent to:

```text
graphical client + authoritative server in the same process
```

The local renderer MUST NOT mutate authoritative state directly.

This prevents single-player and multiplayer from becoming separate implementations.

## 18. Hosting Modes

A HexGame system MAY support multiple hosts.

Common modes:

```csharp
public enum RuntimeMode
{
    LocalSinglePlayer,
    LocalCoopHost,
    DedicatedServer,
    RemoteClient,
    EditorPreview,
    Replay,
    SimulationTest
}
```

Host responsibilities MAY differ, but they SHOULD compose the same core.

## 19. Persistence

Persistence MUST store authoritative game state or explicit save models.

Persistence MUST NOT store engine object graphs as authoritative state.

Good persisted data:

```text
world state
player state
inventory
quest flags
RNG state
simulation time
entity domain IDs
save metadata
```

Bad persisted data:

```text
Stride scene as source of truth
Unity GameObject hierarchy as source of truth
Avalonia visual tree
Raylib texture handles
runtime-only particle systems
audio instance state
```

Persistence implementations are adapters.

## 20. Assets

Assets MAY be loaded by engine-specific or infrastructure adapters.

The core SHOULD refer to assets by stable technology-neutral keys.

Example:

```csharp
public readonly record struct AssetKey(string Value);
```

Good:

```text
"ships/earth_frigate"
"ui/hud/warning"
"audio/weapons/railgun_fire"
```

Bad in the core:

```text
Stride Model
Raylib Texture2D
MonoGame Texture2D
Unity Material
```

## 21. Testing

A HexGame core MUST be testable without:

```text
GPU
window
renderer
audio device
game engine runtime
network server
real filesystem
```

Tests SHOULD call the application core directly.

Example:

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

Rendering adapters MAY have separate integration tests, smoke tests, or visual tests.

Core correctness SHOULD NOT depend on rendering tests.

## 22. Replay

A HexGame implementation SHOULD support replay by recording command streams and tick data.

A replay runner SHOULD be able to reproduce a session by feeding recorded commands into the same application core.

Replay quality depends on deterministic simulation. If the simulation is not fully deterministic, the implementation SHOULD explicitly document the limitations.

## 23. Bot and Simulation Runners

Because input is command-based, bots MAY be implemented as command-producing adapters.

A bot runner MAY:

```text
read snapshots
decide commands
submit commands
inspect results
```

This allows automated playtesting without rendering.

## 24. Tooling and Editors

Tools and editors SHOULD interact with the game through contracts, snapshots, commands, save models, or editor-specific ports.

An Avalonia editor, for example, SHOULD NOT mutate engine scene objects and treat those as the authoritative game.

It SHOULD modify domain/editor models and allow adapters to project those changes into engine views.

## 25. Stride Lite Adapter Layer

A Stride Lite layer MAY be used to make Stride easier to consume as an adapter.

It MAY contain:

```text
code-only Stride bootstrapping
scene helpers
entity factories
UI helpers
input normalization
effect executors
asset lookup helpers
DI integration
runtime options
presenter base types
```

It MUST NOT contain:

```text
game rules
domain state ownership
business workflows
quest logic
combat rules
economy rules
save-game truth
```

Stride Lite is infrastructure.

It exists to reduce engine ceremony, not to become a second game framework.

## 26. Anti-Patterns

The following are non-compliant or weakly compliant with HexGame.

### 26.1 Engine-Owned Game State

```text
The scene graph is the source of truth.
```

### 26.2 Input Leakage

```text
Domain logic reads keyboard, mouse, controller, or engine input events.
```

### 26.3 Presentation Mutation

```text
UI buttons directly mutate domain entities.
```

### 26.4 Engine Persistence

```text
Engine object graphs are saved as authoritative game state.
```

### 26.5 Domain Side Effects

```text
Domain objects play sounds, spawn particles, write files, or send network packets.
```

### 26.6 Split Simulation

```text
Single-player and multiplayer use different rule implementations.
```

### 26.7 Renderer Authority

```text
Renderer position is treated as the real player position.
```

### 26.8 Framework-Driven Core

```text
The game core depends on Stride, Raylib, Unity, Godot, Avalonia, or ASP.NET.
```

## 27. Compliance Levels

### 27.1 HexGame-Compatible

A system is HexGame-compatible if:

```text
core game logic is separable from rendering
input is mostly command-based
snapshots or read models exist
tests can run some game logic without a renderer
```

### 27.2 HexGame-Compliant

A system is HexGame-compliant if:

```text
domain has no engine dependencies
application exposes explicit start/tick/save/load boundaries
input is normalized into commands
authoritative state is core-owned
rendering consumes snapshots/read models
effects are emitted and executed at edges
local and multiplayer modes can share the core
core tests require no engine runtime
```

### 27.3 HexGame-Strict

A system is HexGame-strict if:

```text
all authoritative simulation is deterministic or explicitly controlled
all randomness is explicit or state-owned
all persistence stores core state, not engine state
all frontends are replaceable adapters
server authority is used even for local-hosted play
replay can be driven from recorded commands/ticks
bot/simulation runners use the same application contracts
```

## 28. Recommended Project Layout

```text
src/
  MyGame.Contracts/
  MyGame.Domain/
  MyGame.Application/
  MyGame.Infrastructure/
  MyGame.Frontend.Stride/
  MyGame.Frontend.Avalonia/
  MyGame.Host.Local/
  MyGame.Host.Server/
  MyGame.Runtime/
  MyGame.Launcher.Avalonia/

tests/
  MyGame.Domain.Tests/
  MyGame.Application.Tests/
  MyGame.Infrastructure.Tests/
  MyGame.Frontend.Stride.Tests/
```

Reusable engine helper package:

```text
src/
  HexGame.StrideLite/
```

or:

```text
src/
  MyCompany.StrideLite/
```

## 29. Recommended Minimal Contracts

```csharp
public interface IGameApplication
{
    GameStartResult Start(GameStartRequest request);
    GameFrameResult Tick(GameFrameRequest request);
    GameSaveResult Save(GameSaveRequest request);
    GameLoadResult Load(GameLoadRequest request);
}
```

```csharp
public sealed record GameFrameRequest(
    GameTick Tick,
    IReadOnlyList<PlayerCommand> Commands);
```

```csharp
public sealed record GameFrameResult(
    GameSnapshot Snapshot,
    IReadOnlyList<GameEvent> Events,
    IReadOnlyList<GameEffect> Effects);
```

```csharp
public readonly record struct GameTick(
    TimeSpan Total,
    TimeSpan Delta);
```

```csharp
public abstract record PlayerCommand(PlayerId PlayerId, long Sequence);
```

```csharp
public abstract record GameEvent;
```

```csharp
public abstract record GameEffect;
```

```csharp
public interface IScenePresenter<in TSnapshot>
{
    void Present(TSnapshot snapshot);
}
```

```csharp
public interface IEffectExecutor<in TEffect>
{
    void Execute(IReadOnlyList<TEffect> effects);
}
```

## 30. Summary

HexGame defines a game as an authoritative application core surrounded by adapters.

Its defining properties are:

```text
commands enter the core
the core owns authoritative state
ticks advance simulation
snapshots expose read models
effects describe side effects
adapters render, transport, persist, and execute
engines are infrastructure
```

A HexGame implementation can run the same game as:

```text
local single-player
dedicated server
remote client
editor preview
headless simulation
automated test
replay runner
bot runner
```

The architecture’s central rule is:

```text
The game core owns truth.
Everything else is an adapter.
```
