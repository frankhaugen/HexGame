namespace HexGame.Testing;

/// <summary>FIFO inbox where <see cref="ICommandInbox{TCommand}.Drain"/> returns all pending commands (frame parameter ignored).</summary>
public sealed class InMemoryCommandInbox<TCommand> : ICommandInbox<TCommand>
    where TCommand : IGameCommand
{
    private readonly List<TCommand> _pending = new();

    /// <inheritdoc />
    public void Enqueue(TCommand command) => _pending.Add(command);

    /// <inheritdoc />
    public IReadOnlyList<TCommand> Drain(GameFrameNumber frame)
    {
        if (_pending.Count == 0)
            return Array.Empty<TCommand>();

        var snapshot = _pending.ToArray();
        _pending.Clear();
        return snapshot;
    }
}
