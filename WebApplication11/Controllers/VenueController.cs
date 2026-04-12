// ─── VenueController.cs ───────────────────────────────────────────────────────
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

    // Filter intercepts Ok(...) and wraps it:
    // { "success":true, "statusCode":200, "message":"OK", "data":[...] }
    [HttpGet]
    public IActionResult GetAll([FromQuery] VenueFilter filter)
        => Ok(_venueService.GetAll(filter));

    // Service throws NotFoundException if not found
    // Middleware catches it → { "success":false, "statusCode":404, "message":"Venue with id X was not found" }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
        => Ok(_venueService.GetById(id));

    [HttpPost]
    public IActionResult Create(CreateVenueRequest request)
    => Created(string.Empty, _venueService.Create(request));

    // PUT /api/venue/1
    // Body: { "name": "New Mall Name" }
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateVenueRequest request)
        => Ok(_venueService.Update(id, request));

    // Filter wraps NoContent():
    // { "success":true, "statusCode":204, "message":"Deleted successfully", "data":null }
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
        var floor = _floorService.GetById(floorId);
        // Ownership check — throws ValidationException if wrong venue
        if (floor.VenueId != venueId)
            throw new application.Exceptions.ValidationException(
                "Floor does not belong to this venue");
        return Ok(floor);
    }
}