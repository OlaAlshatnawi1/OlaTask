using application.Service.Interfaces;
using Domain.Interfaces;

namespace application.Service
{
    public class LineService : ILineService
    {

        private readonly ILineRepository _lineRepository;

        public LineService(ILineRepository lineRepository)
        {
            _lineRepository = lineRepository;
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
