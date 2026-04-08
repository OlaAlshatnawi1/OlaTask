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
    public interface IVenueService
    {
<<<<<<< Updated upstream
        bool DeleteVenue(int id);
=======
        List<VenueDto> GetAll();
        VenueDto GetById(int id);
        Venue Create(Venue venue);
        Venue Update(Venue venue);
        Task<bool> DeleteVenue(int id);
>>>>>>> Stashed changes


    }
}
