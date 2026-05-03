---
name: hexgame-implementation-review
description: >-
  Checklist-driven review of a game codebase against the HexGame architecture
  specification (dependency direction, commands, snapshots, effects, testing).
  Use when the user asks whether a project is HexGame-compliant, compatible, or
  strict per Section 27 of the spec.
disable-model-invocation: true
---

# HexGame implementation review

Read `spec/hexgame-architecture.md` (normative) for definitions. This skill is a **read-oriented** audit unless the user explicitly asks for code edits.

## Checklist

1. **Dependency rule (Section 6):** Does domain/application code avoid direct references to engines, UI frameworks, sockets, and file APIs where the spec forbids them?
2. **Commands (Section 8):** Does gameplay input reach the core as technology-neutral commands, not raw device or engine events?
3. **Authoritative state (Section 9):** Is simulation truth owned by core types, not engine scene objects?
4. **Tick (Section 10):** Is time and randomness controlled at boundaries where the spec expects?
5. **Snapshots (Section 11):** Do renderers consume snapshots or read models instead of mutating domain state?
6. **Effects (Section 13):** Are audio, particles, and similar side effects executed in adapters, not inside domain rules?
7. **Testing (Section 21):** Can core logic be tested without GPU, window, or engine runtime?
8. **Networking (Section 16):** For multiplayer, does the server remain authoritative over state?

## Output format

Summarize as:

- **Compliance guess:** HexGame-Compatible / HexGame-Compliant / HexGame-Strict / Not aligned (see spec Section 27), with justification.
- **Evidence:** File or module references that support each major checklist item.
- **Gaps:** Concrete violations with suggested remediation direction (no large unsolicited refactors).
