// ??? FloorController.cs ???????????????????????????????????????????????????????
using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Service.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
        => Ok(_floorService.GetById(id));

    [HttpPost]
    public IActionResult Create(CreateFloorRequest request)
      => Created(string.Empty, _floorService.Create(request));

    // PUT /api/floor/1
    // Body: { "name": "Ground Floor Renamed", "level": 0 }
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateFloorRequest request)
        => Ok(_floorService.Update(id, request));

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _floorService.DeleteFloor(id);
        return NoContent();
    }

    [HttpGet("{floorId}/nodes")]
    public IActionResult GetNodes(int floorId, [FromQuery] NodeFilter filter)
        => Ok(_nodeService.GetByFloorId(floorId, filter));

    [HttpGet("{floorId}/nodes/{nodeId}")]
    public IActionResult GetNodeById(int floorId, int nodeId)
    {
        var node = _nodeService.GetById(nodeId);
        if (node.FloorId != floorId)
            throw new application.Exceptions.ValidationException(
                "Node does not belong to this floor");
        return Ok(node);
    }
}