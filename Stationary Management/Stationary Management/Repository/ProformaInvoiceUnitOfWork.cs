using SCHM.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class ProformaInvoiceUnitOfWork : IDisposable
    {
        public SCHMDbContext _context;
        public ProformaInvoiceRepository _ProformaInvoiceRepository;
        public ProformaInvoiceProductRelationRepository _proformaInvoiceProductRelationRepository;
        public ProformaInvoiceUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            _ProformaInvoiceRepository = new ProformaInvoiceRepository(_context);
            _proformaInvoiceProductRelationRepository = new ProformaInvoiceProductRelationRepository(_context);
        }
        public ProformaInvoiceRepository ProformaInvoiceRepository
        {
            get
            {
                return _ProformaInvoiceRepository;
            }
        }
        public ProformaInvoiceProductRelationRepository ProformaInvoiceProductRelationRepository
        {
            get
            {
                return _proformaInvoiceProductRelationRepository;
            }
        }
        public void Save(string loggedInUserId)
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