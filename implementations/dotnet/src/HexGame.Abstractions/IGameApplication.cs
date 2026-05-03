namespace HexGame;

/// <summary>
/// Authoritative game application boundary: start once, then advance by tick with normalized commands.
/// Matches the normative shape in HexGame spec (Sections 7, 8, 10–13).
/// </summary>
public interface IGameApplication<TCommand, TSnapshot, TDelta, TEffect>
    where TCommand : IGameCommand
{
    /// <summary>Initialize simulation from a cold start.</summary>
    GameStartResult Start(GameStartRequest request);

    /// <summary>Advance authoritative state for one tick.</summary>
    GameFrameOutput<TSnapshot, TDelta, TEffect> Tick(GameTick tick, IReadOnlyList<TCommand> commands);
}
