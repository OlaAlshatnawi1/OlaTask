namespace Domain.Models;

public class Floor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VenueId { get; set; }
    public int Level { get; set; }
    public int UpdateStatus { get; set; }

    // Navigation
    public Venue Venue { get; set; }
    public ICollection<Node> Nodes { get; set; } = new List<Node>();
}



