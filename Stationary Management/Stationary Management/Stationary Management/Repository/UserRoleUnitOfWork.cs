using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Repo
{
    public class UserRoleUnitOfWork : IDisposable
    {
        private SCHMDbContext _context { get; set; }
        private UserRoleRepository _roleRepository { get; set; }
        private RoleTaskRepository _roleTaskRepository { get; set; }

        public UserRoleUnitOfWork(SCHMDbContext context)
        {
            _context = context;
            _roleRepository = new UserRoleRepository(_context);
            _roleTaskRepository = new RoleTaskRepository(_context);
        }

        public UserRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository;
            }
        }
        public RoleTaskRepository RoleTaskRepository
        {
            get
            {
                return _roleTaskRepository;
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
