
using SCHM.Entities;
using SCHM.Repo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Services
{
    public class UserService : IDisposable
    {
        private SCHMDbContext _context;
        private UserUnitOfWork _userUnitOfWork;
        private UserRoleUnitOfWork _userRoleUnitOfWork;
        //private DepartmentUnitOfWork _departmentUnitOfWork;
        //private DesignationUnitOfWork _designationUnitOfWork;


        public UserService()
        {
            _context = new SCHMDbContext();
            _userUnitOfWork = new UserUnitOfWork(_context);
            _userRoleUnitOfWork = new UserRoleUnitOfWork(_context);
            //_departmentUnitOfWork = new DepartmentUnitOfWork(_context);
            //_designationUnitOfWork = new DesignationUnitOfWork(_context);
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _userUnitOfWork.UserRepository.GetAllUsers();

        }

        public List<AuditLog> GetAllAuditLogsByUserIdDate(int currentUserId, DateTime lastDate)
        {
            return _userUnitOfWork.UserRepository.GetAllAuditLogsByUserIdDate(currentUserId, lastDate);
        }

        public void AddUser(User user)
        {
            User newUser = new User
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                MobileNumber = user.MobileNumber,
                PhoneExt = user.PhoneExt,
                PresentAddress = user.PresentAddress,
                Gender = user.Gender,
                SupUser = user.SupUser,
                ImageFile = user.ImageFile,
                RoleId = user.RoleId,
                //DepartmentId = user.DepartmentId,
                //DesignationId = user.DesignationId,
                LastPassword = user.Password,
                LastPassChangeDate = DateTime.Now,
                PasswordChangedCount = 0,
                LockoutEnabled = true,
                LockoutEndDateUtc = DateTime.Now,
                AccessFailedCount = 0,
                CardNo = user.CardNo,
                Status = 1,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                UserType = user.UserType,
                EmployeeType = user.EmployeeType,
                ExpireDate = user.ExpireDate,

            };
            _userUnitOfWork.UserRepository.Add(newUser);
            _userUnitOfWork.Save(user.CreatedBy.ToString());
        }

        //public List<LoginRecord> GetLast7DaysLoginByUserId(string currentUserId)
        //{
        //    return _userUnitOfWork.LoginRecordRepository.GetLast7DaysLoginByUserId(currentUserId);
        //}



        public void EditUser(User updateUser)
        {
            User userEntry = _userUnitOfWork.UserRepository.GetUserById(updateUser.Id);
            if (userEntry != null)
            {
                userEntry.Name = updateUser.Name;
                userEntry.UserName = updateUser.UserName;
                userEntry.Email = updateUser.Email;
                userEntry.MobileNumber = updateUser.MobileNumber;
                userEntry.PresentAddress = updateUser.PresentAddress;
                userEntry.Gender = updateUser.Gender;
                //userEntry.DepartmentId = updateUser.DepartmentId;
                //userEntry.DesignationId = updateUser.DesignationId;
                userEntry.CardNo = updateUser.CardNo;
                userEntry.RoleId = updateUser.RoleId;
                userEntry.UpdatedAt = updateUser.UpdatedAt;
                userEntry.UpdatedBy = updateUser.UpdatedBy;
                userEntry.UserType = updateUser.UserType;
                userEntry.EmployeeType = updateUser.EmployeeType;
                userEntry.ExpireDate = updateUser.ExpireDate;

                if (updateUser.ImageFile != string.Empty)
                {
                    userEntry.ImageFile = updateUser.ImageFile;
                }

            }

            _userUnitOfWork.Save(updateUser.UpdatedBy.ToString());
        }

        //public IEnumerable<AuditLog> GetAllAuditLogs()
        //{
        //    return _userUnitOfWork.UserRepository.GetAllAuditLogs();
        //}

        //public List<string> GetAllActions()
        //{
        //    return _userUnitOfWork.UserRepository.GetAllActions();
        //}

        //public List<string> GetAllModules()
        //{
        //    return _userUnitOfWork.UserRepository.GetAllModules();
        //}

        //public IPagedList<AuditLog> GetAllAuditLogs(int page, int pageSize,string dateTo, string dateFrom, string selectedAction, string selectedModule, int? userId)
        //{
        //    return _userUnitOfWork.UserRepository.GetAllAuditLogs(page,pageSize,dateTo,dateFrom,selectedAction,selectedModule,userId);
        //}



        public string[] GetRolesById(int id)
        {
            // return _userUnitOfWork.UserRepository.GetUserById(id).UserRole.RolePermissions.Select(e=>e.Permission).ToArray<string>();
            return _userRoleUnitOfWork.RoleRepository.GetRolesById(id);
        }

        //public void SaveLogin(string userId, string sessionId, bool loggedIn)
        //{
        //    LoginRecord login = new LoginRecord
        //    {
        //        UserId = userId,
        //        SessionId = sessionId,
        //        LoggedIn = loggedIn,
        //        LoggedInDateTime = DateTime.Now
        //    };
        //    _userUnitOfWork.LoginRecordRepository.Add(login);
        //    _userUnitOfWork.Save();
        //}
        public IEnumerable<User> GetAllUser()
        {
            return _userUnitOfWork.UserRepository.GetAll();
        }

        //public Department GetDepartment(int id)
        //{
        //    return _departmentUnitOfWork.DepartmentRepository.GetById(id);
        //}
        //public Designation GetDesignation(int id)
        //{
        //    return _designationUnitOfWork.DesignationRepository.GetById(id);
        //}
        public User GetUserById(int id)
        {
            return _userUnitOfWork.UserRepository.GetById(id);
        }
        public User GetUserByUsername(string username)
        {
            return _userUnitOfWork.UserRepository.GetUserByUsername(username);
        }
        public bool CheckUsernameIsValid(string username)
        {
            return _userUnitOfWork.UserRepository.CheckUsernameIsValid(username);
        }
        public User GetValidUserByPassword(string username, string password)
        {
            return _userUnitOfWork.UserRepository.GetValidUserByPassword(username, password);
        }
        public bool IsUserNameExist(string UserName, string InitialUserName)
        {
            return _userUnitOfWork.UserRepository.IsUserNameExist(UserName, InitialUserName);
        }
        public bool IsEmailExist(string Email, string InitialEmail)
        {
            return _userUnitOfWork.UserRepository.IsEmailExist(Email, InitialEmail);
        }
        public void DeleteUser(int id, int authorizeId)
        {
            _userUnitOfWork.UserRepository.Disable(id);
            _userUnitOfWork.Save(authorizeId.ToString());
        }
        public void ActiveUser(int id, int authorizeId)
        {
            _userUnitOfWork.UserRepository.Enable(id);
            _userUnitOfWork.Save(authorizeId.ToString());
        }
        public bool IsLockedOut(int id)
        {
            return _userUnitOfWork.UserRepository.IsLockedOut(id);
        } public bool GetLockoutEnabled(int id)
        {
            return _userUnitOfWork.UserRepository.GetLockoutEnabled(id);
            //} public bool IsPasswordValidityExpired(int id)
            //{
            //   return  _userUnitOfWork.UserRepository.IsPasswordValidityExpired(id);
            //} public bool IsFirstLogin(int id)
            //{
            //   return  _userUnitOfWork.UserRepository.IsFirstLogin(id);
            //} public void AccessFailed(int id)
            //{
            //_userUnitOfWork.UserRepository.AccessFailed(id);
            //}
            //public int GetMaxAccesFailedAttempt(int id)
            //{
            //  return  _userUnitOfWork.UserRepository.GetMaxAccesFailedAttempt(id);
            //}
            //public void ResetAccesFailedCount(int Id)
            //{
            //    var user = _userUnitOfWork.UserRepository.GetById(Id);
            //    user.AccessFailedCount = 0;
            //    _userUnitOfWork.Save();
            //}
            //public void SaveNewPassword(string userName, string hashNewPassword)
            //{
            //    var user = _userUnitOfWork.UserRepository.GetUserByUsername(userName);
            //    user.LastPassword = user.Password;
            //    user.Password = hashNewPassword;
            //    user.LastPassChangeDate = DateTime.Now;
            //    user.PasswordChangedCount += 1;
            //    _userUnitOfWork.Save();
            //}
            //logins record

            //public bool IsYourLoginStillTrue(string userId, string sid)
            //{
            //    return _userUnitOfWork.LoginRecordRepository.IsYourLoginStillTrue(userId, sid);
            //}
            //public bool IsUserLoggedOnElsewhere(string userId, string sid)
            //{
            //    return _userUnitOfWork.LoginRecordRepository.IsUserLoggedOnElsewhere(userId, sid);
            //}
            //public void LogEveryoneElseOut(string userId, string sid)
            //{
            //    _userUnitOfWork.LoginRecordRepository.LogEveryoneElseOut(userId, sid);
            //}

            //public void ResetPassword(int id, string encryptPassword)
            //{
            //    var userEntry = GetUserById(id);
            //    if (userEntry != null)
            //    {
            //        userEntry.Password = encryptPassword;
            //        userEntry.PasswordChangedCount = 0;
            //        _userUnitOfWork.UserRepository.Update(userEntry);
            //        _userUnitOfWork.Save();

            //    }
            //}


        }

        public void Dispose()
        {
            _userRoleUnitOfWork.Dispose();
        }
    }
}