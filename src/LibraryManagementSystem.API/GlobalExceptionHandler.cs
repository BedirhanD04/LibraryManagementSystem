using Microsoft.AspNetCore.Diagnostics;

namespace LibraryManagementSystem.API;

// IExceptionHandler: Centrally handles all unhandled exceptions in .NET 8+
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Keep actual error details (including stack trace) server-side only; do not expose to the user
        _logger.LogError(exception, "An unhandled exception occurred. {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        // Return only a generic, safe message to the user — do not leak internal details
        await httpContext.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected error occurred. Please try again later."
        }, cancellationToken);

        return true; // Marks the exception as handled to prevent downstream handlers from running
    }
}