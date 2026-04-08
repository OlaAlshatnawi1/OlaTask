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
    public interface ILineService
    {
<<<<<<< Updated upstream
=======
        List<LineDto> GetAll();
        LineDto GetById(int id);
        Line Create(Line line);
        Line Update(Line line);
>>>>>>> Stashed changes
        bool DeleteLine(int id);
        bool DeleteLinesByNodeIds(IEnumerable<int> nodeIds);


    }
}
