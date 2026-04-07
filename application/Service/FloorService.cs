using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;

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

    public List<Floor> GetAll()
    {
        return _floorRepository.GetAll();
    }

    public Floor GetById(int id)
    {
        return _floorRepository.GetById(id);
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