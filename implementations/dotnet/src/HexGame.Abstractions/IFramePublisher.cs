namespace HexGame;

/// <summary>Publishes a game-defined frame payload (snapshot wire model, combined output, etc.) to transport or observers.</summary>
public interface IFramePublisher<TFrame>
{
    /// <summary>Send or record one frame payload.</summary>
    ValueTask PublishAsync(TFrame frame, CancellationToken cancellationToken = default);
}
