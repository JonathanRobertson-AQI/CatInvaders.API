namespace CatInvaders.API.Domain;

/// <summary>
/// Represents a high score entry in the Cat Invaders game.
/// </summary>
public sealed class HighScore
{
    /// <summary>Gets or sets the unique identifier for this high score.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the player's name.</summary>
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>Gets or sets the score achieved.</summary>
    public int Score { get; set; }

    /// <summary>Gets or sets the highest level reached (optional).</summary>
    public int? LevelReached { get; set; }

    /// <summary>Gets or sets the UTC timestamp when this score was recorded.</summary>
    public DateTime CreatedAtUtc { get; set; }
}
