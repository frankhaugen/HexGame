# Contributing to HexGame

Thank you for helping improve the HexGame architecture specification. This repository holds **documentation and normative prose**, not a reference game engine implementation.

## What to contribute

- Clarifications, errata, and consistency fixes to [spec/hexgame-architecture.md](spec/hexgame-architecture.md).
- Improvements to the informative overview in [spec/informative-overview.md](spec/informative-overview.md), provided they do not contradict the normative spec.
- Tooling for editors and agents under [.cursor/](.cursor/) when it helps maintain the spec.

## Normative versus informative

| Document | Role |
| --- | --- |
| [spec/hexgame-architecture.md](spec/hexgame-architecture.md) | **Normative.** Uses BCP 14 keywords (**MUST**, **SHOULD**, **MAY**). |
| [spec/informative-overview.md](spec/informative-overview.md) | **Informative** narrative only. |
| Root stubs [concept.md](concept.md) and [rough-draft.md](rough-draft.md) | Pointers only; do not extend as alternate specs. |

If a change would make the overview and the normative spec disagree, update the normative spec (or drop the conflicting sentence from the overview).

## Requirements language (BCP 14)

Normative sentences that impose requirements MUST use the capitalized keywords defined in [RFC 2119](https://www.rfc-editor.org/info/rfc2119) and [RFC 8174](https://www.rfc-editor.org/info/rfc8174). Do not use **MUST** or **SHOULD** in casual prose inside the normative document unless you mean a formal requirement.

## Pull request workflow

1. Fork the repository and create a branch from the default branch. Use a short descriptive branch name, for example `fix/command-section-typo` or `add-replay-security-note`.
2. Keep changes focused. Large editorial refactors SHOULD be discussed in an issue first.
3. Update [CHANGELOG.md](CHANGELOG.md) under `[Unreleased]` with a bullet in **Added**, **Changed**, **Deprecated**, **Removed**, **Fixed**, or **Security**, as appropriate. For release tagging, maintainers may roll `[Unreleased]` into a dated version section.
4. Ensure the **Table of Contents** in the normative spec still matches headings if you add or rename sections.
5. Open a pull request with a clear title and description of motivation and impact.

## Developer Certificate of Origin (sign-off)

This project uses the same sign-off process as the [Developer Certificate of Origin](https://developercertificate.org/). By contributing, you agree that your contribution is provided under the [LICENSE](LICENSE) and that you have the right to submit it.

Include a sign-off line in each commit message:

```text
Signed-off-by: Random J Developer <random@developer.example.org>
```

Use your real name and a reachable email address or GitHub noreply address you control.

## Code of conduct

Participants MUST follow [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).

## Questions

Open a [GitHub issue](https://github.com/frankhaugen/HexGame/issues) for discussion before large or ambiguous changes.
