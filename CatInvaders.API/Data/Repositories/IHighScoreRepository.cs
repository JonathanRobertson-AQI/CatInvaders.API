using CatInvaders.API.Domain;

namespace CatInvaders.API.Data.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="HighScore"/> entities.
/// </summary>
public interface IHighScoreRepository
{
    /// <summary>
    /// Persists a new high score to the data store.
    /// </summary>
    /// <param name="highScore">The entity to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(HighScore highScore, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the top <paramref name="limit"/> high scores, optionally filtered by a minimum score.
    /// Results are ordered by Score descending, then CreatedAtUtc ascending.
    /// </summary>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="minimumScore">If set, only scores &gt;= this value are included.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<HighScore>> GetTopAsync(
        int limit,
        int? minimumScore,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single high score by its identifier, or <c>null</c> if not found.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<HighScore?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
