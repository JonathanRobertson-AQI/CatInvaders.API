using CatInvaders.API.Data.Repositories;
using CatInvaders.API.Domain;
using CatInvaders.API.DTOs;

namespace CatInvaders.API.Services;

/// <inheritdoc />
public sealed class HighScoreService : IHighScoreService
{
    private readonly IHighScoreRepository _repository;

    /// <inheritdoc cref="IHighScoreService"/>
    public HighScoreService(IHighScoreRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<HighScoreResponse> CreateAsync(
        CreateHighScoreRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new HighScore
        {
            Id = Guid.NewGuid(),
            PlayerName = request.PlayerName.Trim(),
            Score = request.Score,
            LevelReached = request.LevelReached,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(entity, cancellationToken);

        return MapToResponse(entity);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<HighScoreResponse>> GetTopAsync(
        GetHighScoresQuery query,
        CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetTopAsync(
            query.Limit,
            query.MinimumScore,
            cancellationToken);

        return entities.Select(MapToResponse).ToList();
    }

    /// <inheritdoc />
    public async Task<HighScoreResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToResponse(entity);
    }

    private static HighScoreResponse MapToResponse(HighScore entity) =>
        new()
        {
            Id = entity.Id,
            PlayerName = entity.PlayerName,
            Score = entity.Score,
            LevelReached = entity.LevelReached,
            CreatedAtUtc = entity.CreatedAtUtc
        };
}
