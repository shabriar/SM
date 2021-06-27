using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using SCHM.Entities;
using SCHM.Services;
using System.Security.Cryptography;
using System.Text;
using System.IO;

using System.Configuration;
namespace SCHM.Web.Models
{
    [NotMapped]
    public class UserRegisterModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Remote("IsUserNameExist", "User", ErrorMessage = "Username already Exist", AdditionalFields = "InitialUserName")]
        [Display(Name = "User  Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Remote("IsEmailExist", "User", ErrorMessage = "Email already Exist", AdditionalFields = "InitialEmail")]
        public string Email { get; set; }

        public string InitialUserName { get; set; }
        public string InitialEmail { get; set; }
        [Display(Name = "Mobile")]
        public string MobileNumber { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        //[Required]
        public string Gender { get; set; }
        public bool SupUser { get; set; }
        [Display(Name = "Image Link")]
        public string ImageFile { get; set; }
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFileBase { get; set; }

        [Display(Name = "User Role")]
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual UserRole UserRole { get; set; }
        public byte? Status { get; set; }


        //[Display(Name = "Department")]
        //public int? DepartmentId { get; set; }
        //[ForeignKey("DepartmentId")]
        //public virtual Department Department { get; set; }
        ////[Required]
        //[Display(Name = "Designation")]
        //public int? DesignationId { get; set; }
        //[ForeignKey("DesignationId")]
        //public virtual Designation Designation { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }
        //[Required]
        public string UserType { get; set; }
        //[Required]
        public string EmployeeType { get; set; }
        public string ExpireDateStr { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpireDate { get; set; }



        private UserService _userService;
        //private DepartmentService _departmentService;
        //private DesignationService _designationService;
        private UserRoleService _userRoleService;
        //private PasswordPolicyConfigService passwordPolicyConfigService;
        //public IEnumerable<Department> Departments { get; set; }
        //public IEnumerable<Designation> Designations { get; set; }
        public IEnumerable<UserRole> Roles { get; set; }
        public UserRegisterModel()
        {
            _userService = new UserService();
            //_departmentService = new DepartmentService();
            //_designationService = new DesignationService();
            _userRoleService = new UserRoleService();
            //passwordPolicyConfigService = new PasswordPolicyConfigService();
            //Departments = _departmentService.GetAllDepartments();
            //Designations = _designationService.GetAllDesignation();
            Roles = _userRoleService.GetAllRoles();
            ExpireDate = ExpireDateStr != "" ? Convert.ToDateTime(ExpireDateStr) : DateTime.Now;
        }

        public UserRegisterModel(int id) : this()
        {
            var userEntry = _userService.GetUserById(id);
            if (userEntry != null)
            {
                Id = userEntry.Id;
                Name = userEntry.Name;
                UserName = userEntry.UserName;
                Email = userEntry.Email;
                InitialUserName = UserName;
                InitialEmail = Email;
                MobileNumber = userEntry.MobileNumber;
                Address = userEntry.PresentAddress;
                Gender = userEntry.Gender;
                //DepartmentId = userEntry.DepartmentId;
                //DesignationId = userEntry.DesignationId;
                CardNo = userEntry.CardNo;
                RoleId = userEntry.RoleId;
                ImageFile = userEntry.ImageFile;
                UserType = userEntry.UserType;
                EmployeeType = userEntry.EmployeeType;
                ExpireDate = userEntry.ExpireDate;
            }
        }

        public void RegisterNewUser()
        {
            _userRoleService = new UserRoleService();
            MD5 md5Hash = MD5.Create();
           // var passwordPolicyConfigEntry = passwordPolicyConfigService.GetPasswordPolicyByUserType(UserType);
            //string password = passwordPolicyConfigEntry == null ? CommonMethods.RandomPassword(true, true, true, true, 9) : CommonMethods.RandomPassword(true, passwordPolicyConfigEntry.RequireUpperCase, passwordPolicyConfigEntry.RequiredNumericChar, passwordPolicyConfigEntry.RequireSpecialChar, passwordPolicyConfigEntry.MinLegth); /*CommonMethods.GeneratePassword();*/
            string password = "12345";
            string ImagePath = "";
            if (ImageFileBase != null)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(ImageFileBase.FileName);
                var fileExtension = Path.GetExtension(ImageFileBase.FileName);
                var finalFileName = Name + "_ProfileImage" + string.Format("{0:yyMMddhhmmss}", DateTime.Now) + fileExtension;
                string savePath = Path.Combine(HttpContext.Current.Server.MapPath("/Uploads/"), finalFileName);
                ImageFileBase.SaveAs(savePath);
                ImagePath = "/Uploads/" + finalFileName;

            }
            int? loggedInUserId = AuthenticatedUser.GetUserFromIdentity().UserId;
            User newUser = new User
            {
                Name = Name,
                UserName = UserName,
                Email = Email,
                Password = GetMd5Hash(md5Hash, password),
                MobileNumber = MobileNumber,
                PresentAddress = Address,
                Gender = Gender,
                SupUser = false,
                ImageFile = ImagePath,
                //DepartmentId = DepartmentId,
                //DesignationId = DesignationId,
                CardNo = CardNo,
                Status = 1,
                CreatedBy = loggedInUserId,
                UserType = UserType,
                EmployeeType = EmployeeType,
                RoleId = RoleId,
                //ExpireDate = EmployeeType != "Permanent" ? ExpireDate : null
            };
            _userService.AddUser(newUser);
            //new MailerModel().SendRegistationMail(Email, "New User Registration for SCHM Application", Name, UserName, password, ConfigurationManager.AppSettings["WebUrl"].ToString());
        }

        public void EditUser()
        {
            _userRoleService = new UserRoleService();
            string ImagePath = "";
            int? loggedInUserId = AuthenticatedUser.GetUserFromIdentity().UserId;
            User updateUser = new User
            {
                Id = Id,
                Name = Name,
                UserName = UserName,
                Email = Email,
                MobileNumber = MobileNumber,
                PresentAddress = Address,
                Gender = Gender,
                //DepartmentId = DepartmentId,
                //DesignationId = DesignationId,
                CardNo = CardNo,
                RoleId = RoleId,
                UpdatedAt = DateTime.Now,
                UpdatedBy = loggedInUserId,
                UserType = UserType,
                EmployeeType = EmployeeType,
                //ExpireDate = EmployeeType != "Permanent" ? ExpireDate : null
            };
            if (ImageFileBase != null)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(ImageFileBase.FileName);
                var fileExtension = Path.GetExtension(ImageFileBase.FileName);
                var finalFileName = Name + "_ProfileImage" + string.Format("{0:yyMMddhhmmss}", DateTime.Now) + fileExtension;
                string savePath = Path.Combine(HttpContext.Current.Server.MapPath("/Uploads/"), finalFileName);
                ImageFileBase.SaveAs(savePath);
                updateUser.ImageFile = "/Uploads/" + finalFileName;
            }
          
            _userService.EditUser(updateUser);
        }
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

    }

}