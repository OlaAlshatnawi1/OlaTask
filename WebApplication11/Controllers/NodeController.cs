using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using application.Service.Interfaces;
using application.DTOs.Filters;

namespace WebApplication11.Controllers
{
    [ApiController]
    [Route("api/node")]
    public class NodeController : ControllerBase
    {
      
        private readonly INodeService _nodeService;
        private readonly ILineService _lineService;

        public NodeController(INodeService nodeService, ILineService lineService)
        {
            _nodeService = nodeService;
            _lineService = lineService;
        }

        
        
        [HttpGet]
        public IActionResult GetAll([FromQuery] NodeFilter filter)
            => Ok(_nodeService.GetAll(filter));

        // Returns node + all its connected lines nested inside
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_nodeService.GetById(id));
        }

        
        [HttpPost]
        public IActionResult Create(Node node)
            => Ok(_nodeService.Create(node));

        
        [HttpPut("{id}")]
        public IActionResult Update(int id, Node node)
        {
            node.Id = id;
            return Ok(_nodeService.Update(node));
        }

        // Cascades: soft-deletes all lines connected to this node
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_nodeService.DeleteNode(id)) return NotFound();
            return NoContent();
        }

        // Navigation route — get all lines connected to node {nodeId}
        // A line connects two nodes, so we check both FirstNodeId and SecondNodeId
        [HttpGet("{nodeId}/lines")]
        public IActionResult GetLines(int nodeId, [FromQuery] LineFilter filter)
        {
            if (_nodeService.GetById(nodeId) == null)
                return NotFound("Node not found");

            return Ok(_lineService.GetByNodeId(nodeId, filter));
        }
    }
}