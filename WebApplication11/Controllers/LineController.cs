using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using application.Service.Interfaces;
using application.DTOs.Filters;

namespace WebApplication11.Controllers
{
    [ApiController]
    [Route("api/line")]
    public class LineController : ControllerBase
    {
        private readonly ILineService _lineService;

        public LineController(ILineService lineService)
        {
            _lineService = lineService;
        }

      
        [HttpGet]
        public IActionResult GetAll([FromQuery] LineFilter filter)
            => Ok(_lineService.GetAll(filter));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_lineService.GetById(id));
        }

        [HttpPost]
        public IActionResult Create(Line line)
            => Ok(_lineService.Create(line));

        [HttpPut("{id}")]
        public IActionResult Update(int id, Line line)
        {
            line.Id = id;
            return Ok(_lineService.Update(line));
        }

        // Only soft-deletes this single line, no cascade needed
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_lineService.DeleteLine(id)) return NotFound();
            return NoContent();
        }
    }
}