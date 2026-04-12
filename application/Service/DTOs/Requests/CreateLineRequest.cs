namespace application.DTOs.Requests;

public class CreateLineRequest
{
    public int FirstNodeId { get; set; }
    public int SecondNodeId { get; set; }
    public bool IsTwoWay { get; set; }
}