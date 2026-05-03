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

Packages are emitted under each project’s `bin/Release/` as `HexGame.*.0.1.0.nupkg` (or the version you pass to `dotnet pack`).

## Publish to GitHub Packages (maintainers)

Packages are published from GitHub Actions ([`.github/workflows/dotnet-packages.yml`](../../.github/workflows/dotnet-packages.yml)) to the **GitHub Packages** NuGet registry for this repository’s owner.

**When it runs**

- **Manual:** Actions → “Publish .NET packages (GitHub Packages)” → *Run workflow* → set **version** (SemVer, e.g. `0.1.1`).
- **Release:** Create a [GitHub Release](https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository) with a tag like `v0.1.1`. The workflow strips a leading `v` and packs/pushes that version.

The workflow restores, builds, tests, packs into `implementations/dotnet/artifacts/`, then runs `dotnet nuget push` for each `.nupkg` using `GITHUB_TOKEN` (`packages: write`). Re-pushing the same version uses `--skip-duplicate`.

## Consume from GitHub Packages

The feed URL is:

`https://nuget.pkg.github.com/<GITHUB_USERNAME_OR_ORG>/index.json`

Replace `<GITHUB_USERNAME_OR_ORG>` with the account that owns the repository (for this repo: `frankhaugen`).

**1. Create a PAT** (classic: `read:packages`, or fine-grained with Packages read for this repository). `GITHUB_TOKEN` from your machine is not used for consumption.

**2. Register the source** (example: user `frankhaugen`, PAT in env for one session):

```bash
dotnet nuget add source "https://nuget.pkg.github.com/frankhaugen/index.json" \
  --name github-hexgame \
  --username frankhaugen \
  --password YOUR_PAT_HERE \
  --store-password-in-clear-text
```

**3. Or use `nuget.config`** next to your solution (do not commit the PAT; use environment variable substitution in CI only):

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github-hexgame" value="https://nuget.pkg.github.com/frankhaugen/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github-hexgame>
      <add key="Username" value="frankhaugen" />
      <add key="ClearTextPassword" value="%GITHUB_PACKAGES_TOKEN%" />
    </github-hexgame>
  </packageSourceCredentials>
</configuration>
```

Set `GITHUB_PACKAGES_TOKEN` in your environment to your PAT before `dotnet restore` / `dotnet add package`.

**4. Reference packages** (use the version you published, e.g. `0.1.1`):

```xml
<ItemGroup>
  <PackageReference Include="HexGame.Abstractions" Version="0.1.1" />
  <PackageReference Include="HexGame.Hosting" Version="0.1.1" />
  <PackageReference Include="HexGame.Testing" Version="0.1.1" />
</ItemGroup>
```

If restore does not see the feed, add `--source github-hexgame` or ensure `nuget.config` lists the source.

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
