using application.Service;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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
            entity.UpdateStatus = 1;
            _context.Set<Floor>().Add(entity);
            _context.SaveChanges();
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
            entity.UpdateStatus = 2;
            _context.Set<Floor>().Update(entity);
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
        var floor = _context.Set<Floor>().Find(id);
        if (floor is null) return;

        floor.UpdateStatus = 3;
        _context.SaveChanges();
    }

    public void SoftDeleteByVenueId(int venueId)
    {
        var floors = _context.Set<Floor>()
            .Where(f => f.VenueId == venueId &&  f.UpdateStatus != 3)
            .ToList();

        if (floors.Count == 0) return;

        foreach (var floor in floors)
            floor.UpdateStatus = 3; ;

        _context.SaveChanges();

    }

    public IEnumerable<int> GetIdsByVenueId(int venueId)
    {
        return _context.Set<Floor>()
            .Where(f => f.VenueId == venueId && f.UpdateStatus != 3)
            .Select(f => f.Id)
            .ToList();
    }
}