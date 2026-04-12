using application.DTOs;
using application.DTOs.Filters;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IFloorService
    {
        List<FloorDto> GetAll(FloorFilter filter = null);
        List<FloorDto> GetByVenueId(int venueId, FloorFilter filter = null);

        FloorDto GetById(int id);
        Floor Create(Floor floor);
        Floor Update(Floor floor);
        bool DeleteFloor(int id);
        bool DeleteFloorsByVenueId(int venueId);

    }
}
