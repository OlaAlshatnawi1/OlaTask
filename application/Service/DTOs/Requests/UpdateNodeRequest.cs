namespace application.DTOs.Requests;

// FloorId excluded — you shouldn't move a node to a different floor
public class UpdateNodeRequest
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Long { get; set; }
    public decimal Lat { get; set; }
    public string NodeType { get; set; }
}