namespace HexGame.Testing;

/// <summary>Async-enumerable predetermined ticks (for tests). Not coupled to the hosting tick interface.</summary>
public sealed class FakeTickSource : IAsyncEnumerable<GameTick>
{
    private readonly GameTick[] _ticks;

    /// <summary>Creates a source that enumerates the given ticks in order.</summary>
    public FakeTickSource(IEnumerable<GameTick> ticks) => _ticks = ticks.ToArray();

    /// <summary>Linear ticks: frames 1..<paramref name="count"/>, fixed <paramref name="delta"/>, monotonic total.</summary>
    public static FakeTickSource Linear(int count, TimeSpan delta)
    {
        var list = new List<GameTick>(count);
        for (var i = 1; i <= count; i++)
        {
            var frame = new GameFrameNumber(i);
            var total = TimeSpan.FromTicks(delta.Ticks * i);
            list.Add(new GameTick(frame, total, delta));
        }

        return new FakeTickSource(list);
    }

    /// <inheritdoc />
    public async IAsyncEnumerator<GameTick> GetAsyncEnumerator(
        CancellationToken cancellationToken = default)
    {
        foreach (var tick in _ticks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return tick;
            await Task.Yield();
        }
    }
}
