using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Service.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/venue")]
public class VenueController : ControllerBase
{
    private readonly IVenueService _venueService;
    private readonly IFloorService _floorService;

    public VenueController(IVenueService venueService, IFloorService floorService)
    {
        _venueService = venueService;
        _floorService = floorService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] VenueFilter filter)
        => Ok(_venueService.GetAll(filter));

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
        => Ok(_venueService.GetById(id));

    [HttpPost]
    public IActionResult Create(CreateVenueRequest request)
    {
        var created = _venueService.Create(request);
        return Created(string.Empty, _venueService.GetById(created.Id));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateVenueRequest request)
    {
        _venueService.Update(id, request);
        return Ok(_venueService.GetById(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _venueService.DeleteVenue(id);
        return NoContent();
    }

    [HttpGet("{venueId}/floors")]
    public IActionResult GetFloors(int venueId, [FromQuery] FloorFilter filter)
        => Ok(_floorService.GetByVenueId(venueId, filter));

    [HttpGet("{venueId}/floors/{floorId}")]
    public IActionResult GetFloorById(int venueId, int floorId)
    {
        _venueService.GetById(venueId);
        var floor = _floorService.GetById(floorId);
        if (floor.VenueId != venueId)
            throw new application.Exceptions.ValidationException(
                "Floor does not belong to this venue");
        return Ok(floor);
    }
}