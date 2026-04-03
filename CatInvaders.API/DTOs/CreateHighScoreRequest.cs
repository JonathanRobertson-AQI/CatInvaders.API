using System.ComponentModel.DataAnnotations;

namespace CatInvaders.API.DTOs;

/// <summary>
/// Request payload for submitting a new high score.
/// </summary>
public sealed class CreateHighScoreRequest
{
    /// <summary>
    /// The name of the player. Required, maximum 50 characters.
    /// </summary>
    /// <example>JonR</example>
    [Required(AllowEmptyStrings = false, ErrorMessage = "PlayerName is required.")]
    [MaxLength(50, ErrorMessage = "PlayerName must not exceed 50 characters.")]
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// The score achieved. Must be zero or greater.
    /// </summary>
    /// <example>4200</example>
    [Range(0, int.MaxValue, ErrorMessage = "Score must be 0 or greater.")]
    public int Score { get; set; }

    /// <summary>
    /// The highest level reached during the game (optional).
    /// </summary>
    /// <example>5</example>
    public int? LevelReached { get; set; }
}
