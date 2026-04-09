using application.DTOs;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface INodeService
    {
        List<NodeDto> GetAll();
        NodeDto GetById(int id);
        Node Create(Node node);
        Node Update(Node node);
        bool DeleteNode(int id);
        bool DeleteNodesByFloorId(int floorId);
        bool DeleteNodesByFloorIds(IEnumerable<int> floorIds);

    }
}
