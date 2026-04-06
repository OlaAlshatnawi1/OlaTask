using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Service.Interfaces
{
    public interface INodeService
    {
        bool DeleteNode(int id);
        bool DeleteNodesByFloorId(int floorId);
        bool DeleteNodesByFloorIds(IEnumerable<int> floorIds);

    }
}
