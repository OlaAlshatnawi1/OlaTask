using Domain.Interfaces;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<T> GetAll()
    {


        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        var entity = _context.Set<T>().Find(id);

        return entity;
    }

    public T Create(T entity)
    {
        try
        {
            _context.Set<T>().Add(entity);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    public T Update(T entity)
    {
        try
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }
}
