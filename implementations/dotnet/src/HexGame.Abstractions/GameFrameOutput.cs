namespace HexGame;

/// <summary>
/// One tick's outward-facing data: optional full snapshot, optional delta, and effects for adapters to execute.
/// </summary>
/// <typeparam name="TSnapshot">Read model for presentation / replication.</typeparam>
/// <typeparam name="TDelta">Optional partial update.</typeparam>
/// <typeparam name="TEffect">Side effects (sounds, particles, etc.) executed outside the core.</typeparam>
public sealed record GameFrameOutput<TSnapshot, TDelta, TEffect>(
    GameFrameNumber Frame,
    TSnapshot? Snapshot,
    TDelta? Delta,
    IReadOnlyList<TEffect> Effects);
