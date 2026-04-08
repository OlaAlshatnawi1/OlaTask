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
    public interface INodeService
    {
<<<<<<< Updated upstream
=======
        List<NodeDto> GetAll();
        NodeDto GetById(int id);
        Node Create(Node node);
        Node Update(Node node);
>>>>>>> Stashed changes
        bool DeleteNode(int id);
        bool DeleteNodesByFloorId(int floorId);
        bool DeleteNodesByFloorIds(IEnumerable<int> floorIds);

    }
}
