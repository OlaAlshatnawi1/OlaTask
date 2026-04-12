namespace application.DTOs;
public class LineSummaryDto
{
    public int Id { get; set; }
    public int FirstNodeId { get; set; }
    public int SecondNodeId { get; set; }
    public bool IsTwoWay { get; set; }
}