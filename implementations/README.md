# Reference implementations

This folder contains **optional, language-specific** libraries that mirror the seams described in the normative specification ([spec/hexgame-architecture.md](../spec/hexgame-architecture.md)). They are **not** part of the spec itself and are **not** the only way to implement HexGame.

| Stack | Location | Status |
| --- | --- | --- |
| .NET / NuGet | [dotnet/](dotnet/) | v0.1 skeleton (Abstractions, Hosting, Testing) |

Additional ecosystems (for example Rust, Go, or TypeScript) may add peer folders here over time. Each implementation should link back to the spec and avoid claiming to be the canonical HexGame runtime.

Documentation for readers who start from `docs/`: [docs/reference-implementations/README.md](../docs/reference-implementations/README.md). Package build and layout details: [docs/reference-implementations/dotnet.md](../docs/reference-implementations/dotnet.md).
