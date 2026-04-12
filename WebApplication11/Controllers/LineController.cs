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
     => Created(string.Empty, _lineService.Create(request));

    // PUT /api/line/1
    // Body: { "isTwoWay": false }
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateLineRequest request)
        => Ok(_lineService.Update(id, request));



    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _lineService.DeleteLine(id);
        return NoContent();
    }
}