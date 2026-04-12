using application.DTOs;
using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Exceptions;
using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;


namespace Application.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;
    private readonly IFloorService _floorService;
    private readonly IUnitOfWork _uow;



    public VenueService(IUnitOfWork uow, IVenueRepository venueRepository, IFloorService floorService)
    {
        _venueRepository = venueRepository;
        _floorService = floorService;
        _uow = uow;

    }

    public List<VenueDto> GetAll(VenueFilter filter = null)
    {
        return _venueRepository.GetAll()
            .Where(v => v.UpdateStatus != 3)
            .Where(v => filter == null || string.IsNullOrEmpty(filter.Name)
                || v.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
            .Select(v => MapToDto(v))
            .ToList();
    }

    public VenueDto GetById(int id)
    {
        var venue = _venueRepository.GetById(id);
        if (venue is null || venue.UpdateStatus == 3)
            throw new NotFoundException("Venue", id); // throw NotFoundException — middleware catches it and returns 404 automatically

        return MapToDto(venue);
    }

    public Venue Create(CreateVenueRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Venue name is required");

        var venue = new Venue
        {
            Name = request.Name
            // UpdateStatus is set to 1 inside the repository
        };

        return _venueRepository.Create(venue);
    }

    public Venue Update(int id, UpdateVenueRequest request)
    {
        // Fix 3: verify it exists before updating — throws 404 if not
        var existing = _venueRepository.GetById(id);
        if (existing is null || existing.UpdateStatus == 3)
            throw new NotFoundException("Venue", id);

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Venue name is required");

        // Apply changes to the existing entity — don't replace the whole object
        existing.Name = request.Name;

        return _venueRepository.Update(existing);
    }

    public async Task<bool> DeleteVenue(int id)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            var venue = _uow.Venues.GetById(id);

            // Throw instead of returning false — cleaner flow, middleware handles 404
            if (venue is null)
                throw new NotFoundException("Venue", id);

            // cascade delete
            _floorService.DeleteFloorsByVenueId(id);

            _uow.Venues.SoftDeleteById(id);

            await _uow.SaveChangesAsync(); // ONE SAVE ONLY
            await _uow.CommitAsync();

            return true;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;  // re-throw so middleware can catch and handle it
        }
    }


    private VenueDto MapToDto(Venue v) => new VenueDto
    {
        Id = v.Id,
        Name = v.Name,
        Floors = v.Floors?
            .Where(f => f.UpdateStatus != 3)
            .Select(f => new FloorSummaryDto
            {
                Id = f.Id,
                Name = f.Name,
                Level = f.Level
            }).ToList() ?? new()
    };
}