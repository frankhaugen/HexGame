namespace HexGame;

/// <summary>
/// Server-side (or local) command buffer drained once per tick before <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}.Tick"/>.
/// </summary>
public interface ICommandInbox<TCommand>
    where TCommand : IGameCommand
{
    /// <summary>Queue a command for a future <see cref="Drain"/>.</summary>
    void Enqueue(TCommand command);

    /// <summary>
    /// Removes and returns all pending commands for the tick identified by <paramref name="frame"/>.
    /// Implementations may ignore <paramref name="frame"/> if they use a single FIFO queue (v0.1 default for in-memory inbox).
    /// </summary>
    IReadOnlyList<TCommand> Drain(GameFrameNumber frame);
}
