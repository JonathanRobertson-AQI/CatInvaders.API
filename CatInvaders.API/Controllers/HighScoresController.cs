using CatInvaders.API.DTOs;
using CatInvaders.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatInvaders.API.Controllers;

/// <summary>
/// Manages Cat Invaders high score entries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class HighScoresController : ControllerBase
{
    private readonly IHighScoreService _service;
    private readonly ILogger<HighScoresController> _logger;

    /// <inheritdoc cref="HighScoresController"/>
    public HighScoresController(IHighScoreService service, ILogger<HighScoresController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Submits a new high score entry.
    /// </summary>
    /// <param name="request">The high score data to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The newly created high score, including its server-assigned ID and timestamp.</returns>
    /// <response code="201">High score created successfully.</response>
    /// <response code="400">Request validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(HighScoreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHighScore(
        [FromBody] CreateHighScoreRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating high score for player '{PlayerName}' with score {Score}",
            request.PlayerName, request.Score);

        var response = await _service.CreateAsync(request, cancellationToken);

        _logger.LogInformation("High score created with Id {Id}", response.Id);

        return CreatedAtAction(
            nameof(GetHighScoreById),
            new { id = response.Id },
            response);
    }

    /// <summary>
    /// Returns the top high scores, optionally filtered by a minimum score value.
    /// Results are ordered by score descending, then by submission time ascending.
    /// </summary>
    /// <param name="query">Optional query parameters: <c>limit</c> (1–100, default 10) and <c>minimumScore</c>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of high score entries.</returns>
    /// <response code="200">Returns the list of high scores.</response>
    /// <response code="400">Query parameter validation failed.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HighScoreResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHighScores(
        [FromQuery] GetHighScoresQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Retrieving top {Limit} high scores (minimumScore: {MinimumScore})",
            query.Limit, query.MinimumScore);

        var results = await _service.GetTopAsync(query, cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Returns a single high score entry by its unique identifier.
    /// </summary>
    /// <param name="id">The GUID identifier of the high score.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching high score, or 404 if not found.</returns>
    /// <response code="200">High score found and returned.</response>
    /// <response code="404">No high score with the specified ID exists.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HighScoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHighScoreById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving high score with Id {Id}", id);

        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("High score with Id {Id} not found", id);
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "High Score Not Found",
                Detail = $"No high score with ID '{id}' was found.",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(result);
    }
}
