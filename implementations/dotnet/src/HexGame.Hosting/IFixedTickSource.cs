namespace HexGame.Hosting;

/// <summary>Produces an asynchronous stream of simulation ticks until cancelled.</summary>
public interface IFixedTickSource
{
    /// <summary>Yield ticks until the operation is cancelled or the implementation ends.</summary>
    IAsyncEnumerable<GameTick> ReadTicksAsync(CancellationToken cancellationToken = default);
}
