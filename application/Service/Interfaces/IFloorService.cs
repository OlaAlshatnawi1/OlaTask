using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Service.Interfaces
{
    public interface IFloorService
    {
        bool DeleteFloor(int id);
        bool DeleteFloorsByVenueId(int venueId);
    }
}
