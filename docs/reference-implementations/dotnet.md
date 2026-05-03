# .NET reference implementation (NuGet)

This is **one** optional reference stack for the HexGame architecture. The normative specification is language-neutral: [spec/hexgame-architecture.md](../../spec/hexgame-architecture.md). Other languages may add peer implementations under [implementations/](../../implementations/).

## Packages (v0.1)

| Package | Role |
| --- | --- |
| `HexGame.Abstractions` | Core seams: `IGameCommand`, `IGameApplication`, `GameFrameOutput`, presenter/input ports, command inbox, frame publisher, optional `IGamePersistence`, `GameDeliveryMode` (reserved for future transport packages). |
| `HexGame.Hosting` | `IFixedTickSource`, `FixedTickSource`, `PrebuiltTickSource`, `IGameHost`, `LocalGameHost`. |
| `HexGame.Testing` | `HexGameScenario`, `InMemoryCommandInbox`, `CapturingFramePublisher`, `FakeTickSource`. |

There is **no** engine binding, transport implementation, or binary serialization in v0.1.

## Build, test, pack

Run from the .NET solution root:

```bash
cd implementations/dotnet
dotnet build HexGame.sln -c Release
dotnet test HexGame.sln -c Release
dotnet pack HexGame.sln -c Release
```

Packages are emitted under each project’s `bin/Release/` as `HexGame.*.0.1.0.nupkg`.

## Spec mapping (informative)

| Spec sections | .NET types |
| --- | --- |
| 7 Application core, 29 Minimal contracts | `IGameApplication`, `GameTick`, `GameFrameOutput`, lifecycle records |
| 8 Commands | `IGameCommand`, `ISystemCommand` |
| 10 Tick | `GameTick`, `GameFrameNumber` |
| 11–13 Snapshot / events-style output / effects | Generic `TSnapshot`, `TDelta`, `TEffect` on `GameFrameOutput` and `IGamePresenter` |
| 15 Input adapter | `IGameInputSource` |
| 16+ hosting (local loop) | `LocalGameHost`, `IFixedTickSource` |
| 21 Testing | `HexGameScenario`, in-memory adapters |

## Roadmap (not in v0.1)

Future packages may include `HexGame.Transport.*`, `HexGame.Client`, `HexGame.Server`, `HexGame.Replay`, and technology-specific adapters (Stride, SignalR, etc.). They are intentionally absent from this skeleton.
