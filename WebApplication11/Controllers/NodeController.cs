using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Interfaces;
using application.Service.Interfaces;

namespace WebApplication11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {

        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_nodeService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var node = _nodeService.GetById(id);

            if (node == null)
                return NotFound();

            return Ok(node);
        }

        [HttpPost]
        public IActionResult Create(Node node)
        {
            var created = _nodeService.Create(node);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Node node)
        {
            node.Id = id;
            return Ok(_nodeService.Update(node));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_nodeService.DeleteNode(id))
                return NotFound();

            return NoContent();
        }
    }
}
