# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to semantic versioning for **documentation releases** of the normative specification.

## [Unreleased]

### Added

- .NET reference implementation under [implementations/dotnet/](implementations/dotnet/) (`HexGame.Abstractions`, `HexGame.Hosting`, `HexGame.Testing` NuGet packages v0.1.0) with smoke tests and [docs/reference-implementations/dotnet.md](docs/reference-implementations/dotnet.md).
- GitHub Actions workflow [.github/workflows/dotnet-packages.yml](.github/workflows/dotnet-packages.yml) to publish those packages to **GitHub Packages** (manual `workflow_dispatch` with version input, or on **Release published** from tag `v*`). Consumer notes in [docs/reference-implementations/dotnet.md](docs/reference-implementations/dotnet.md).

### Removed

- Root `concept.md` and `rough-draft.md` (use `spec/informative-overview.md` and `spec/hexgame-architecture.md` instead).

## [0.1.0] - 2026-05-03

### Added

- Normative specification [spec/hexgame-architecture.md](spec/hexgame-architecture.md) (version 0.1) with RFC-style framing: abstract, table of contents, BCP 14 document conventions, and closing sections for security considerations, IANA considerations, and informative references.
- [spec/README.md](spec/README.md) describing normative versus informative documents and versioning expectations.
- [spec/informative-overview.md](spec/informative-overview.md) as the narrative companion to the normative spec.
- [CONTRIBUTING.md](CONTRIBUTING.md), [SECURITY.md](SECURITY.md), [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) (Contributor Covenant 2.1), and [LEGAL.md](LEGAL.md).
- Cursor project context: [AGENTS.md](AGENTS.md), [.cursor/rules/](.cursor/rules/), [.cursor/skills/](.cursor/skills/), and [.cursor/agents/](.cursor/agents/).

### Changed

- [README.md](README.md) expanded into a proper project portal.
- Root `rough-draft.md` and `concept.md` replaced with short stubs pointing at `spec/` (stubs removed in a later change; use `spec/` paths directly).
