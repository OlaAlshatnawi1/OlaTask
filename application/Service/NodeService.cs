using application.Service.Interfaces;
using application.DTOs;
using Domain.Interfaces;

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

<<<<<<< Updated upstream
=======
    public List<NodeDto> GetAll()
    {
        return _nodeRepository.GetAll()
            .Where(n => n.UpdateStatus != 3)
            .Select(n => new NodeDto
            {
                Id = n.Id,
                FloorId = n.FloorId,
                X = n.X,
                Y = n.Y,
                Long = n.Long,
                Lat = n.Lat,
                NodeType = n.NodeType
            })
            .ToList();
    }

    public NodeDto GetById(int id)
    {
        var node = _nodeRepository.GetById(id);
        if (node is null || node.UpdateStatus == 3)
            return null;

        return new NodeDto
        {
            Id = node.Id,
            FloorId = node.FloorId,
            X = node.X,
            Y = node.Y,
            Long = node.Long,
            Lat = node.Lat,
            NodeType = node.NodeType
        };
    }

    public Node Create(Node node)
    {
        return _nodeRepository.Create(node);
    }

    public Node Update(Node node)
    {
        return _nodeRepository.Update(node);
    }

>>>>>>> Stashed changes
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
}