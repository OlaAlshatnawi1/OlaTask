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

    public List<Venue> GetAll()
    {
        return _venueRepository.GetAll();
    }

    public Venue GetById(int id)
    {
        return _venueRepository.GetById(id);
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