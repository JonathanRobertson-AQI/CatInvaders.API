namespace CatInvaders.API.DTOs;

/// <summary>
/// Response payload representing a high score entry.
/// </summary>
public sealed class HighScoreResponse
{
    /// <summary>Unique identifier for this high score entry.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>The player's name.</summary>
    /// <example>JonR</example>
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>The score achieved.</summary>
    /// <example>4200</example>
    public int Score { get; set; }

    /// <summary>The highest level reached (null if not provided).</summary>
    /// <example>5</example>
    public int? LevelReached { get; set; }

    /// <summary>UTC timestamp when this score was recorded.</summary>
    /// <example>2026-04-03T12:00:00Z</example>
    public DateTime CreatedAtUtc { get; set; }
}
