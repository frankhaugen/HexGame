namespace HexGame;

/// <summary>
/// Optional persistence boundary (Save/Load) split from <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}"/>
/// so games without saves need not implement no-ops on the core tick interface.
/// </summary>
public interface IGamePersistence
{
    /// <summary>Persist authoritative state or explicit save models.</summary>
    GameSaveResult Save(GameSaveRequest request);

    /// <summary>Restore from a save model.</summary>
    GameLoadResult Load(GameLoadRequest request);
}
