using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository;

public class LineRepository : GenericRepository<Line>, ILineRepository
{
    private readonly AppDbContext _context;

    public LineRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Line> GetAll()
    {
        return _context.Set<Line>().ToList();
    }

    public Line GetById(int id)
    {
        return _context.Set<Line>().Find(id);
    }

    public Line Create(Line entity)
    {
        try
        {
            entity.UpdateStatus = 1;
            _context.Set<Line>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public Line Update(Line entity)
    {
        try
        {
            entity.UpdateStatus = 2;
            _context.Set<Line>().Update(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public void SoftDeleteById(int id)
    {
        var line = _context.Set<Line>().Find(id);
        if (line is null) return;

        line.UpdateStatus = 3;
        _context.SaveChanges();
=======
        line.UpdateStatus = 3;
>>>>>>> Stashed changes
    }

    public void SoftDeleteByNodeIds(IEnumerable<int> nodeIds)
    {
        var lines = _context.Set<Line>()
            .Where(l => nodeIds.Contains(l.FirstNodeId) || nodeIds.Contains(l.SecondNodeId))
            .ToList();

        if (lines.Count == 0) return;

        foreach (var line in lines)
            line.UpdateStatus = 3;

        _context.SaveChanges();
    }

    public List<Line> GetByNodeId(int nodeId)
    {
        return _context.Set<Line>()
            .Where(l => (l.FirstNodeId == nodeId || l.SecondNodeId == nodeId)
                        && l.UpdateStatus != 3)
            .ToList();
    }

}