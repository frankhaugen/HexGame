namespace HexGame;

/// <summary>
/// Intended reliability / ordering for transport (reserved for future <c>HexGame.Transport.*</c> packages).
/// Not used by v0.1 hosting or testing packages.
/// </summary>
public enum GameDeliveryMode
{
    /// <summary>Best-effort; loss and reordering allowed.</summary>
    Unreliable,

    /// <summary>Unreliable with sequencing hints (transport-defined).</summary>
    UnreliableSequenced,

    /// <summary>Delivery guaranteed; order not guaranteed.</summary>
    ReliableUnordered,

    /// <summary>Delivery guaranteed in send order.</summary>
    ReliableOrdered,

    /// <summary>Reliable with per-channel sequencing semantics (transport-defined).</summary>
    ReliableSequenced,
}
