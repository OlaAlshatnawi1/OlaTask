using Domain.Models;

namespace Domain.Interfaces;

public interface ILineRepository : IGenericRepository<Line>
{
    List<Line> GetAll();
    Line GetById(int id);
    Line Create(Line entity);
    Line Update(Line entity);
    void SoftDeleteById(int id);
    void SoftDeleteByNodeIds(IEnumerable<int> nodeIds);
}