using System.Runtime.CompilerServices;

namespace HexGame.Hosting;

/// <summary>Replays a finite or infinite list of ticks without wall-clock delay (for tests harnessing <see cref="LocalGameHost{TCommand,TSnapshot,TDelta,TEffect}"/>).</summary>
public sealed class PrebuiltTickSource : IFixedTickSource
{
    private readonly IReadOnlyList<GameTick> _ticks;

    /// <summary>Creates a tick source that yields exactly the given sequence.</summary>
    public PrebuiltTickSource(IEnumerable<GameTick> ticks) => _ticks = ticks.ToArray();

    /// <inheritdoc />
    public async IAsyncEnumerable<GameTick> ReadTicksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var tick in _ticks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return tick;
            await Task.Yield();
        }
    }
}
