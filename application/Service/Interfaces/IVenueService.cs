using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IVenueService
    {
        List<VenueDto> GetAll(VenueFilter filter = null);
        VenueDto GetById(int id);                         
        Venue Create(CreateVenueRequest request);          
        Venue Update(int id, UpdateVenueRequest request); 
        Task<bool> DeleteVenue(int id);


    }
}
