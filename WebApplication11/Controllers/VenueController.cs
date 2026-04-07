using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Interfaces;
using application.Service.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class VenueController : ControllerBase
{
    
    private readonly IVenueService _venueService;

    public VenueController( IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_venueService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var venue = _venueService.GetById(id);

        if (venue == null)
            return NotFound();

        return Ok(venue);
    }

    [HttpPost]
    public IActionResult Create(Venue venue)
    {
        var created = _venueService.Create(venue);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Venue venue)
    {
        venue.Id = id;
        return Ok(_venueService.Update(venue));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _venueService.DeleteVenue(id))
            return NotFound();

        return NoContent();
    }
}