namespace application.DTOs.Filters;
public class LineFilter
{
    public int? FirstNodeId { get; set; }
    public int? SecondNodeId { get; set; }
    public bool? IsTwoWay { get; set; }
}