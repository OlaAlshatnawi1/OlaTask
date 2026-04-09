using application.DTOs;
using Domain.Models;

namespace application.Service.Interfaces
{
    public interface ILineService
    {
        List<LineDto> GetAll();
        LineDto GetById(int id);
        Line Create(Line line);
        Line Update(Line line);
        bool DeleteLine(int id);
        bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds);


    }
}
