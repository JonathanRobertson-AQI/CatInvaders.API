using CatInvaders.API.DTOs;

namespace CatInvaders.API.Services;

/// <summary>
/// Business logic for managing Cat Invaders high scores.
/// </summary>
public interface IHighScoreService
{
    /// <summary>
    /// Creates and persists a new high score entry.
    /// </summary>
    /// <param name="request">The validated create request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The persisted high score response, including server-assigned fields.</returns>
    Task<HighScoreResponse> CreateAsync(
        CreateHighScoreRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the top high scores, optionally filtered by a minimum score.
    /// </summary>
    /// <param name="query">Query parameters (limit, minimumScore).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<HighScoreResponse>> GetTopAsync(
        GetHighScoresQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single high score by ID, or <c>null</c> if not found.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<HighScoreResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
