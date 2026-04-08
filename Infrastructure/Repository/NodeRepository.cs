using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class NodeRepository : GenericRepository<Node>, INodeRepository
{
    private readonly AppDbContext _context;

    public NodeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Node> GetAll()
    {
        return _context.Set<Node>().ToList();
    }

    public Node GetById(int id)
    {
        return _context.Set<Node>().Find(id);
    }

    public Node Create(Node entity)
    {
        try
        {
            entity.UpdateStatus = 1;
            _context.Set<Node>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public Node Update(Node entity)
    {
        try
        {
            entity.UpdateStatus = 2;
            _context.Set<Node>().Update(entity);
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
        var node = _context.Set<Node>().Find(id);
        if (node is null) return;

<<<<<<< Updated upstream
        node.IsDeleted = true;
        _context.SaveChanges();
=======
        node.UpdateStatus = 3;
>>>>>>> Stashed changes
    }

    public void SoftDeleteByFloorId(int floorId)
    {
        var nodes = _context.Set<Node>()
            .Where(n => n.FloorId == floorId && n.UpdateStatus != 3)
            .ToList();

        if (nodes.Count == 0) return;

        foreach (var node in nodes)
            node.UpdateStatus = 3;

        _context.SaveChanges();
    }

    public void SoftDeleteByFloorIds(IEnumerable<int> floorIds)
    {
        var nodes = _context.Set<Node>()
            .Where(n => floorIds.Contains(n.FloorId) && n.UpdateStatus != 3)
            .ToList();

        if (nodes.Count == 0) return;

        foreach (var node in nodes)
            node.UpdateStatus = 3;

        _context.SaveChanges();
    }

    public IEnumerable<int> GetIdsByFloorId(int floorId)
    {
        return _context.Set<Node>()
            .Where(n => n.FloorId == floorId && n.UpdateStatus != 3)
            .Select(n => n.Id)
            .ToList();
    }

    public IEnumerable<int> GetIdsByFloorIds(IEnumerable<int> floorIds)
    {
        return _context.Set<Node>()
            .Where(n => floorIds.Contains(n.FloorId) && n.UpdateStatus != 3)
            .Select(n => n.Id)
            .ToList();
    }
}