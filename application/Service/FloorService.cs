using application.DTOs;
using application.DTOs.Filters;
using application.Exceptions;
using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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

    public List<FloorDto> GetAll(FloorFilter filter = null)
    {
        return _floorRepository.GetAll()
            .Where(f => f.UpdateStatus != 3)
            .Where(f => filter == null || string.IsNullOrEmpty(filter.Name)
                || f.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
            .Where(f => filter == null || filter.Level == null || f.Level == filter.Level)
            .Where(f => filter == null || filter.VenueId == null || f.VenueId == filter.VenueId)
            .Select(f => MapToDto(f))
            .ToList();
    }

    public FloorDto GetById(int id)
    {
        var floor = _floorRepository.GetById(id);
        if (floor is null || floor.UpdateStatus == 3)
            throw new NotFoundException("Floor", id);

        return MapToDto(floor);
    }

    public Floor Create(Floor floor)
    {
        if (string.IsNullOrWhiteSpace(floor.Name))
            throw new ValidationException("Floor name is required");
        if (floor.VenueId <= 0)
            throw new ValidationException("A valid VenueId is required");
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

    public List<FloorDto> GetByVenueId(int venueId)
    {
        return _floorRepository.GetByVenueId(venueId)
            .Select(f => new FloorDto
            {
                Id = f.Id,
                Name = f.Name,
                VenueId = f.VenueId,
                Level = f.Level
            }).ToList();
    }


    public List<FloorDto> GetByVenueId(int venueId, FloorFilter filter = null)
    {
        return _floorRepository.GetByVenueId(venueId)
            .Where(f => filter == null || string.IsNullOrEmpty(filter.Name)
                || f.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
            .Where(f => filter == null || filter.Level == null || f.Level == filter.Level)
            .Select(f => MapToDto(f))
            .ToList();
    }

    private FloorDto MapToDto(Floor f) => new FloorDto
    {
        Id = f.Id,
        Name = f.Name,
        VenueId = f.VenueId,
        Level = f.Level,
        Nodes = f.Nodes?
            .Where(n => n.UpdateStatus != 3)
            .Select(n => new NodeSummaryDto
            {
                Id = n.Id,
                X = n.X,
                Y = n.Y,
                NodeType = n.NodeType
            }).ToList() ?? new()
    };
}