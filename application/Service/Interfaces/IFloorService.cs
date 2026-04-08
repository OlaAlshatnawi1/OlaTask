<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
﻿using application.DTOs;
using Domain.Models;
>>>>>>> Stashed changes

namespace application.Service.Interfaces
{
    public interface IFloorService
    {
<<<<<<< Updated upstream
=======
        List<FloorDto> GetAll();
        FloorDto GetById(int id);
        Floor Create(Floor floor);
        Floor Update(Floor floor);
>>>>>>> Stashed changes
        bool DeleteFloor(int id);
        bool DeleteFloorsByVenueId(int venueId);
    }
}
