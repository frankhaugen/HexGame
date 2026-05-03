namespace HexGame;

/// <summary>
/// Non-player command (AI, replay, admin, etc.) that still participates in sequencing.
/// </summary>
public interface ISystemCommand
{
    /// <summary>Monotonic sequence from the non-player command source.</summary>
    long Sequence { get; }
}
