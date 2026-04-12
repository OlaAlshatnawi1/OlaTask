using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class VenueRepository : GenericRepository<Venue>, IVenueRepository
{
    private readonly AppDbContext _context;

    public VenueRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Venue> GetAll()
    {
        return _context.Set<Venue>()
            .Include(v => v.Floors.Where(f => f.UpdateStatus != 3))
            .ToList();
    }

    public Venue GetById(int id)
    {
        return _context.Set<Venue>()
            .Include(v => v.Floors.Where(f => f.UpdateStatus != 3))
            .FirstOrDefault(v => v.Id == id);
    }

    public Venue Create(Venue entity)
    {
        try
        {
            entity.UpdateStatus = 1;
            _context.Set<Venue>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public Venue Update(Venue entity)
    {
        try
        {
            entity.UpdateStatus = 2;
            _context.Set<Venue>().Update(entity);
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
        var venue = _context.Set<Venue>().Find(id);
        if (venue is null) return;

        venue.UpdateStatus = 3;
        _context.SaveChanges();
    }
}