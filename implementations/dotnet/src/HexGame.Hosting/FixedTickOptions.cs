namespace HexGame.Hosting;

/// <summary>Configuration for <see cref="FixedTickSource"/>.</summary>
public sealed record FixedTickOptions
{
    /// <summary>Gets simulation ticks per wall-clock second (must be at least 1).</summary>
    public int TicksPerSecond { get; init; } = 60;

    /// <summary>
    /// Gets the reserved catch-up cap when the real-time loop falls behind.
    /// v0.1: documented only; <see cref="FixedTickSource"/> emits one tick per wait interval.
    /// </summary>
    public int MaxCatchUpTicks { get; init; } = 5;
}
