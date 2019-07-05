using SCHM.Repo;
using Stationary_Management.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class StoreRepository : Repository<Store>
    {
        private SCHMDbContext _context;

        public StoreRepository(SCHMDbContext context) : base(context)
        {
            _context = context;
        }
    }
}