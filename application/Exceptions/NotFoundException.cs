namespace application.Exceptions;

// Custom exception for "resource not found" situations
// When thrown anywhere in the app, the middleware will catch it
// and automatically return a 404 response — no need to check for null in controllers
public class NotFoundException : Exception
{
    // entityName = what was not found, e.g. "Venue"
    // id         = which id was looked up, e.g. 5
    // Results in message: "Venue with id 5 was not found"
    public NotFoundException(string entityName, int id)
        : base($"{entityName} with id {id} was not found")
    {
    }

    // Overload for custom messages
    // e.g. new NotFoundException("Floor does not belong to this venue")
    public NotFoundException(string message)
        : base(message)
    {
    }
}