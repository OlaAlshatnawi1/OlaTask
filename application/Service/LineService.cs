using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using application.Exceptions;
using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace application.Service;

public class LineService : ILineService
{
    private readonly ILineRepository _lineRepository;
    private readonly INodeRepository _nodeRepository;

    public LineService(ILineRepository lineRepository, INodeRepository nodeRepository)
    {
        _lineRepository = lineRepository;
        _nodeRepository = nodeRepository;
    }

    public List<LineDto> GetAll(LineFilter filter = null)
    {
        return _lineRepository.GetAll()
            .Where(l => l.UpdateStatus != 3)
            .Where(l => filter == null || filter.FirstNodeId == null
                || l.FirstNodeId == filter.FirstNodeId)
            .Where(l => filter == null || filter.SecondNodeId == null
                || l.SecondNodeId == filter.SecondNodeId)
            .Where(l => filter == null || filter.IsTwoWay == null
                || l.IsTwoWay == filter.IsTwoWay)
            .Select(l => MapToDto(l))
            .ToList();
    }

    public LineDto GetById(int id)
    {
        var line = _lineRepository.GetById(id);

        if (line is null || line.UpdateStatus == 3)
            throw new NotFoundException("Line", id);

        return MapToDto(line);
    }

    public List<LineDto> GetByNodeId(int nodeId, LineFilter filter = null)
    {
        var node = _nodeRepository.GetById(nodeId);
        if (node is null || node.UpdateStatus == 3)
            throw new NotFoundException("Node", nodeId);

        return _lineRepository.GetByNodeId(nodeId)
            .Where(l => l.UpdateStatus != 3)
            .Where(l => filter == null || filter.IsTwoWay == null
                || l.IsTwoWay == filter.IsTwoWay)
            .Select(l => MapToDto(l))
            .ToList();
    }

    public Line Create(CreateLineRequest request)
    {
        if (request.FirstNodeId <= 0 || request.SecondNodeId <= 0)
            throw new ValidationException("Both FirstNodeId and SecondNodeId are required");
        if (request.FirstNodeId == request.SecondNodeId)
            throw new ValidationException("A line cannot connect a node to itself");

        var line = new Line
        {
            FirstNodeId = request.FirstNodeId,
            SecondNodeId = request.SecondNodeId,
            IsTwoWay = request.IsTwoWay
        };

        return _lineRepository.Create(line);
    }

    public Line Update(int id, UpdateLineRequest request)
    {
        var existing = _lineRepository.GetById(id);
        if (existing is null || existing.UpdateStatus == 3)
            throw new NotFoundException("Line", id);

        existing.IsTwoWay = request.IsTwoWay;

        return _lineRepository.Update(existing);
    }

    public bool DeleteLine(int id)
    {
        var line = _lineRepository.GetById(id);
        if (line is null || line.UpdateStatus == 3)
            throw new NotFoundException("Line", id);

        _lineRepository.SoftDeleteById(id);
        return true;
    }

    public bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds)
    {
        _lineRepository.SoftDeleteByNodeIds(nodeIds);
        return true;
    }

    private LineDto MapToDto(Line l) => new LineDto
    {
        Id = l.Id,
        FirstNodeId = l.FirstNodeId,
        SecondNodeId = l.SecondNodeId,
        IsTwoWay = l.IsTwoWay
    };
}