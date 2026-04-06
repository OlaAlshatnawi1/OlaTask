using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Interfaces;
using application.Service.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class VenueController : ControllerBase
{
    private readonly IGenericRepository<Venue> _repository;
    private readonly IVenueService _venueService;

    public VenueController(IGenericRepository<Venue> repository, IVenueService venueService)
    {
        _repository = repository;
        _venueService = venueService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var venue = _repository.GetById(id);

        if (venue == null)
            return NotFound();

        return Ok(venue);
    }

    [HttpPost]
    public IActionResult Create(Venue venue)
    {
        var created = _repository.Create(venue);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Venue venue)
    {
        venue.Id = id;
        return Ok(_repository.Update(venue));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_venueService.DeleteVenue(id))
            return NotFound();

        return NoContent();
    }
}