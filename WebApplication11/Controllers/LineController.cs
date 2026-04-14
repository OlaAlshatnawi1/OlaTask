using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Service.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
        => Ok(_lineService.GetById(id));

    [HttpPost]
    public IActionResult Create(CreateLineRequest request)
    {
        var created = _lineService.Create(request);
        return Created(string.Empty, _lineService.GetById(created.Id));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateLineRequest request)
    {
        _lineService.Update(id, request);
        return Ok(_lineService.GetById(id));
    }



    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _lineService.DeleteLine(id);
        return NoContent();
    }
}