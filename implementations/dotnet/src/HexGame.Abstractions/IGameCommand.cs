namespace HexGame;

/// <summary>
/// Player-originated command entering the game application boundary. Games define concrete types;
/// the framework only requires <see cref="PlayerId"/> and a monotonic <see cref="Sequence"/> per source.
/// </summary>
public interface IGameCommand
{
    /// <summary>Player this command is attributed to.</summary>
    PlayerId PlayerId { get; }

    /// <summary>Monotonic sequence from the command source (input or network).</summary>
    long Sequence { get; }
}
