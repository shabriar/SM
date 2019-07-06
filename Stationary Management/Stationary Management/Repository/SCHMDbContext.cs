using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCHM.Entities;
using Stationary_Management.Entity;

namespace SCHM.Repo
{
   public class SCHMDbContext:DbContext
    {
        public SCHMDbContext() : base("SCHMConnectionString")
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Products> Products  { get; set; }
        public DbSet<Store> stores   { get; set; }
        public DbSet<Requisition> Requisitions  { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        
        public DbSet<AuditLog> AuditLogs { get; set; }
       

    }
}
