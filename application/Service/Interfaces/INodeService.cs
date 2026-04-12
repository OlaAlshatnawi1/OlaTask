using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface INodeService
    {
        List<NodeDto> GetAll(NodeFilter filter = null);
        NodeDto GetById(int id);
        List<NodeDto> GetByFloorId(int floorId, NodeFilter filter = null);
        Node Create(CreateNodeRequest request);
        Node Update(int id, UpdateNodeRequest request);
        bool DeleteNode(int id);
        bool DeleteNodesByFloorId(int floorId);
        bool DeleteNodesByFloorIds(IEnumerable<int> floorIds);

    }
}
