using application.DTOs;
using application.DTOs.Filters;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IVenueService
    {
        VenueDto GetById(int id);
        Venue Create(Venue venue);
        Venue Update(Venue venue);
        Task<bool> DeleteVenue(int id);

        List<VenueDto> GetAll(VenueFilter filter = null);


    }
}
