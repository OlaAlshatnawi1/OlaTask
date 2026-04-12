using application.DTOs;
using application.DTOs.Filters;
using application.Service.Interfaces;
using application.DTOs;
using Domain.Interfaces;
using Domain.Models;
using application.Exceptions;


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

    public Venue Create(Venue venue)
    {
        // Validate input before hitting the DB
        if (string.IsNullOrWhiteSpace(venue.Name))
            throw new ValidationException("Venue name is required");
       
        return _venueRepository.Create(venue);
    }

    public Venue Update(Venue venue)
    {
        return _venueRepository.Update(venue);
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