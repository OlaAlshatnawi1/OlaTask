using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using application.DTOs;


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

    public List<VenueDto> GetAll()
    {
        return _venueRepository.GetAll()
           .Where(f => f.UpdateStatus != 3)
           .Select(v => new VenueDto
           {
               Id = v.Id,
               Name = v.Name
           })
           .ToList();
    }

    public VenueDto GetById(int id)
    {
        var venue = _venueRepository.GetById(id);
        if (venue is null || venue.UpdateStatus == 3)
            return null;
        return new VenueDto
        {
            Id = venue.Id,
            Name = venue.Name
        };
    }

    public Venue Create(Venue venue)
    {
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

            if (venue is null)
                return false;

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
            throw;
        }
    }
}