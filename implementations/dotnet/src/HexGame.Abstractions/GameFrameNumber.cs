namespace HexGame;

/// <summary>
/// Monotonic simulation frame index. Used with <see cref="GameTick"/> and command ordering.
/// </summary>
public readonly record struct GameFrameNumber(long Value)
{
    /// <summary>Frame before the first simulation tick (not passed to <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}.Tick"/>).</summary>
    public static GameFrameNumber Zero => new(0);

    /// <summary>Returns the next frame after this one.</summary>
    public GameFrameNumber Next() => new(Value + 1);
}
