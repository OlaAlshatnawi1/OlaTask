using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using application.Service;
using Domain.Interfaces;

namespace Infrastructure.Repository;

public class FloorRepository : GenericRepository<Floor>, IFloorRepository
{
    private readonly AppDbContext _context;

    public FloorRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Floor> GetAll()
    {
        return _context.Set<Floor>().ToList();
    }

    public Floor GetById(int id)
    {
        return _context.Set<Floor>().Find(id);
    }

    public Floor Create(Floor entity)
    {
        try
        {
            _context.Set<Floor>().Add(entity);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public Floor Update(Floor entity)
    {
        try
        {
            _context.Set<Floor>().Update(entity);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }


    public void SoftDeleteById(int id)
    {
        var floor = _context.Set<Floor>().Find(id);
        if (floor is null) return;

        floor.IsDeleted = true;
    }

    public void SoftDeleteByVenueId(int venueId)
    {
        var floors = _context.Set<Floor>()
            .Where(f => f.VenueId == venueId && !f.IsDeleted)
            .ToList();

        if (floors.Count == 0) return;

        foreach (var floor in floors)
            floor.IsDeleted = true;

    }

    public IEnumerable<int> GetIdsByVenueId(int venueId)
    {
        return _context.Set<Floor>()
            .Where(f => f.VenueId == venueId && !f.IsDeleted)
            .Select(f => f.Id)
            .ToList();
    }
}