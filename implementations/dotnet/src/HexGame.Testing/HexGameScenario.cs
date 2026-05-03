namespace HexGame.Testing;

/// <summary>Deterministic multi-frame run: schedule commands per frame, then drive the application tick.</summary>
public sealed class HexGameScenario<TCommand, TSnapshot, TDelta, TEffect>
    where TCommand : IGameCommand
{
    private readonly List<(GameFrameNumber Frame, TCommand Command)> _scheduled = new();

    /// <summary>Schedule a command for a specific simulation frame (see <see cref="GameFrameNumber"/>).</summary>
    public HexGameScenario<TCommand, TSnapshot, TDelta, TEffect> At(GameFrameNumber frame, TCommand command)
    {
        _scheduled.Add((frame, command));
        return this;
    }

    /// <summary>
    /// Calls <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}.Start"/> once, then ticks frames 1..<paramref name="frameCount"/> with the given <paramref name="delta"/>.
    /// </summary>
    public IReadOnlyList<GameFrameOutput<TSnapshot, TDelta, TEffect>> Run(
        IGameApplication<TCommand, TSnapshot, TDelta, TEffect> application,
        int frameCount,
        TimeSpan delta)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(frameCount, 1);

        _ = application.Start(new GameStartRequest());
        var outputs = new List<GameFrameOutput<TSnapshot, TDelta, TEffect>>(frameCount);
        for (var i = 1; i <= frameCount; i++)
        {
            var frame = new GameFrameNumber(i);
            var tick = new GameTick(frame, TimeSpan.FromTicks(delta.Ticks * i), delta);
            var commands = CommandsForFrame(frame);
            outputs.Add(application.Tick(tick, commands));
        }

        return outputs;
    }

    private List<TCommand> CommandsForFrame(GameFrameNumber frame)
    {
        var list = new List<TCommand>();
        foreach (var (f, c) in _scheduled)
        {
            if (f.Value == frame.Value)
                list.Add(c);
        }

        return list;
    }
}
