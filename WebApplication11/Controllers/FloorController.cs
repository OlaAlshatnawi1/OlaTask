using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Interfaces;
using application.Service.Interfaces;

namespace WebApplication11.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FloorController : ControllerBase
    {
        private readonly IGenericRepository<Floor> _repository;
        private readonly IFloorService _floorService;

        public FloorController(IGenericRepository<Floor> repository, IFloorService floorService)
        {
            _repository = repository;
            _floorService = floorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var floor = _repository.GetById(id);

            if (floor == null)
                return NotFound();

            return Ok(floor);
        }

        [HttpPost]
        public IActionResult Create(Floor floor)
        {
            var created = _repository.Create(floor);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Floor floor)
        {
            floor.Id = id;
            return Ok(_repository.Update(floor));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_floorService.DeleteFloor(id))
                return NotFound();

            return NoContent();
        }
    }
}
