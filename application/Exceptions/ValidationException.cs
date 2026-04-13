namespace application.Exceptions;

// Custom exception for bad input — maps to HTTP 400
// Throw this when the caller sends invalid data
// e.g. creating a floor with a VenueId that doesn't exist
public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }
}
