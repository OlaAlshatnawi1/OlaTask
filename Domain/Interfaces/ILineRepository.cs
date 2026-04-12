using Domain.Models;

namespace Domain.Interfaces;

public interface ILineRepository : IGenericRepository<Line>
{
    List<Line> GetAll();
    Line GetById(int id);
    Line Create(Line entity);
    Line Update(Line entity);
    void SoftDeleteById(int id);
    List<Line> GetByNodeId(int nodeId);
    void SoftDeleteByNodeIds(IEnumerable<int> nodeIds);
}