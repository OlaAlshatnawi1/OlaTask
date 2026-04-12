using application.DTOs;
using application.DTOs.Filters;
using application.Exceptions;
using application.Service.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace application.Service
{
    public class LineService : ILineService
    {

        private readonly ILineRepository _lineRepository;

        public LineService(ILineRepository lineRepository)
        {
            _lineRepository = lineRepository;
        }

        public List<LineDto> GetAll(LineFilter filter = null)
        {
            return _lineRepository.GetAll()
                .Where(l => l.UpdateStatus != 3)
                .Where(l => filter == null || filter.FirstNodeId == null || l.FirstNodeId == filter.FirstNodeId)
                .Where(l => filter == null || filter.SecondNodeId == null || l.SecondNodeId == filter.SecondNodeId)
                .Where(l => filter == null || filter.IsTwoWay == null || l.IsTwoWay == filter.IsTwoWay)
                .Select(l => new LineDto
                {
                    Id = l.Id,
                    FirstNodeId = l.FirstNodeId,
                    SecondNodeId = l.SecondNodeId,
                    IsTwoWay = l.IsTwoWay
                }).ToList();
        }

        public LineDto GetById(int id)
        {
            var line = _lineRepository.GetById(id);
            if (line is null || line.UpdateStatus == 3)
                throw new NotFoundException("Line", id);
            return new LineDto
            {
                Id = line.Id,
                FirstNodeId = line.FirstNodeId,
                SecondNodeId = line.SecondNodeId,
                IsTwoWay = line.IsTwoWay
            };
        }

        public Line Create(Line line)
        {
            if (line.FirstNodeId <= 0 || line.SecondNodeId <= 0)
                throw new ValidationException("Both FirstNodeId and SecondNodeId are required");
            if (line.FirstNodeId == line.SecondNodeId)
                throw new ValidationException("A line cannot connect a node to itself");
            return _lineRepository.Create(line);
        }

        public Line Update(Line line)
        {
            return _lineRepository.Update(line);
        }

        public bool DeleteLine(int id)
        {
            if (_lineRepository.GetById(id) is null)
                return false;

            _lineRepository.SoftDeleteById(id);
            return true;
        }

        public bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds)
        {
            _lineRepository.SoftDeleteByNodeIds(nodeIds);
            return true;
        }

        public List<LineDto> GetByNodeId(int nodeId)
        {
            return _lineRepository.GetByNodeId(nodeId)
                .Select(l => new LineDto
                {
                    Id = l.Id,
                    FirstNodeId = l.FirstNodeId,
                    SecondNodeId = l.SecondNodeId,
                    IsTwoWay = l.IsTwoWay
                }).ToList();
        }

       
        public List<LineDto> GetByNodeId(int nodeId, LineFilter filter = null)
        {
            return _lineRepository.GetByNodeId(nodeId)
                .Where(l => l.UpdateStatus != 3)
                // Filter by IsTwoWay if provided
                .Where(l => filter == null || filter.IsTwoWay == null
                    || l.IsTwoWay == filter.IsTwoWay)
                .Select(l => new LineDto
                {
                    Id = l.Id,
                    FirstNodeId = l.FirstNodeId,
                    SecondNodeId = l.SecondNodeId,
                    IsTwoWay = l.IsTwoWay
                }).ToList();
        }

    }
}
