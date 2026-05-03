# HexGame

**Status:** Draft specification **0.1** (informational).

HexGame is a **hexagonal, authoritative-core architecture** for games: commands in, an application core owns simulation state and advances ticks, snapshots and effects out—rendering, transport, and persistence stay in adapters.

## Read the spec

| Document | Description |
| --- | --- |
| [spec/hexgame-architecture.md](spec/hexgame-architecture.md) | **Normative** architecture specification (BCP 14 keywords). |
| [spec/informative-overview.md](spec/informative-overview.md) | **Informative** narrative overview (same spirit, not binding). |
| [spec/README.md](spec/README.md) | How this folder is organized. |

## Reference implementations

Optional libraries that mirror HexGame seams in specific ecosystems. The **spec remains canonical**; these are aids, not replacements.

- [implementations/README.md](implementations/README.md) — index of implementations.
- [.NET / NuGet](docs/reference-implementations/dotnet.md) — `HexGame.Abstractions`, `HexGame.Hosting`, `HexGame.Testing` (v0.1).

## Community and governance

- [CONTRIBUTING.md](CONTRIBUTING.md) — pull requests, changelog, DCO sign-off.
- [CHANGELOG.md](CHANGELOG.md) — release notes for the documentation set.
- [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) — Contributor Covenant 2.1.
- [SECURITY.md](SECURITY.md) — how to report security issues.
- [LEGAL.md](LEGAL.md) — copyright, disclaimer, IETF non-affiliation.

## License

[MIT License](LICENSE).

## Agents and editors

If you use Cursor in this repo, see [AGENTS.md](AGENTS.md) and the [.cursor/](.cursor/) directory for rules, skills, and subagents aligned with spec maintenance.
