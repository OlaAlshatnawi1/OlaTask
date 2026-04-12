using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IVenueService
    {
        List<VenueDto> GetAll(VenueFilter filter = null);
        VenueDto GetById(int id);                          // throws NotFoundException if missing
        Venue Create(CreateVenueRequest request);           // ← request DTO now
        Venue Update(int id, UpdateVenueRequest request);  // ← request DTO now
        Task<bool> DeleteVenue(int id);


    }
}
