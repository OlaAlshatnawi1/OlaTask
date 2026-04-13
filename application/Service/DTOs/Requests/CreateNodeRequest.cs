namespace application.DTOs.Requests;

public class CreateNodeRequest
{
    public int FloorId { get; set; }
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Long { get; set; }
    public decimal Lat { get; set; }
    public string NodeType { get; set; }
}