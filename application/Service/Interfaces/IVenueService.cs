using application.DTOs;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IVenueService
    {
        List<VenueDto> GetAll();
        VenueDto GetById(int id);
        Venue Create(Venue venue);
        Venue Update(Venue venue);
        Task<bool> DeleteVenue(int id);


    }
}
