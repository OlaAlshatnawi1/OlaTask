namespace Domain.Models;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UpdateStatus { get; set; }

    // Navigation
    public ICollection<Floor> Floors { get; set; } = new List<Floor>();
}
