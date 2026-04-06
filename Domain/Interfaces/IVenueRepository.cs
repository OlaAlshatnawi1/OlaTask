using Domain.Models;
namespace Domain.Interfaces;

public interface IVenueRepository : IGenericRepository<Venue>
{
    List<Venue> GetAll();
    Venue GetById(int id);
    Venue Create(Venue entity);
    Venue Update(Venue entity);
    void SoftDeleteById(int id);
}