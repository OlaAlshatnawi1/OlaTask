namespace application.DTOs;

// This is the object that gets serialized to JSON and sent back to the client
// Every error in the entire app returns this exact shape — consistent for the frontend
public class ErrorResponse
{
    // HTTP status code — e.g. 400, 404, 500
    public int StatusCode { get; set; }

    // Human-readable message safe to show the user
    // e.g. "Venue not found" or "An unexpected error occurred"
    public string Message { get; set; }

    // Technical detail — only included in Development environment
    // In Production this is null so we don't leak stack traces
    public string? Detail { get; set; }
}