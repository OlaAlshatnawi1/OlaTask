namespace Domain.Models;

public class Node
{
    public int Id { get; set; }
    public int FloorId { get; set; }
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Long { get; set; }
    public decimal Lat { get; set; }
    public bool IsDeleted { get; set; }
    public string NodeType { get; set; }
}
