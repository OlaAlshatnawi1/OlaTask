using Domain.Models;
using System;

namespace Domain.Interfaces
{
    public interface IFloorRepository : IGenericRepository<Floor>
    {
        List<Floor> GetAll();
        Floor GetById(int id);
        Floor Create(Floor entity);
        Floor Update(Floor entity);
        void SoftDeleteById(int id);
        void SoftDeleteByVenueId(int venueId);
        IEnumerable<int> GetIdsByVenueId(int venueId);
    }
}
