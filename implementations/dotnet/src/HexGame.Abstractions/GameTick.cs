namespace HexGame;

/// <summary>
/// Elapsed simulation time for one tick. The authoritative core advances once per <see cref="GameTick"/>.
/// </summary>
public readonly record struct GameTick(GameFrameNumber Frame, TimeSpan Total, TimeSpan Delta);
