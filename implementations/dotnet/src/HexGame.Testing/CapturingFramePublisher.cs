namespace HexGame.Testing;

/// <summary>Records every published frame for assertions.</summary>
public sealed class CapturingFramePublisher<TFrame> : IFramePublisher<TFrame>
{
    private readonly List<TFrame> _frames = new();

    /// <summary>All frames passed to <see cref="PublishAsync"/> so far.</summary>
    public IReadOnlyList<TFrame> Frames => _frames;

    /// <inheritdoc />
    public ValueTask PublishAsync(TFrame frame, CancellationToken cancellationToken = default)
    {
        _frames.Add(frame);
        return ValueTask.CompletedTask;
    }
}
