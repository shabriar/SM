
using Stationary_Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Repo
{
    public class ProductUnitOfWork : IDisposable
    {
        private ProductRepository _productRepository;
        private SCHMDbContext _context;
        public ProductUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            _productRepository = new ProductRepository(_context);
        }
        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository;
            }
        }
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
