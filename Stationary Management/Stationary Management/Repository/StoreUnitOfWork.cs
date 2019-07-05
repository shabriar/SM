
using Stationary_Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCHM.Repo
{
    public class StoreUnitOfWork : IDisposable
    {
        private StoreRepository _storeRepository;
        private SCHMDbContext _context;
        public StoreUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            _storeRepository = new StoreRepository(_context);
        }
        public StoreRepository StoreRepository => _storeRepository;


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
