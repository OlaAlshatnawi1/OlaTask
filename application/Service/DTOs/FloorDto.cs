namespace application.DTOs;

public class FloorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VenueId { get; set; }
    public int Level { get; set; }
    public List<NodeSummaryDto> Nodes { get; set; } = new();
}

