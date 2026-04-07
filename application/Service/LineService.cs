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

        public List<Line> GetAll()
        {
            return _lineRepository.GetAll();
        }

        public Line GetById(int id)
        {
            return _lineRepository.GetById(id);
        }

        public Line Create(Line line)
        {
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

    }
}
