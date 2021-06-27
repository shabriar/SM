using SCHM.Entities;
using SCHM.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace SCHM.Web.Models
{
    [NotMapped]
    public class UserModel:User
    {
        private UserService _userService;
        //private DepartmentService _departmentService;
        //private DesignationService _designationService;

        //public IEnumerable<Department> Departments { get; set; }
        //public IEnumerable<Designation> Designations { get; set; }

        public int CurrentUserId { get; set; }
        public UserModel()
        {
            _userService = new UserService();
            //_departmentService = new DepartmentService();
            //_designationService = new DesignationService();

            //Departments = GetAllDepartment();
            //Designations = GetAllDesignation();
            CurrentUserId = AuthenticatedUser.GetUserFromIdentity().UserId;
        }
        public IEnumerable<User> GetAllUser()
        {
            return _userService.GetAllUsers();
        }
 
        //public IEnumerable<Department> GetAllDepartment()
        //{
        //    return _departmentService.GetAllDepartments();
        //}
        //public IEnumerable<Designation> GetAllDesignation()
        //{
        //    return _designationService.GetAllDesignation();
        //}
        public User GetUserById(int id)
        {
            return _userService.GetUserById(id);
        }
        public User GetUserByUsername(string username)
        {
            return _userService.GetUserByUsername(username);
        }

      
        public bool IsUserNameExist(string UserName, string InitialUserName)
        {
            return _userService.IsUserNameExist(UserName, InitialUserName);
        }
        public bool IsEmailExist(string Email, string InitialEmail)
        {
            return _userService.IsEmailExist(Email, InitialEmail);
        }
        public void Delete(int id)
        {
            _userService.DeleteUser(id, CurrentUserId);
        }
        public void Active(int id)
        {
            _userService.ActiveUser(id, CurrentUserId);
        }

        //public void ResetPassword(int id)
        //{
        //    var user = _userService.GetUserById(id);
        //    MD5 md5Hash = MD5.Create();
        //    if (user != null)
        //    {
        //        string password = "12345";
        //        _userService.ResetPassword(id, GetMd5Hash(md5Hash, password));
        //    }
        //}

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        //internal List<string> GetAllModule()
        //{
        //    return _userService.GetAllModules();
        //}

        //internal List<string> GetAllAction()
        //{
        //    return _userService.GetAllActions();
        //}
       
    }
}