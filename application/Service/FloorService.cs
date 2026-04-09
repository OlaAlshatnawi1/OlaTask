using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using application.DTOs;

namespace Application.Services;

public class FloorService : IFloorService
{
    private readonly IFloorRepository _floorRepository;
    private readonly INodeService _nodeService;

    public FloorService(IFloorRepository floorRepository, INodeService nodeService)
    {
        _floorRepository = floorRepository;
        _nodeService = nodeService;
    }

    public List<FloorDto> GetAll()
    {
        return _floorRepository.GetAll()
            .Where(f => f.UpdateStatus != 3)
            .Select(f => new FloorDto
            {
                Id = f.Id,
                Name = f.Name,
                VenueId = f.VenueId,
                Level = f.Level
            })
            .ToList();
    }

    public FloorDto GetById(int id)
    {
        var floor = _floorRepository.GetById(id);
        if (floor is null || floor.UpdateStatus == 3)
            return null;
        return new FloorDto
        {
            Id = floor.Id,
            Name = floor.Name,
            VenueId = floor.VenueId,
            Level = floor.Level
        };
    }

    public Floor Create(Floor floor)
    {
        return _floorRepository.Create(floor);
    }

    public Floor Update(Floor floor)
    {
        return _floorRepository.Update(floor);
    }

    public bool DeleteFloor(int id)
    {
        if (_floorRepository.GetById(id) is null)
            return false;

        _nodeService.DeleteNodesByFloorId(id);

        _floorRepository.SoftDeleteById(id);
        return true;
    }

    public bool DeleteFloorsByVenueId(int venueId)
    {
        var floorIds = _floorRepository.GetIdsByVenueId(venueId).ToList();
        if (floorIds.Count == 0) return true;

        _nodeService.DeleteNodesByFloorIds(floorIds);

        _floorRepository.SoftDeleteByVenueId(venueId);
        return true;
    }
}