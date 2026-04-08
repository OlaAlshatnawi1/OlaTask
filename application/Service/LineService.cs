using application.Service.Interfaces;
using Domain.Interfaces;

namespace application.Service
{
    using application.DTOs;

    public class LineService : ILineService
    {

        private readonly ILineRepository _lineRepository;

        public LineService(ILineRepository lineRepository)
        {
            _lineRepository = lineRepository;
        }

<<<<<<< Updated upstream
=======
        public List<LineDto> GetAll()
        {
            return _lineRepository.GetAll()
                .Where(l => l.UpdateStatus != 3)
                .Select(l => new LineDto
                {
                    Id = l.Id,
                    FirstNodeId = l.FirstNodeId,
                    SecondNodeId = l.SecondNodeId,
                    IsTwoWay = l.IsTwoWay
                })
                .ToList();
        }

        public LineDto GetById(int id)
        {
            var line = _lineRepository.GetById(id);
            if (line is null || line.UpdateStatus == 3)
                return null;

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
            return _lineRepository.Create(line);
        }

        public Line Update(Line line)
        {
            return _lineRepository.Update(line);
        }

>>>>>>> Stashed changes
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

    }
}
