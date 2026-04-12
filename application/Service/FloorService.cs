// FloorService.cs — full updated implementation
using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Exceptions;
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

    public List<FloorDto> GetAll(FloorFilter filter = null)
    {
        return _floorRepository.GetAll()
            .Where(f => f.UpdateStatus != 3)       // Fix 2
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

        // Fix 3: throw so the response wrapper returns a proper 404 JSON
        if (floor is null || floor.UpdateStatus == 3)
            throw new NotFoundException("Floor", id);

        return MapToDto(floor);
    }

    public List<FloorDto> GetByVenueId(int venueId, FloorFilter filter = null)
    {
        return _floorRepository.GetByVenueId(venueId)
            // Fix 2: soft-deleted floors excluded from nested navigation results too
            .Where(f => f.UpdateStatus != 3)
            .Where(f => filter == null || string.IsNullOrEmpty(filter.Name)
                || f.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
            .Where(f => filter == null || filter.Level == null || f.Level == filter.Level)
            .Select(f => MapToDto(f))
            .ToList();
    }

    // Fix 1: maps CreateFloorRequest → Floor domain model internally
    public Floor Create(CreateFloorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Floor name is required");
        if (request.VenueId <= 0)
            throw new ValidationException("A valid VenueId is required");

        var floor = new Floor
        {
            Name = request.Name,
            VenueId = request.VenueId,
            Level = request.Level
            // UpdateStatus set to 1 inside the repository
        };

        return _floorRepository.Create(floor);
    }

    // Fix 1: only Name and Level can be updated — VenueId is immutable after creation
    public Floor Update(int id, UpdateFloorRequest request)
    {
        var existing = _floorRepository.GetById(id);
        if (existing is null || existing.UpdateStatus == 3)
            throw new NotFoundException("Floor", id);   // Fix 3

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Floor name is required");

        existing.Name = request.Name;
        existing.Level = request.Level;

        return _floorRepository.Update(existing);
    }

    public bool DeleteFloor(int id)
    {
        var floor = _floorRepository.GetById(id);
        if (floor is null || floor.UpdateStatus == 3)
            throw new NotFoundException("Floor", id);   // Fix 3

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

    private FloorDto MapToDto(Floor f) => new FloorDto
    {
        Id = f.Id,
        Name = f.Name,
        VenueId = f.VenueId,
        Level = f.Level,
        Nodes = f.Nodes?
            // Fix 2: exclude soft-deleted nodes inside nested floor response
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