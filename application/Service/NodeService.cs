using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Exceptions;
using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class NodeService : INodeService
{
    private readonly INodeRepository _nodeRepository;
    private readonly ILineService _lineService;
    private readonly IFloorRepository _floorRepository;

    public NodeService(INodeRepository nodeRepository, ILineService lineService, IFloorRepository floorRepository)
    {
        _nodeRepository = nodeRepository;
        _lineService = lineService;
        _floorRepository = floorRepository;
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

    public List<NodeDto> GetByFloorId(int floorId, NodeFilter filter = null)
    {
        var floor = _floorRepository.GetById(floorId);
        if (floor is null || floor.UpdateStatus == 3)
            throw new NotFoundException("Floor", floorId);

        return _nodeRepository.GetByFloorId(floorId)
            .Where(n => n.UpdateStatus != 3)
            .Where(n => filter == null || string.IsNullOrEmpty(filter.NodeType)
                || n.NodeType.Contains(filter.NodeType, StringComparison.OrdinalIgnoreCase))
            .Select(n => MapToDto(n))
            .ToList();
    }

    public Node Create(CreateNodeRequest request)
    {
        if (request.FloorId <= 0)
            throw new ValidationException("A valid FloorId is required");

        var node = new Node
        {
            FloorId = request.FloorId,
            X = request.X,
            Y = request.Y,
            Long = request.Long,
            Lat = request.Lat,
            NodeType = request.NodeType
        };

        return _nodeRepository.Create(node);
    }

    public Node Update(int id, UpdateNodeRequest request)
    {
        var existing = _nodeRepository.GetById(id);
        if (existing is null || existing.UpdateStatus == 3)
            throw new NotFoundException("Node", id);

        existing.X = request.X;
        existing.Y = request.Y;
        existing.Long = request.Long;
        existing.Lat = request.Lat;
        existing.NodeType = request.NodeType;

        return _nodeRepository.Update(existing);
    }

    public bool DeleteNode(int id)
    {
        var node = _nodeRepository.GetById(id);
        if (node is null || node.UpdateStatus == 3)
            throw new NotFoundException("Node", id);

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