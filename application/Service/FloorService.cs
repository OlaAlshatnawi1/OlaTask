using application.Service.Interfaces;
using application.DTOs;
using Domain.Interfaces;

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

<<<<<<< Updated upstream
    // delete one floor → use NodeService to delete its nodes and lines first
=======
    public List<FloorDto> GetAll()
    {
        return _floorRepository.GetAll()
            .Where(f => f.UpdateStatus != 3 )
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

>>>>>>> Stashed changes
    public bool DeleteFloor(int id)
    {
        if (_floorRepository.GetById(id) is null)
            return false;

        // delete nodes and their lines using NodeService
        _nodeService.DeleteNodesByFloorId(id);

        // then delete the floor itself
        _floorRepository.SoftDeleteById(id);
        return true;
    }

    // delete all floors in one venue → use NodeService to delete their nodes and lines first
    public bool DeleteFloorsByVenueId(int venueId)
    {
        // get all floor ids in this venue
        var floorIds = _floorRepository.GetIdsByVenueId(venueId).ToList();
        if (floorIds.Count == 0) return true;

        // delete nodes and their lines using NodeService
        _nodeService.DeleteNodesByFloorIds(floorIds);

        // then delete the floors themselves
        _floorRepository.SoftDeleteByVenueId(venueId);
        return true;
    }
}