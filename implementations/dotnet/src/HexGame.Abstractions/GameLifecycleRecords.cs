namespace HexGame;

/// <summary>Application start parameters (extend in your game assembly as needed).</summary>
public sealed record GameStartRequest;

/// <summary>Outcome of <see cref="IGameApplication{TCommand,TSnapshot,TDelta,TEffect}.Start"/>.</summary>
public sealed record GameStartResult;

/// <summary>Save request DTO (extend per game).</summary>
public sealed record GameSaveRequest;

/// <summary>Save operation outcome.</summary>
public sealed record GameSaveResult;

/// <summary>Load request DTO (extend per game).</summary>
public sealed record GameLoadRequest;

/// <summary>Load operation outcome.</summary>
public sealed record GameLoadResult;
