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

        private readonly IGenericRepository<Node> _repository;
        private readonly INodeService _nodeService;

        public NodeController(IGenericRepository<Node> repository, INodeService nodeService)
        {
<<<<<<< Updated upstream
            _repository = repository;
            _nodeService = _nodeService;
=======
            _nodeService = nodeService;
>>>>>>> Stashed changes
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var node = _repository.GetById(id);

            if (node == null)
                return NotFound();

            return Ok(node);
        }

        [HttpPost]
        public IActionResult Create(Node node)
        {
            var created = _repository.Create(node);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Node node)
        {
            node.Id = id;
            return Ok(_repository.Update(node));
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
