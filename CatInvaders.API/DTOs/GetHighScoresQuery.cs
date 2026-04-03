using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CatInvaders.API.DTOs;

/// <summary>
/// Query parameters for retrieving a list of high scores.
/// </summary>
public sealed class GetHighScoresQuery
{
    /// <summary>
    /// Maximum number of scores to return. Defaults to 10. Must be between 1 and 100.
    /// </summary>
    /// <example>10</example>
    [FromQuery(Name = "limit")]
    [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
    public int Limit { get; set; } = 10;

    /// <summary>
    /// If specified, only scores greater than or equal to this value are returned.
    /// </summary>
    /// <example>1000</example>
    [FromQuery(Name = "minimumScore")]
    [Range(0, int.MaxValue, ErrorMessage = "MinimumScore must be 0 or greater.")]
    public int? MinimumScore { get; set; }
}
