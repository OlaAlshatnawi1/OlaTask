using application.DTOs;
using application.DTOs.Filters;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface INodeService
    {
        List<NodeDto> GetAll(NodeFilter filter = null);
        NodeDto GetById(int id);
        Node Create(Node node);
        Node Update(Node node);
        bool DeleteNode(int id);
        bool DeleteNodesByFloorId(int floorId);
        bool DeleteNodesByFloorIds(IEnumerable<int> floorIds);
        List<NodeDto> GetByFloorId(int floorId, NodeFilter filter = null);

    }
}
