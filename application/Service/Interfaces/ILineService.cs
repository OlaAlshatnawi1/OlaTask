using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Service.Interfaces
{
    public interface ILineService
    {
        bool DeleteLine(int id);
        bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds);


    }
}
