namespace application.DTOs;

// Generic wrapper — T is the type of data being returned
// e.g. ApiResponse<VenueDto>, ApiResponse<List<FloorDto>>, ApiResponse<object>
public class ApiResponse<T>
{
    // true = request succeeded, false = something went wrong
    public bool Success { get; set; }

    // The HTTP status code repeated in the body for client convenience
    // e.g. 200, 201, 400, 404, 500
    public int StatusCode { get; set; }

    // Human-readable message describing the result
    // Success: "OK", "Venue created successfully"
    // Error:   "Venue with id 5 was not found", "Name is required"
    public string Message { get; set; }

    // The actual payload — null when Success=false
    public T? Data { get; set; }

    // ── Static factory methods ────────────────────────────────────────────────
    // These give you a clean one-liner anywhere in the code
    // instead of typing new ApiResponse<T> { ... } every time

    // 200 OK — general success with data
    public static ApiResponse<T> Ok(T data, string message = "OK")
        => new() { Success = true, StatusCode = 200, Message = message, Data = data };

    // 201 Created — resource was created successfully
    public static ApiResponse<T> Created(T data, string message = "Created successfully")
        => new() { Success = true, StatusCode = 201, Message = message, Data = data };

    // 400 Bad Request — invalid input
    public static ApiResponse<T> BadRequest(string message)
        => new() { Success = false, StatusCode = 400, Message = message, Data = default };

    // 404 Not Found — resource doesn't exist
    public static ApiResponse<T> NotFound(string message)
        => new() { Success = false, StatusCode = 404, Message = message, Data = default };

    // 500 Server Error — unexpected crash
    public static ApiResponse<T> ServerError(string message = "An unexpected error occurred")
        => new() { Success = false, StatusCode = 500, Message = message, Data = default };
}

// Non-generic version for responses with no data body
// e.g. DELETE returning 204, or error responses
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse NoContent(string message = "Deleted successfully")
        => new() { Success = true, StatusCode = 204, Message = message, Data = null };
}