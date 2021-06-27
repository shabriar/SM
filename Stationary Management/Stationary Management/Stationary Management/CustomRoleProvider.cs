using SCHM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;



namespace Stationary_Management
{
    #region role provider
    public class CustomRoleProvider:RoleProvider
    {
       // private int _cacheTimeoutInMinute = 60;
        private UserService _userManagementService;

        public CustomRoleProvider()
        {
            _userManagementService=new UserService();
        }
        public override bool IsUserInRole(string userId, string roleName)
        {
            var userRoles = GetRolesForUser(userId);
            return userRoles.Contains(roleName);
        }

        public override string[] GetRolesForUser(string userid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
           
            int id = int.Parse(userid.Split('|')[0]);
           
            string[] RoleTasks = new string[] { };

            //using (TaxStampDbContext db = new TaxStampDbContext())
            //{
            //    RoleTasks = (from a in db.UserRoles
            //                 join b in db.Users on new { a.Id } equals new { b.RoleId.Value }
            //                 where b.Id.Equals(id)
            //                 select a.Task).ToArray<string>();
            //}

            //RoleTasks = _userManagementService.GetRolesByUserName(strCUser);
            RoleTasks = _userManagementService.GetRolesById(id);
            return RoleTasks;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }

    }
    #endregion
    #region role authorization
    // Roles authorization=========================================
    public class RolesAttribute : AuthorizeAttribute
    {
        // Multiple roles authorization=========================================

        public RolesAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }
        // Handle Unauthorized Request =========================================

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "ErrorHandler",
                        action = "UnAuthenticError"
                    })
                );
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
    #endregion
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : May 2018
//=======================================================================================//