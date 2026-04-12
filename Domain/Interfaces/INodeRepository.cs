using Domain.Models;
namespace Domain.Interfaces;

public interface INodeRepository : IGenericRepository<Node>
{
    List<Node> GetAll();
    Node GetById(int id);
    Node Create(Node entity);
    Node Update(Node entity);
    void SoftDeleteById(int id);
    void SoftDeleteByFloorId(int floorId);
    void SoftDeleteByFloorIds(IEnumerable<int> floorIds);
    List<Node> GetByFloorId(int floorId);
    IEnumerable<int> GetIdsByFloorId(int floorId);
    IEnumerable<int> GetIdsByFloorIds(IEnumerable<int> floorIds);
}