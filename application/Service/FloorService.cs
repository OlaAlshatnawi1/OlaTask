using application.Service.Interfaces;
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

    // delete one floor → use NodeService to delete its nodes and lines first
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