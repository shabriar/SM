using SCHM.Repo;
using Stationary_Management.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class RequisitionRepository : Repository<Requisition>
    {
        private SCHMDbContext _context;

        public RequisitionRepository(SCHMDbContext context) : base(context)
        {
            _context = context;
        }
    }
}