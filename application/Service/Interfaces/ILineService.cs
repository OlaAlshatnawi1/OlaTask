using application.DTOs;
using application.DTOs.Filters;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface ILineService
    {
        List<LineDto> GetAll(LineFilter filter = null);
        LineDto GetById(int id);
        Line Create(Line line);
        Line Update(Line line);
        bool DeleteLine(int id);
        bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds);
        List<LineDto> GetByNodeId(int nodeId);
        List<LineDto> GetByNodeId(int nodeId, LineFilter filter = null);



    }
}
