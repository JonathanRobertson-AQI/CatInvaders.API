using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CatInvaders.API.Middleware;

/// <summary>
/// Catches all unhandled exceptions and returns a consistent RFC 7807 ProblemDetails response.
/// Stack traces are never exposed in Production environments.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    /// <inheritdoc cref="GlobalExceptionMiddleware"/>
    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await WriteErrorResponseAsync(context, ex);
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
        {
            // Cannot modify headers once streaming has begun
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var detail = _environment.IsDevelopment()
            ? ex.Message
            : "An unexpected error occurred. Please try again later.";

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}

/// <summary>
/// Extension method for registering <see cref="GlobalExceptionMiddleware"/>.
/// </summary>
public static class GlobalExceptionMiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="GlobalExceptionMiddleware"/> to the application pipeline.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
