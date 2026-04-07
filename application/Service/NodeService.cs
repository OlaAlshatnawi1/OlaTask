using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;

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

    public List<Node> GetAll()
    {
        return _nodeRepository.GetAll();
    }

    public Node GetById(int id)
    {
        return _nodeRepository.GetById(id);
    }

    public Node Create(Node node)
    {
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
}