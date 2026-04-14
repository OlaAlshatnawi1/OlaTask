using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Service.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
        => Ok(_nodeService.GetById(id));

    [HttpPost]
    public IActionResult Create(CreateNodeRequest request)
    {
        var created = _nodeService.Create(request);
        return Created(string.Empty, _nodeService.GetById(created.Id));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateNodeRequest request)
    {
        _nodeService.Update(id, request);
        return Ok(_nodeService.GetById(id));
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _nodeService.DeleteNode(id);
        return NoContent();
    }

    [HttpGet("{nodeId}/lines")]
    public IActionResult GetLines(int nodeId, [FromQuery] LineFilter filter)
        => Ok(_lineService.GetByNodeId(nodeId, filter));
}