namespace HexGame;

/// <summary>
/// Adapter port: applies snapshots/deltas and executes effects (render/audio/UI), without owning authoritative state.
/// </summary>
public interface IGamePresenter<in TSnapshot, in TDelta, in TEffect>
{
    /// <summary>Full read model for this tick (optional if deltas only).</summary>
    void PresentSnapshot(TSnapshot snapshot);

    /// <summary>Apply a partial update on top of the last snapshot.</summary>
    void ApplyDelta(TDelta delta);

    /// <summary>Execute presentation-side effects emitted by the core for this tick.</summary>
    void ExecuteEffects(IReadOnlyList<TEffect> effects);
}
