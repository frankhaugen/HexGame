---
name: hexgame-spec-change
description: >-
  Workflow for proposing and landing a change to the HexGame normative or
  informative specifications, including changelog and TOC hygiene. Use when
  the user asks to update the architecture spec, add sections, or reconcile
  overview text with hexgame-architecture.md.
disable-model-invocation: true
---

# HexGame spec change workflow

## Before you edit

1. Read the relevant sections of `spec/hexgame-architecture.md` (normative) and, if touched, `spec/informative-overview.md` (informative only).
2. Confirm whether the change is normative (creates or tightens a requirement) or editorial (clarity only). Normative changes need precise BCP 14 wording.

## Editing

1. Edit `spec/hexgame-architecture.md` and/or `spec/informative-overview.md` in small, coherent chunks.
2. If headings changed, update the **Table of Contents** in the normative file.
3. Ensure examples and lists still align with Section 31 (Security Considerations) where security-relevant behavior is described.

## After you edit

1. Append an entry under `## [Unreleased]` in `CHANGELOG.md` using Keep a Changelog categories.
2. If `CONTRIBUTING.md` process changed, update that file in the same change set.

## Pull request description template

Use this shape when opening or describing a PR:

```markdown
## Summary
- What changed in one short paragraph.

## Normative impact
- [ ] Editorial / clarification only
- [ ] New or changed MUST/SHOULD/MAY (describe)

## Files
- List touched paths.

## Changelog
- [x] CHANGELOG.md updated under [Unreleased]
```
