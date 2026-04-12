using System.Net;
using System.Text.Json;
using application.DTOs;
using application.Exceptions;

namespace WebApplication11.Middleware;

// Middleware in ASP.NET is a class that sits in the request pipeline
// Every single HTTP request passes THROUGH this class
// We wrap the next() call in a try/catch to intercept any unhandled exception
public class ErrorHandlingMiddleware
{
    // _next is the next piece of middleware (or the controller) in the pipeline
    // We must call it to allow the request to continue normally
    private readonly RequestDelegate _next;

    // ILogger lets us write to the console / log file
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    // IHostEnvironment tells us if we're in Development or Production
    // In Development we show full exception details; in Production we hide them
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

    // InvokeAsync is called automatically by ASP.NET for every request
    // context = the current HTTP request/response
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass the request to the next middleware/controller
            // If nothing throws, everything works normally — this middleware is invisible
            await _next(context);
        }
        catch (Exception ex)
        {
            // Something threw an exception anywhere in the pipeline
            // Log it so we can investigate later
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

            // Build and send the JSON error response
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Determine status code and message based on exception type
        // This is the "routing" of exceptions to HTTP status codes
        var (statusCode, message) = exception switch
        {
            // Our custom NotFoundException → 404 Not Found
            NotFoundException notFound =>
                (HttpStatusCode.NotFound, notFound.Message),

            // Our custom ValidationException → 400 Bad Request
            ValidationException validation =>
                (HttpStatusCode.BadRequest, validation.Message),

            // EF Core DbUpdateException → 400 (usually a constraint violation)
            // InnerException has the actual SQL error message
            Microsoft.EntityFrameworkCore.DbUpdateException dbEx =>
                (HttpStatusCode.BadRequest,
                 dbEx.InnerException?.Message ?? "A database error occurred"),

            // ArgumentException → 400 (bad arguments passed to a method)
            ArgumentException argEx =>
                (HttpStatusCode.BadRequest, argEx.Message),

            // Anything else → 500 Internal Server Error
            // We give a safe generic message — never expose internals to the client
            _ => (HttpStatusCode.InternalServerError,
                  "An unexpected error occurred")
        };

        // Set the response Content-Type to JSON
        context.Response.ContentType = "application/json";

        // Set the HTTP status code (404, 400, 500 etc.)
        context.Response.StatusCode = (int)statusCode;

        // Build the ErrorResponse DTO
        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,

            // Only include the full exception detail in Development
            // In Production this is null — never leaks stack traces to the client
            Detail = _env.IsDevelopment() ? exception.ToString() : null
        };

        // Serialize to JSON with camelCase (standard for APIs)
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Write the JSON to the HTTP response body
        await context.Response.WriteAsync(json);
    }
}