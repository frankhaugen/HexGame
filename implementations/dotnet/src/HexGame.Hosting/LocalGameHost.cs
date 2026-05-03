namespace HexGame.Hosting;

/// <summary>
/// In-process loop: fixed ticks → read commands → <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}.Tick"/> → optional presenter.
/// </summary>
public sealed class LocalGameHost<TCommand, TSnapshot, TDelta, TEffect> : IGameHost
    where TCommand : IGameCommand
{
    private readonly IGameApplication<TCommand, TSnapshot, TDelta, TEffect> _application;
    private readonly IGameInputSource<TCommand> _input;
    private readonly IFixedTickSource _tickSource;
    private readonly IGamePresenter<TSnapshot, TDelta, TEffect>? _presenter;

    /// <summary>Creates a local in-process host.</summary>
    public LocalGameHost(
        IGameApplication<TCommand, TSnapshot, TDelta, TEffect> application,
        IGameInputSource<TCommand> input,
        IFixedTickSource tickSource,
        IGamePresenter<TSnapshot, TDelta, TEffect>? presenter = null)
    {
        _application = application;
        _input = input;
        _tickSource = tickSource;
        _presenter = presenter;
    }

    /// <inheritdoc />
    public async ValueTask RunAsync(CancellationToken cancellationToken = default)
    {
        _ = _application.Start(new GameStartRequest());
        await foreach (var tick in _tickSource.ReadTicksAsync(cancellationToken).ConfigureAwait(false))
        {
            var commands = _input.ReadCommands(tick.Frame);
            var output = _application.Tick(tick, commands);
            if (_presenter is not null)
                Present(_presenter, output);
        }
    }

    private static void Present(IGamePresenter<TSnapshot, TDelta, TEffect> presenter, GameFrameOutput<TSnapshot, TDelta, TEffect> output)
    {
        if (output.Snapshot is { } snapshot)
            presenter.PresentSnapshot(snapshot);

        if (output.Delta is { } delta)
            presenter.ApplyDelta(delta);

        presenter.ExecuteEffects(output.Effects);
    }
}
