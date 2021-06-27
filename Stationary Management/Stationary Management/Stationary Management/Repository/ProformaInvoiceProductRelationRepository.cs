using SCHM.Repo;
using Stationary_Management.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class ProformaInvoiceProductRelationRepository : BaseRepository<ProformaInvoiceProductRelation>
    {
        private SCHMDbContext _context;
        public ProformaInvoiceProductRelationRepository(SCHMDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ProformaInvoiceProductRelation> GetAllByPFI(int id)
        {
            return _context.ProformaInvoiceProductRelations.Where(s => s.PfiId == id).ToList();
        }

        public void GetAllPfi(string pfiNumber, int? buyerId, int? sellerId)
        {
            throw new NotImplementedException();
        }
        public ProformaInvoiceProductRelation GetPIProductRelationByCustomerId(int customerId, int productId)
        {
            var piProductRelation = _context.ProformaInvoiceProductRelations.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ProformaInvoice.BuyerId == customerId && x.ProductId == productId);
            return piProductRelation;
        }
        public ProformaInvoiceProductRelation GetPIProductRelationById(int piId, int productId)
        {
            return _context.ProformaInvoiceProductRelations.FirstOrDefault(x => x.PfiId == piId && x.ProductId == productId);
        }

    }
}