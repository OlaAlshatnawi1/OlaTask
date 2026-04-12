using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IFloorService
    {
        List<FloorDto> GetAll(FloorFilter filter = null);
        FloorDto GetById(int id);
        List<FloorDto> GetByVenueId(int venueId, FloorFilter filter = null);
        Floor Create(CreateFloorRequest request);
        Floor Update(int id, UpdateFloorRequest request);
        bool DeleteFloor(int id);
        bool DeleteFloorsByVenueId(int venueId);

    }
}
