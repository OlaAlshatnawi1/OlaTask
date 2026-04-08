using application.Service.Interfaces;
using application.DTOs;
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

<<<<<<< Updated upstream
    public bool DeleteVenue(int id)
    {
        if (_venueRepository.GetById(id) is null)
            return false;

        _floorService.DeleteFloorsByVenueId(id);
=======
    public List<VenueDto> GetAll()
    {
        return _venueRepository.GetAll()
            .Where(v => v.UpdateStatus != 3)
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
>>>>>>> Stashed changes

        _venueRepository.SoftDeleteById(id);
        return true;
    }
}