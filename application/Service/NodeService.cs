using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using application.DTOs;
using application.DTOs.Filters;
using application.Exceptions;


namespace Application.Services;

public class NodeService : INodeService
{
    private readonly INodeRepository _nodeRepository;
    private readonly ILineService _lineService;

    public NodeService(INodeRepository nodeRepository, ILineService lineService)
    {
        _nodeRepository = nodeRepository;
        _lineService = lineService;
    }

    public List<NodeDto> GetAll(NodeFilter filter = null)
    {
        return _nodeRepository.GetAll()
            .Where(n => n.UpdateStatus != 3)
            .Where(n => filter == null || string.IsNullOrEmpty(filter.NodeType)
                || n.NodeType.Contains(filter.NodeType, StringComparison.OrdinalIgnoreCase))
            .Where(n => filter == null || filter.FloorId == null || n.FloorId == filter.FloorId)
            .Select(n => MapToDto(n))
            .ToList();
    }

    public NodeDto GetById(int id)
    {
        var node = _nodeRepository.GetById(id);
        if (node is null || node.UpdateStatus == 3)
            throw new NotFoundException("Node", id);

        return MapToDto(node);
    }

    public Node Create(Node node)
    {
        if (node.FloorId <= 0)
            throw new ValidationException("A valid FloorId is required");
       
        return _nodeRepository.Create(node);
    }

    public Node Update(Node node)
    {
        return _nodeRepository.Update(node);
    }

    public bool DeleteNode(int id)
    {
        if (_nodeRepository.GetById(id) is null)
            return false;

        _lineService.DeleteLinesByNodeIds(new[] { id });

        _nodeRepository.SoftDeleteById(id);
        return true;
    }

    public bool DeleteNodesByFloorId(int floorId)
    {
        var nodeIds = _nodeRepository.GetIdsByFloorId(floorId).ToList();
        if (nodeIds.Count == 0) return true;

        _lineService.DeleteLinesByNodeIds(nodeIds);

        _nodeRepository.SoftDeleteByFloorId(floorId);
        return true;
    }

    public bool DeleteNodesByFloorIds(IEnumerable<int> floorIds)
    {
        var nodeIds = _nodeRepository.GetIdsByFloorIds(floorIds).ToList();
        if (nodeIds.Count == 0) return true;

        _lineService.DeleteLinesByNodeIds(nodeIds);

        _nodeRepository.SoftDeleteByFloorIds(floorIds);
        return true;
    }

    public List<NodeDto> GetByFloorId(int floorId)
    {
        return _nodeRepository.GetByFloorId(floorId)
            .Select(n => new NodeDto
            {
                Id = n.Id,
                FloorId = n.FloorId,
                X = n.X,
                Y = n.Y,
                Long = n.Long,
                Lat = n.Lat,
                NodeType = n.NodeType
            }).ToList();
    }

    public List<NodeDto> GetByFloorId(int floorId, NodeFilter filter = null)
{
    return _nodeRepository.GetByFloorId(floorId)
        .Where(n => filter == null || string.IsNullOrEmpty(filter.NodeType)
            || n.NodeType.Contains(filter.NodeType, StringComparison.OrdinalIgnoreCase))
        .Select(n => MapToDto(n))
        .ToList();
}

private NodeDto MapToDto(Node n)
{
    var allLines = new List<Line>();
    if (n.LinesAsFirst != null) allLines.AddRange(n.LinesAsFirst);
    if (n.LinesAsSecond != null) allLines.AddRange(n.LinesAsSecond);

    return new NodeDto
    {
        Id = n.Id,
        FloorId = n.FloorId,
        X = n.X,
        Y = n.Y,
        Long = n.Long,
        Lat = n.Lat,
        NodeType = n.NodeType,
        Lines = allLines
            .Where(l => l.UpdateStatus != 3)
            .Select(l => new LineSummaryDto
            {
                Id = l.Id,
                FirstNodeId = l.FirstNodeId,
                SecondNodeId = l.SecondNodeId,
                IsTwoWay = l.IsTwoWay
            }).ToList()
    };
}
}