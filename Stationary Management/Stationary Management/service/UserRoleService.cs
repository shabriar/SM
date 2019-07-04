using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCHM.Entities;
using SCHM.Repo;


namespace SCHM.Services
{
    public class UserRoleService
    {
        private SCHMDbContext _context;
        private UserRoleUnitOfWork _roleUnitOfWork;
        //private RoleTaskCheckBoxModel _roleTaskCheckBoxModel;

        public UserRoleService()
        {
            _context = new SCHMDbContext();
            _roleUnitOfWork = new UserRoleUnitOfWork(_context);
            // _roleTaskCheckBoxModel=new RoleTaskCheckBoxModel();

        }

        public IEnumerable<UserRole> GetAllRoles()
        {
            return _roleUnitOfWork.RoleRepository.GetAllRoles().ToList();
        }

        public UserRole GetRole(int id)
        {
            return _roleUnitOfWork.RoleRepository.GetById(id);
        }

        public void DeleteRole(int id, int authorizeId)
        {
            _roleUnitOfWork.RoleRepository.Disable(id);
            _roleUnitOfWork.Save(authorizeId.ToString());
        }

        public int GetCount()
        {

            return _roleUnitOfWork.RoleRepository.GetAll().Count();

        }
        public void AddRole(string roleName, List<UserRolePermission> rolePermissionList, int authorizeId)
        {

            var role = new UserRole()
            {
                RoleName = roleName,
                Status = 1,
                RolePermissionCollection = rolePermissionList
            };

            // role.RoleTasks = roleTaskList;
            _roleUnitOfWork.RoleRepository.Add(role);
            _roleUnitOfWork.Save(authorizeId.ToString());

        }
        public void EditRole(int id, string roleName, List<UserRolePermission> rolePermissionList, int authorizeId)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(id);
            #region delete existing role pernmissions
            var rolePermissions = role.RolePermissionCollection.ToList();
            foreach (var removePermission in rolePermissions)
            {
                _roleUnitOfWork.RoleTaskRepository.DeleteFromDb(removePermission);
            }
            _roleUnitOfWork.Save();
            #endregion

            role.RoleName = roleName;
            role.RolePermissionCollection = rolePermissionList;
            _roleUnitOfWork.RoleRepository.Update(role);
            _roleUnitOfWork.Save(authorizeId.ToString());

        }
        public bool IsRoleNameExist(string RoleName, string InitialRoleName)
        {
            return _roleUnitOfWork.RoleRepository.IsRoleNameExist(RoleName, InitialRoleName);
        }

        public UserRole GetUserRoleByName(string name)
        {
            return _roleUnitOfWork.RoleRepository.GetRoleByRoleName(name);
        }
        public void Dispose()
        {
            _roleUnitOfWork.Dispose();
        }
    }
   
   
}
