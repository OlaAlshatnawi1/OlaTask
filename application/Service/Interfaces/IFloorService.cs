using application.DTOs;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface IFloorService
    {
        List<FloorDto> GetAll();
        FloorDto GetById(int id);
        Floor Create(Floor floor);
        Floor Update(Floor floor);
        bool DeleteFloor(int id);
        bool DeleteFloorsByVenueId(int venueId);
    }
}
