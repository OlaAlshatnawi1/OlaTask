using Domain.Interfaces;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        AppDbContext context,
        IVenueRepository venues,
        IFloorRepository floors,
        INodeRepository nodes,
        ILineRepository lines)
    {
        _context = context;
        Venues = venues;
        Floors = floors;
        Nodes = nodes;
        Lines = lines;
    }

    public IVenueRepository Venues { get; }
    public IFloorRepository Floors { get; }
    public INodeRepository Nodes { get; }
    public ILineRepository Lines { get; }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null) return;
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();

    public async Task CommitAsync()
    {
        if (_transaction == null) return;

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}