namespace Domain.Models;

public class Line
{
    public int Id { get; set; }
    public int FirstNodeId { get; set; }
    public int SecondNodeId { get; set; }
    public int UpdateStatus { get; set; }
    public bool IsTwoWay { get; set; }
    // Navigation
    public Node FirstNode { get; set; }
    public Node SecondNode { get; set; }
}
