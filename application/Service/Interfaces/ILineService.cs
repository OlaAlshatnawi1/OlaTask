using application.DTOs;
using application.DTOs.Filters;
using application.DTOs.Requests;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface ILineService
    {
        List<LineDto> GetAll(LineFilter filter = null);
        LineDto GetById(int id);
        List<LineDto> GetByNodeId(int nodeId, LineFilter filter = null);
        Line Create(CreateLineRequest request);
        Line Update(int id, UpdateLineRequest request);
        bool DeleteLine(int id);
        bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds);



    }
}
