namespace HexGame;

/// <summary>
/// Adapter port: collects normalized commands for a frame (keyboard/gamepad/network → <typeparamref name="TCommand"/>).
/// </summary>
public interface IGameInputSource<TCommand>
    where TCommand : IGameCommand
{
    /// <summary>Commands to apply when simulating <paramref name="frame"/>.</summary>
    IReadOnlyList<TCommand> ReadCommands(GameFrameNumber frame);
}
