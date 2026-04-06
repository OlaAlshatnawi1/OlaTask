namespace Domain.Models;

public class Line
{
    public int Id { get; set; }
    public int FirstNodeId { get; set; }
    public int SecondNodeId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsTwoWay { get; set; }
}
