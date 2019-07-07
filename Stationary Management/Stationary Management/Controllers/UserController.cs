using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Threading.Tasks;

using System.Web.Mvc;
using System.Web.Security;
using System.Text;

using System.Configuration;
using SCHM.Web.Models;
using Stationary_Management;

namespace SCHM.Web.Controllers
{

    [Authorize]
    public class UserController : Controller
    {
        // GET: Admin/User

     
        public ActionResult Index()
        {
            return View(new UserModel().GetAllUser().ToList());
        }
        #region login logout area
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                TempData["PasswordAgedMessage"] = "";

                var loginModel = new UserLoginModel();
                // find user by username first
                var user = loginModel.GetUserByUsername(model.UserName);
                if (user!=null)
                {
                    MD5 md5Hash = MD5.Create();
                    string hashPassword = GetMd5Hash(md5Hash, model.Password);
                    var validUser = loginModel.GetValidUserByPassword(model.UserName, hashPassword);
                    //var validCredentials = await UserManager.FindAsync(model.UserName, model.Password);
                    //if (loginModel.IsLockedOut(user.Id))
                    //{
                    //    ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
                    //}
                    //else if (loginModel.GetLockoutEnabled(user.Id) && validUser == null)
                    //{
                    //    loginModel.AccessFailed(user.Id);
                    //    string message;
                    //    if (loginModel.IsLockedOut(user.Id))
                    //    {
                    //        message = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());
                    //    }
                    //    else
                    //    {
                    //        var accessFailedUser =loginModel.GetUserById(user.Id);
                    //        int accessFailedCount = loginModel.GetUserById(user.Id).AccessFailedCount??1;
                    //        int attemptsLeft =loginModel.GetMaxAccesFailedAttempt(user.Id) -accessFailedCount;

                    //        message = string.Format("Invalid password. You have {0} more attempt(s) before your account gets locked out.", attemptsLeft);

                    //    }
                    //    ModelState.AddModelError("", message);
                    //}
                    //else if (validUser == null)
                    //{
                    //    ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    //}

                    if (validUser == null)
                    {
                        ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    }
                    else
                    {
                        
                        //loginModel.ResetAccesFailedCount(validUser.Id);
                        //if (loginModel.IsPasswordValidityExpired(validUser.Id))
                        //{
                        //    TempData["PasswordAgedMessage"] = "It seems your password not changed since more than 45 days! Please Change your password now.";
                        //    return RedirectToAction("ChangePassword", "User", new { userName = user.UserName.ToLower() });
                        //}
                        //if (loginModel.IsFirstLogin(validUser.Id))
                        //{

                        //    TempData["PasswordAgedMessage"] = "Please change password at first login.";
                        //    return RedirectToAction("ChangePassword", "User", new { userName = user.UserName.ToLower() });
                        //}

                        FormsAuthentication.SetAuthCookie(validUser.Id + "|" + validUser.UserName.ToUpper() + "|" + validUser.Name + "|" + validUser.ImageFile, model.RememberMe); ;
                        Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
                        //loginModel.SaveLogin(validUser.UserName.ToUpper(), System.Web.HttpContext.Current.Session.SessionID.ToString(), true);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        } 
                    }
                } 
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                ModelState.Remove("Password");
            }
            // If we got this far, something failed, redisplay form
            
         
            return View(model);
        }
        [Authorize]
        public ActionResult LogOut()
        {
          
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        #endregion
        #region user register area
        
        public ActionResult Register()
        {

            ViewBag.Message = "";
            TempData["RegistrationSuccess"] = "";
            //ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.Status == 1).OrderBy(x => x.RoleName), "RoleId", "RoleName");
            UserRegisterModel registerModel = new UserRegisterModel();

            return View(registerModel);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegisterModel userRegister)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {

                userRegister.RegisterNewUser();
                TempData["RegistrationSuccess"] = "New user registration successfully complete! Username and Password sent to user by Email.";

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "Something went wrong! please try again";
            }
            //ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.RoleId != 1 && r.Status == 1).OrderBy(x => x.RoleName), "RoleId", "RoleName", userRegister.RoleId);
            return View(userRegister);
        }
        #endregion

        #region user edit area

     
        public ActionResult Edit(int id)
        {

            ViewBag.Message = "";
            TempData["RegistrationSuccess"] = "";
            UserRegisterModel registerModel = new UserRegisterModel(id);

            return View(registerModel);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRegisterModel userRegister)
        {
            ViewBag.Message = "";
          
                userRegister.EditUser();
                TempData["RegistrationSuccess"] = "User update successfully complete!";

                return RedirectToAction("index");
            //ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.RoleId != 1 && r.Status == 1).OrderBy(x => x.RoleName), "RoleId", "RoleName", userRegister.RoleId);
          
        }
        #endregion
        #region user password change area
        [AllowAnonymous]
        public ActionResult ChangePassword(string userName)
        {
            ViewBag.PasswordAged = "";
            ViewBag.oldPasswordNotMatched = "";
            ViewBag.PasswordHistryAlert = "";
            ChangePsswordViewModel model = new ChangePsswordViewModel();
            if (userName != null)
            {
                model.UserName = userName;
            }
            else
            {
                model.UserName = AuthenticatedUser.GetUserFromIdentity().Username;
            }
            ViewBag.PasswordAged = TempData["PasswordAgedMessage"];
            return View(model);
        }
        // POST: /Account/ChangePassword
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(ChangePsswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    if (model.NewPassword == model.OldPassword)
                    {
                        changePasswordSucceeded = false;
                    }
                    else
                    {
                        MD5 md5Hash = MD5.Create();
                        string hashOldPassword = GetMd5Hash(md5Hash, model.OldPassword);
                        string hashNewPassword = GetMd5Hash(md5Hash, model.NewPassword);


                        var user = model.GetValidUserByPassword(model.UserName, hashOldPassword);

                        if (user == null)
                        {
                            ViewBag.oldPasswordNotMatched = "Wrong Password!";
                            changePasswordSucceeded = false;
                        }


                        //if (CheckPasswordStrength(user.UserName, user.FullName, model.NewPassword) && hashNewPassword != user.PrevLastPassword && hashNewPassword != user.LastPassword)
                        //{
                        else { 
                       // model.SaveNewPassword(model.UserName, hashNewPassword);
                            changePasswordSucceeded = true;
                        }
                        //}
                        //else
                        //{
                        //    ViewBag.PasswordHistryAlert = "You can not use previously used password or Password should not be part of Name!";
                        //    changePasswordSucceeded = false;
                        //}
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded == true)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        return RedirectToAction("ChangePasswordSuccess", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Current password is incorrect or new password is invalid.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        [AllowAnonymous]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        #endregion
        #region user roles configuration


        //[Roles("Global_SupAdmin,Configuration")]
        public ActionResult Roles()
        {
            var roles = new UserRoleModel().GetAllRoles().ToList();
            return View("RoleList", roles);
        }

        //[Roles("Global_SupAdmin,Configuration")]
        public ActionResult AddRole()
        {
            var roleModel = new UserRoleModel();

            return View(roleModel);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(UserRoleModel roleModel)
        {
            try
            {
                roleModel.AddRole();
                TempData["message"] = "Successfully added Role.";
                TempData["alertType"] = "success";
            }

            catch (Exception e)
            {
                TempData["message"] = "Failed to Add Role.";
                TempData["alertType"] = "danger";
                Console.Write(e.Message);
            }

            return RedirectToAction("Roles");
        }

       
        public ActionResult EditRole(int id)
        {

            var role = new UserRoleModel(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(UserRoleModel model)
        {
            model.EditRole(model.Id);
            return RedirectToAction("Roles");
        }
        #endregion
      

        public JsonResult IsUserNameExist(string UserName, string InitialUserName)
        {
            bool isNotExist = new UserModel().IsUserNameExist(UserName, InitialUserName);
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsEmailExist(string Email, string InitialEmail)
        {
            bool isNotExist = new UserModel().IsEmailExist(Email, InitialEmail);
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsRoleNameExist(string RoleName, string InitialRoleName)
        {
            bool isNotExist = new UserRoleModel().IsRoleNameExist(RoleName, InitialRoleName);
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            new UserModel().Delete(id);
            return Json(new { msg = "Success" });
        }

        public JsonResult Active(int id)
        {
            new UserModel().Active(id);
            return Json(new { msg = "Success" });
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
       
        //public JsonResult CheckValidatePassword(string password,string userName)
        //{
        //    var user = new UserModel().GetUserByUsername(userName);
        //   // var passwordPolicyConfig = new PasswordPolicyConfigModel().GetPasswordPolicyByType(user.UserType);
        //    //bool minLCheck = passwordPolicyConfig.MinLegth > password.ToCharArray().Count() ? false : true;
        //    //int minL = passwordPolicyConfig.MinLegth;
        //    //bool maxLCheck = passwordPolicyConfig.MaxLegth <= password.ToCharArray().Count() ? false : true;
        //    //int maxL = passwordPolicyConfig.MaxLegth;
        //    //bool minNumberCheck = password.ToCharArray().Count(x => Char.IsDigit(x)) >= passwordPolicyConfig.MinNumber ? true : false;
        //    //int? minNumber = passwordPolicyConfig.MinNumber;
        //    //bool minLCharCheck = password.ToCharArray().Count(x => Char.IsLower(x)) >= passwordPolicyConfig.MinLowerCase ? true : false;
        //    //int? minLChar = passwordPolicyConfig.MinLowerCase;
        //    //bool minUCharCheck = password.ToCharArray().Count(x => Char.IsUpper(x)) >= passwordPolicyConfig.MinUpperCase ? true : false;
        //    //int? minUChar = passwordPolicyConfig.MinUpperCase;
        //    //bool minSCharCheck = password.ToCharArray().Count(x =>  !Char.IsLetterOrDigit(x)) >= passwordPolicyConfig.MinSpecialChar ? true : false;
        //    //int? minSChar = passwordPolicyConfig.MinSpecialChar;
        //    //return Json(new {
        //    //    minLCheck = minLCheck,
        //    //    minL = minL,
        //    //    maxLCheck =maxLCheck,
        //    //    maxL = maxL,
        //    //    minNumberCheck = minNumberCheck,
        //    //    minNumber = minNumber,
        //    //    minLCharCheck= minLCharCheck,
        //    //    minLChar = minLChar,
        //    //    minUCharCheck= minUCharCheck,
        //    //    minUChar= minUChar,
        //    //    minSCharCheck= minSCharCheck,
        //    //    minSChar= minSChar

        //    //});
        //}

        public JsonResult ResetPassword(int id)
        {
            string str = "";
            try
            {
                //new UserModel().ResetPassword(id);
                str = "Password Reset Successful";
            }
            catch (Exception r)
            {
                str = r.Message;
            }
            return Json(new { msg = str });
        }
    }
}