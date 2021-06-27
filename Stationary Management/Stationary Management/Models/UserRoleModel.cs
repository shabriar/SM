using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCHM.Entities;
using SCHM.Services;

namespace SCHM.Web.Models
{
    [NotMapped]
    public class UserRoleModel : UserRole
    {
        private UserRoleService _roleService { get; set; }

        public List<RoleTaskCheckBoxModel> RoleTaskList { get; set; }

        [Required]
        [Remote("IsRoleNameExist", "User", AdditionalFields = "InitialRoleName",
           ErrorMessage = "Role already Exist")]
        [Display(Name = "Role")]
        public new string RoleName
        {
            get { return base.RoleName; }
            set { base.RoleName = value; }
        }

        public int loggedInUserId { get; set; }
        public UserRoleModel()
        {
            loggedInUserId = AuthenticatedUser.GetUserFromIdentity().UserId;
            _roleService = new UserRoleService();
            this.RoleTaskList = new RoleTaskCheckBoxList().TaskList.OrderBy(x => x.PermissionCategory).ThenBy(x => x.PermissionName).ToList();
        }
        public List<UserRole> GetAllRoles()
        {
            return _roleService.GetAllRoles().Where(e => e.RoleName.ToLower() != "operator").ToList();
        }
        public UserRoleModel(int id)
            : this()
        {
            var role = _roleService.GetRole(id);
            this.Id = role.Id;
            this.RoleName = role.RoleName;
            this.Status = role.Status;
            this.RoleTaskList = new List<RoleTaskCheckBoxModel>();
            foreach (var item in new RoleTaskCheckBoxList().TaskList.OrderBy(x => x.PermissionCategory).ThenBy(x => x.PermissionName))
            {
                item.IsChecked = role.RolePermissionCollection.Any(x => x.Permission.Equals(item.PermissionName));
                this.RoleTaskList.Add(item);
            }
        }
        public void AddRole()
        {
            var taskList = this.RoleTaskList.Where(x => x.IsChecked).Select(taskItem => new UserRolePermission
            {
                Permission = taskItem.PermissionName
            }).ToList();
            _roleService.AddRole(RoleName, taskList, loggedInUserId);
        }
        public void EditRole(int id)
        {
            var taskList = this.RoleTaskList.Where(x => x.IsChecked).Select(taskItem => new UserRolePermission
            {
                Permission = taskItem.PermissionName
            }).ToList();
            _roleService.EditRole(id, RoleName, taskList, loggedInUserId);
        }
        public bool IsRoleNameExist(string RoleName, string InitialRoleName)
        {
            return _roleService.IsRoleNameExist(RoleName, InitialRoleName);
        }

    }
    #region role viewmodels
    public class RoleTaskCheckBoxModel
    {
        public string PermissionName { get; set; }
        public string PermissionCategory { get; set; }
        public bool IsChecked { get; set; }

    }
    public class RoleTaskCheckBoxList
    {
        public List<RoleTaskCheckBoxModel> TaskList = new List<RoleTaskCheckBoxModel>
        {
  
            new RoleTaskCheckBoxModel {PermissionName="Configuration", PermissionCategory = "Configuration"},
             new RoleTaskCheckBoxModel {PermissionName="Proforma_Invoice", PermissionCategory = "Forms"},
             new RoleTaskCheckBoxModel {PermissionName="Loan_Entry", PermissionCategory = "Forms"},
             new RoleTaskCheckBoxModel {PermissionName="Loan_Return", PermissionCategory = "Forms"},
        };


    }
    #endregion

}