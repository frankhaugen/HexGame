namespace HexGame.Hosting;

/// <summary>Process-level game host (local single-player, dedicated server shell, etc.).</summary>
public interface IGameHost
{
    /// <summary>Run the host loop until cancelled or completed.</summary>
    ValueTask RunAsync(CancellationToken cancellationToken = default);
}
