# HexGame repository agents

This repository is a **specification** workspace for the HexGame game architecture. Treat it as documentation-first: the product is clear, testable prose and structure, not a shipped runtime.

## Canonical sources

- **Normative:** [spec/hexgame-architecture.md](spec/hexgame-architecture.md). This file defines requirements using BCP 14 keywords.
- **Informative:** [spec/informative-overview.md](spec/informative-overview.md) and [spec/README.md](spec/README.md).
- **Not normative:** Root [concept.md](concept.md) and [rough-draft.md](rough-draft.md) are stubs that only point into `spec/`. Do not treat them as sources of truth.

## When editing

1. Prefer minimal, reviewable diffs; preserve section numbering unless you are intentionally issuing a new spec revision.
2. Keep BCP 14 usage correct: uppercase **MUST**, **SHOULD**, **MAY** only for normative requirements in the normative document.
3. After substantive normative or governance edits, update [CHANGELOG.md](CHANGELOG.md) under `[Unreleased]` per [CONTRIBUTING.md](CONTRIBUTING.md).
4. If you add or rename headings in the normative spec, update its **Table of Contents** anchors.

## Cursor assets

- **Rules:** [.cursor/rules/](.cursor/rules/) — always-on repo context and spec-authoring constraints.
- **Skills:** [.cursor/skills/](.cursor/skills/) — workflows for spec changes and implementation audits.
- **Subagents:** [.cursor/agents/](.cursor/agents/) — specialized prompts for spec editing, spec review, and codebase compliance review.

Invoke skills by name when the user asks for a structured spec change or a compliance audit. Delegate to subagents when isolation or a fresh pass helps.
