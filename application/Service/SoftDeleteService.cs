//using System;
//using System.Collections.Generic;
//using System.Linq;
//using application.Service.Interfaces;
//using Domain;
//using Domain.Interfaces;

//namespace application.Service
//{
//    public class SoftDeleteService : ISoftDeleteService
//    {
//        private readonly IVenueRepository venueRepository;




//        public SoftDeleteService(IVenueRepository venueRepository)
//        {
//            this.venueRepository = venueRepository;
//        }

//        public bool DeleteVenue(int id)
//        {

//            var venue = this.venueRepository.GetById(id);

//            if (_context.Venues.FirstOrDefault(v => v.Id == id) is null)
//                return false;

//            using var tx = _context.Database.BeginTransaction();
//            try
//            {
//                var floorIds = _context.Floors.Where(f => f.VenueId == id).Select(f => f.Id).ToList();
//                var nodeIds = _context.Nodes.Where(n => floorIds.Contains(n.FloorId)).Select(n => n.Id).ToList();

//                if (nodeIds.Count > 0)
//                {
//                    _context.Lines
//                        .Where(l => nodeIds.Contains(l.FirstNodeId) || nodeIds.Contains(l.SecondNodeId))
//                        .ExecuteUpdate(s => s.SetProperty(l => l.IsDeleted, true));

//                    _context.Nodes
//                        .Where(n => floorIds.Contains(n.FloorId))
//                        .ExecuteUpdate(s => s.SetProperty(n => n.IsDeleted, true));
//                }

//                _context.Floors
//                    .Where(f => f.VenueId == id)
//                    .ExecuteUpdate(s => s.SetProperty(f => f.IsDeleted, true));

//                _context.Venues
//                    .Where(v => v.Id == id)
//                    .ExecuteUpdate(s => s.SetProperty(v => v.IsDeleted, true));

//                tx.Commit();
//                return true;
//            }
//            catch
//            {
//                tx.Rollback();
//                throw;
//            }
//        }

//        public bool DeleteFloor(int id)
//        {
//            if (_context.Floors.FirstOrDefault(f => f.Id == id) is null)
//                return false;

//            using var tx = _context.Database.BeginTransaction();
//            try
//            {
//                var nodeIds = _context.Nodes.Where(n => n.FloorId == id).Select(n => n.Id).ToList();

//                if (nodeIds.Count > 0)
//                {
//                    _context.Lines
//                        .Where(l => nodeIds.Contains(l.FirstNodeId) || nodeIds.Contains(l.SecondNodeId))
//                        .ExecuteUpdate(s => s.SetProperty(l => l.IsDeleted, true));

//                    _context.Nodes
//                        .Where(n => n.FloorId == id)
//                        .ExecuteUpdate(s => s.SetProperty(n => n.IsDeleted, true));
//                }

//                _context.Floors
//                    .Where(f => f.Id == id)
//                    .ExecuteUpdate(s => s.SetProperty(f => f.IsDeleted, true));

//                tx.Commit();
//                return true;
//            }
//            catch
//            {
//                tx.Rollback();
//                throw;
//            }
//        }

//        public bool DeleteNode(int id)
//        {
//            if (_context.Nodes.FirstOrDefault(n => n.Id == id) is null)
//                return false;

//            using var tx = _context.Database.BeginTransaction();
//            try
//            {
//                _context.Lines
//                    .Where(l => l.FirstNodeId == id || l.SecondNodeId == id)
//                    .ExecuteUpdate(s => s.SetProperty(l => l.IsDeleted, true));

//                _context.Nodes
//                    .Where(n => n.Id == id)
//                    .ExecuteUpdate(s => s.SetProperty(n => n.IsDeleted, true));

//                tx.Commit();
//                return true;
//            }
//            catch
//            {
//                tx.Rollback();
//                throw;
//            }
//        }

//        public bool DeleteLine(int id)
//        {
//            if (_context.Lines.FirstOrDefault(l => l.Id == id) is null)
//                return false;

//            _context.Lines
//                .Where(l => l.Id == id)
//                .ExecuteUpdate(s => s.SetProperty(l => l.IsDeleted, true));

//            return true;
//        }
//    }
//}
