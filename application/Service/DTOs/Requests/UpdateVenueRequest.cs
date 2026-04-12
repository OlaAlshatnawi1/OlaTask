namespace application.DTOs.Requests;

// Same as create for Venue — only name can change
public class UpdateVenueRequest
{
    public string Name { get; set; }
}