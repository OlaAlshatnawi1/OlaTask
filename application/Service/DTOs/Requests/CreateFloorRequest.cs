namespace application.DTOs.Requests;

// Caller provides name, which venue it belongs to, and what level it is
// Id and UpdateStatus are handled internally
public class CreateFloorRequest
{
    public string Name { get; set; }
    public int VenueId { get; set; }
    public int Level { get; set; }
}