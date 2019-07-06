
using Stationary_Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCHM.Repo
{
    public class RequisitionUnitOfWork : IDisposable
    {
        private RequisitionRepository _requisitionRepository;
        private SCHMDbContext _context;
        public RequisitionUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            _requisitionRepository = new RequisitionRepository(_context);
        }
        public RequisitionRepository RequisitionRepository => _requisitionRepository;


        public void Save(string currentUserId)
        {
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
