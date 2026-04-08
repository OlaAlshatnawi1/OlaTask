namespace Domain.Models;

public class Floor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VenueId { get; set; }
    public int Level { get; set; }
    public bool IsDeleted { get; set; }
}



