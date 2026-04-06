using application.Service.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;
    private readonly IFloorService _floorService;

    public VenueService(IVenueRepository venueRepository, IFloorService floorService)
    {
        _venueRepository = venueRepository;
        _floorService = floorService;
    }

    public bool DeleteVenue(int id)
    {
        if (_venueRepository.GetById(id) is null)
            return false;

        _floorService.DeleteFloorsByVenueId(id);

        _venueRepository.SoftDeleteById(id);
        return true;
    }
}