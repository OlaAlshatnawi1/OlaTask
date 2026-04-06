using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Interfaces;
using application.Service.Interfaces;

namespace WebApplication11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineController : ControllerBase
    {

        private readonly IGenericRepository<Line> _repository;
        private readonly ILineService _lineService;

        public LineController(IGenericRepository<Line> repository, ILineService lineService)
        {
            _repository = repository;
            _lineService = lineService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var line = _repository.GetById(id);

            if (line == null)
                return NotFound();

            return Ok(line);
        }

        [HttpPost]
        public IActionResult Create(Line line)
        {
            var created = _repository.Create(line);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Line line)
        {
            line.Id = id;
            return Ok(_repository.Update(line));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_lineService.DeleteLine(id))
                return NotFound();

            return NoContent();
        }
    }
}
