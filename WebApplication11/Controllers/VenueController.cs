using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using application.Service.Interfaces;
using application.DTOs.Filters;

namespace WebApplication11.Controllers
{
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

        
        // Returns venue + its floors nested inside the response
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // If not found, service throws NotFoundException
            // Middleware catches it ? returns { "statusCode": 404, "message": "Venue with id 5 was not found" }
            // No null check needed here at all
            return Ok(_venueService.GetById(id));
        }

     
        [HttpPost]
        public IActionResult Create(Venue venue)
            => Ok(_venueService.Create(venue));

      
        // We assign the route id to the entity to prevent mismatch
        [HttpPut("{id}")]
        public IActionResult Update(int id, Venue venue)
        {
            venue.Id = id;
            return Ok(_venueService.Update(venue));
        }

        
        // async because DeleteVenue uses a transaction (BeginTransactionAsync)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _venueService.DeleteVenue(id)) return NotFound();
            return NoContent(); 
        }

        
        // Navigation route — get all floors that belong to venue {venueId}
        // [FromQuery] FloorFilter allows: ?name=ground&level=1
        [HttpGet("{venueId}/floors")]
        public IActionResult GetFloors(int venueId, [FromQuery] FloorFilter filter)
        {
            
            if (_venueService.GetById(venueId) == null)
                return NotFound("Venue not found");

            return Ok(_floorService.GetByVenueId(venueId, filter));
        }

        
        // Get one specific floor but only if it belongs to this venue
        [HttpGet("{venueId}/floors/{floorId}")]
        public IActionResult GetFloorById(int venueId, int floorId)
        {
            if (_venueService.GetById(venueId) == null)
                return NotFound("Venue not found");

            var floor = _floorService.GetById(floorId);

            // Double check ownership — floor must belong to this venue
            if (floor == null || floor.VenueId != venueId)
                return NotFound("Floor not found in this venue");

            return Ok(floor);
        }
    }
}