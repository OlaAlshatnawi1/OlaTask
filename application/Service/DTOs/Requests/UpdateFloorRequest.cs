namespace application.DTOs.Requests;

// VenueId intentionally excluded — you shouldn't move a floor to a different venue
public class UpdateFloorRequest
{
    public string Name { get; set; }
    public int Level { get; set; }
}