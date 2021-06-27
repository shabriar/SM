using SCHM.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class CustomerUnitOfWork : IDisposable
    {
        private SCHMDbContext _context { get; set; }
        private CustomerRepository customerRepository { get; set; }
        public CustomerUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            customerRepository = new CustomerRepository(_context);
        }
        public void Save(string currentUserId)
        {
            _context.SaveChanges();
        }
        public CustomerRepository CustomerRepository
        {
            get
            {
                return customerRepository;
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}