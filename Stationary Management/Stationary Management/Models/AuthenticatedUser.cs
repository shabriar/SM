using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SCHM.Web.Models;

using System.Web.Security;
using SCHM.Services;
namespace SCHM.Web.Models
{
    [Authorize]
    public static class AuthenticatedUser
    {
        public static AuthenticatedUserModel GetUserFromIdentity()
        {
            var authenticatedUserData = System.Web.HttpContext.Current.User.Identity.Name;
           
            var authenticatedUserModel = new AuthenticatedUserModel
            {
                UserId = Convert.ToInt32(authenticatedUserData.Split('|')[0]),
                Username = authenticatedUserData.Split('|')[1],
                FullName = authenticatedUserData.Split('|')[2],
                ImageLink = (string.IsNullOrEmpty(authenticatedUserData.Split('|')[3]))
                    ? "~/Content/img/no-image.jpg"
                    : authenticatedUserData.Split('|')[3],
            };
            return authenticatedUserModel;
        }
        public static AuthenticatedUserModel GetUserFromIdentity(IPrincipal user)
        {
            var authenticatedUserData = user == null ? System.Web.HttpContext.Current.User.Identity.Name : user.Identity.Name;
            var authenticatedUserModel = new AuthenticatedUserModel
            {
                UserId = Convert.ToInt32(authenticatedUserData.Split('|')[0]),
                Username = authenticatedUserData.Split('|')[1],
                FullName = authenticatedUserData.Split('|')[2],
                ImageLink = (string.IsNullOrEmpty(authenticatedUserData.Split('|')[3]))
                    ? "~/Content/img/no-image.jpg"
                    : authenticatedUserData.Split('|')[3],
            };
            return authenticatedUserModel;
        }
	}
    public class AuthenticatedUserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ImageLink { get; set; }
    }
}