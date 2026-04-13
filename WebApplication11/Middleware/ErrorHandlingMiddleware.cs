using System.Net;
using System.Text.Json;
using application.DTOs;
using application.Exceptions;

namespace WebApplication11.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

        var response = new ApiResponse<object>
        {
            Success = false,
            StatusCode = (int)statusCode,
            Message = message,
            // Always return an empty array for errors to avoid leaking stack traces.
            Data = Array.Empty<object>()
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}