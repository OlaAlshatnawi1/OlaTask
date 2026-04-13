using System.Net;
using System.Text.Json;
using application.DTOs;
using application.Exceptions;

namespace WebApplication11.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }











    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException notFound =>
                (HttpStatusCode.NotFound, notFound.Message),

            ValidationException validation =>
                (HttpStatusCode.BadRequest, validation.Message),

            Microsoft.EntityFrameworkCore.DbUpdateException dbEx =>
                (HttpStatusCode.BadRequest,
                 dbEx.InnerException?.Message ?? "A database error occurred"),

            ArgumentException argEx =>
                (HttpStatusCode.BadRequest, argEx.Message),

            _ => (HttpStatusCode.InternalServerError,
                  "An unexpected error occurred")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        // ✅ NOW uses ApiResponse<object> instead of ErrorResponse
        // This makes errors look exactly like successes — same wrapper shape
        var response = new ApiResponse<object>
        {
            Success = false,
            StatusCode = (int)statusCode,
            Message = message,
            // Only include full exception in Development — never in Production
            Data = _env.IsDevelopment() ? exception.ToString() : null
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}