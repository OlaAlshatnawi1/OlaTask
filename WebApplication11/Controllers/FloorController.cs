using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using application.Service.Interfaces;
using application.DTOs.Filters;

namespace WebApplication11.Controllers
{
    [ApiController]
    [Route("api/floor")]
    public class FloorController : ControllerBase
    {
        
        private readonly IFloorService _floorService;
        private readonly INodeService _nodeService;

        public FloorController(IFloorService floorService, INodeService nodeService)
        {
            _floorService = floorService;
            _nodeService = nodeService;
        }


        [HttpGet]
        public IActionResult GetAll([FromQuery] FloorFilter filter)
            => Ok(_floorService.GetAll(filter));

       
        // Returns floor + its nodes nested inside
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_floorService.GetById(id));
        }

        
        [HttpPost]
        public IActionResult Create(Floor floor)
            => Ok(_floorService.Create(floor));

        
        [HttpPut("{id}")]
        public IActionResult Update(int id, Floor floor)
        {
            floor.Id = id;
            return Ok(_floorService.Update(floor));
        }

        
        // Cascades: soft-deletes all nodes (and their lines) under this floor
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_floorService.DeleteFloor(id)) return NotFound();
            return NoContent();
        }

        
        // Navigation route — get all nodes belonging to floor {floorId}
        [HttpGet("{floorId}/nodes")]
        public IActionResult GetNodes(int floorId, [FromQuery] NodeFilter filter)
        {
            // Validate parent floor exists first
            if (_floorService.GetById(floorId) == null)
                return NotFound("Floor not found");

            return Ok(_nodeService.GetByFloorId(floorId, filter));
        }

        
        // Get a specific node only if it belongs to this floor
        [HttpGet("{floorId}/nodes/{nodeId}")]
        public IActionResult GetNodeById(int floorId, int nodeId)
        {
            if (_floorService.GetById(floorId) == null)
                return NotFound("Floor not found");

            var node = _nodeService.GetById(nodeId);

            if (node == null || node.FloorId != floorId)
                return NotFound("Node not found in this floor");

            return Ok(node);
        }
    }
}