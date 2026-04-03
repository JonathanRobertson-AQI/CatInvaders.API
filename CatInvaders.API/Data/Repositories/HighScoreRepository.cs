using CatInvaders.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace CatInvaders.API.Data.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IHighScoreRepository"/>.
/// </summary>
public sealed class HighScoreRepository : IHighScoreRepository
{
    private readonly CatInvadersDbContext _db;

    /// <inheritdoc cref="IHighScoreRepository"/>
    public HighScoreRepository(CatInvadersDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task AddAsync(HighScore highScore, CancellationToken cancellationToken = default)
    {
        await _db.HighScores.AddAsync(highScore, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<HighScore>> GetTopAsync(
        int limit,
        int? minimumScore,
        CancellationToken cancellationToken = default)
    {
        var query = _db.HighScores.AsNoTracking();

        if (minimumScore.HasValue)
        {
            query = query.Where(h => h.Score >= minimumScore.Value);
        }

        return await query
            .OrderByDescending(h => h.Score)
            .ThenBy(h => h.CreatedAtUtc)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<HighScore?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.HighScores
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }
}
