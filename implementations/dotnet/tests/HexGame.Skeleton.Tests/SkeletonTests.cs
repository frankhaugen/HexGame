using HexGame.Hosting;
using HexGame.Testing;

namespace HexGame.Skeleton.Tests;

public sealed class SkeletonTests
{
    private static readonly PlayerId Player1 = new("p1");

    [Fact]
    public void HexGameScenario_runs_scheduled_commands_per_frame()
    {
        var app = new CounterApplication();
        var outputs = new HexGameScenario<BumpCommand, int, int?, int>()
            .At(new GameFrameNumber(1), new BumpCommand(Player1, 1, 2))
            .At(new GameFrameNumber(2), new BumpCommand(Player1, 2, 3))
            .Run(app, frameCount: 3, delta: TimeSpan.FromMilliseconds(16));

        Assert.Equal(3, outputs.Count);
        Assert.Equal(2, outputs[0].Snapshot);
        Assert.Equal(5, outputs[1].Snapshot);
        Assert.Equal(5, outputs[2].Snapshot);
    }

    [Fact]
    public async Task LocalGameHost_runs_until_prebuilt_ticks_exhaust()
    {
        var ticks = new[]
        {
            new GameTick(new GameFrameNumber(1), TimeSpan.FromMilliseconds(16), TimeSpan.FromMilliseconds(16)),
            new GameTick(new GameFrameNumber(2), TimeSpan.FromMilliseconds(32), TimeSpan.FromMilliseconds(16)),
        };
        var app = new CounterApplication();
        var input = new PerFrameCommands(
            (new GameFrameNumber(1), new BumpCommand(Player1, 1, 1)),
            (new GameFrameNumber(2), new BumpCommand(Player1, 2, 4)));
        var host = new LocalGameHost<BumpCommand, int, int?, int>(app, input, new PrebuiltTickSource(ticks));
        await host.RunAsync(CancellationToken.None);
        Assert.Equal(5, app.Total);
    }

    private sealed record BumpCommand(PlayerId PlayerId, long Sequence, int Delta) : IGameCommand;

    private sealed class CounterApplication : IGameApplication<BumpCommand, int, int?, int>
    {
        public int Total { get; private set; }

        public GameStartResult Start(GameStartRequest request) => new();

        public GameFrameOutput<int, int?, int> Tick(GameTick tick, IReadOnlyList<BumpCommand> commands)
        {
            foreach (var c in commands)
                Total += c.Delta;

            return new GameFrameOutput<int, int?, int>(tick.Frame, Total, null, Array.Empty<int>());
        }
    }

    private sealed class PerFrameCommands : IGameInputSource<BumpCommand>
    {
        private readonly Dictionary<long, List<BumpCommand>> _byFrame = new();

        public PerFrameCommands(params (GameFrameNumber Frame, BumpCommand Command)[] items)
        {
            foreach (var (frame, command) in items)
            {
                if (!_byFrame.TryGetValue(frame.Value, out var list))
                {
                    list = new List<BumpCommand>();
                    _byFrame[frame.Value] = list;
                }

                list.Add(command);
            }
        }

        public IReadOnlyList<BumpCommand> ReadCommands(GameFrameNumber frame) =>
            _byFrame.TryGetValue(frame.Value, out var list) ? list : Array.Empty<BumpCommand>();
    }
}
