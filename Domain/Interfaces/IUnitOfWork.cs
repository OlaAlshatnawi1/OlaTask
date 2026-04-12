using System;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVenueRepository Venues { get; }
    IFloorRepository Floors { get; }
    INodeRepository Nodes { get; }
    ILineRepository Lines { get; }

    Task BeginTransactionAsync();
    Task<int> SaveChangesAsync();
    Task CommitAsync();
    Task RollbackAsync();
}