using System.Runtime.CompilerServices;

namespace HexGame.Hosting;

/// <summary>
/// Wall-clock paced ticks at <see cref="FixedTickOptions.TicksPerSecond"/>. First yielded tick uses <see cref="GameFrameNumber"/> value 1.
/// </summary>
public sealed class FixedTickSource : IFixedTickSource
{
    private readonly FixedTickOptions _options;

    /// <summary>Creates a wall-clock paced tick source.</summary>
    public FixedTickSource(FixedTickOptions options) => _options = options;

    /// <inheritdoc />
    public async IAsyncEnumerable<GameTick> ReadTicksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var tps = Math.Max(1, _options.TicksPerSecond);
        var delta = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / tps);
        for (var frameValue = 1L; !cancellationToken.IsCancellationRequested; frameValue++)
        {
            await Task.Delay(delta, cancellationToken).ConfigureAwait(false);
            var frame = new GameFrameNumber(frameValue);
            var total = TimeSpan.FromTicks(delta.Ticks * frameValue);
            yield return new GameTick(frame, total, delta);
        }
    }
}
