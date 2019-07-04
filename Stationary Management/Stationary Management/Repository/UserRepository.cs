using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

//using PagedList;
//using SCHM.Common;

namespace SCHM.Repo
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly SCHMDbContext _context;

        public UserRepository(SCHMDbContext context)
            : base(context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers()
        {
            var users = _context.Users.Where(x => x.Status != 2);
            return users.OrderBy(x => x.Name);
        }
        public User GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0);
            return user;

        }
        public bool CheckUsernameIsValid(string username)
        {
            return _context.Users.Any(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0);
        }
        public User GetValidUserByPassword(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0 && u.Password.Equals(password));
            return user;

        }

        public List<AuditLog> GetAllAuditLogsByUserIdDate(int currentUserId, DateTime lastDate)
        {
            return _context.AuditLogs.Where(x => x.CreatedUser.Value == currentUserId && x.UpdatedDate >= lastDate).ToList();
        }

        public bool IsUserNameExist(string UserName, string InitialUserName)
        {
            bool isNotExist = true;
            if (UserName != string.Empty && InitialUserName == "undefined")
            {
                var isExist = _context.Users.Any(x => x.Status != 0 && x.UserName.ToLower().Equals(UserName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (UserName != string.Empty && InitialUserName != "undefined")
            {
                var isExist = _context.Users.Any(x => x.Status != 0 && x.UserName.ToLower() == UserName.ToLower() && x.UserName.ToLower() != InitialUserName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }
        public bool IsEmailExist(string Email, string InitialEmail)
        {
            bool isNotExist = true;
            if (Email != string.Empty && InitialEmail == "undefined")
            {
                var isExist = _context.Users.Any(x => x.Status != 0 && x.Email.ToLower().Equals(Email.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (Email != string.Empty && InitialEmail != "undefined")
            {
                var isExist = _context.Users.Any(x => x.Status != 0 && x.Email.ToLower() == Email.ToLower() && x.Email.ToLower() != InitialEmail.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }



        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
        public bool IsLockedOut(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Status != 0);
            if (user.LockoutEndDateUtc != null && user.LockoutEndDateUtc > DateTime.Now)
            {
                return true;
            }

            return false;
        }
        public bool GetLockoutEnabled(int? id)
        {
            var lockOutEnabled = _context.Users.FirstOrDefault(u => u.Id == id && u.Status != 0).LockoutEnabled;
            if (lockOutEnabled)
            {
                return true;
            }

            return false;
        }

        //public void AccessFailed(int? id)
        //{

        //    var user = _context.Users.Find(id);
        //    int maxFailedAccessAttempts = (int)_context.PasswordPolicyConfigs.FirstOrDefault(x => x.UserType == user.UserType).LockUnathorizeAccess;
        //    // int lockoutTimeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());
        //    int lockoutTimeSpan = 15;
        //    user.AccessFailedCount = Convert.ToInt32(user.AccessFailedCount) + 1;
        //    if (user.AccessFailedCount >= maxFailedAccessAttempts)
        //    {
        //        user.LockoutEndDateUtc = DateTime.Now.AddMinutes(lockoutTimeSpan);
        //        user.AccessFailedCount = 0;
        //    }
        //    _context.SaveChanges();
        //}
        //public int GetMaxAccesFailedAttempt(int id)
        //{
        //    var user = _context.Users.Find(id);
        //    int maxFailedAccessAttempts = (int)_context.PasswordPolicyConfigs.FirstOrDefault(x => x.UserType == user.UserType).LockUnathorizeAccess;
        //    return maxFailedAccessAttempts;
        //}


        //public bool IsPasswordValidityExpired(int? id)
        //{

        //    var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Status != 0);
        //    int passwordValidityAge = 90;
        //    if (_context.PasswordPolicyConfigs.Any(x => x.UserType == user.UserType))
        //    {
        //        passwordValidityAge = Convert.ToInt32(_context.PasswordPolicyConfigs.FirstOrDefault(x => x.UserType == user.UserType).PasswordExpire);
        //    }

        //    if (DateTime.Now.Date >= Convert.ToDateTime(user.LastPassChangeDate).AddDays(passwordValidityAge - 5) && DateTime.Now.Date < Convert.ToDateTime(user.LastPassChangeDate).AddDays(passwordValidityAge))
        //    {
        //        //mailCtrl.ControllerContext = ControllerContext;
        //        //var sent = mailCtrl.PasswordChangeNotification(user.UserId);
        //    }

        //    if (DateTime.Now.Date >= Convert.ToDateTime(user.LastPassChangeDate).AddDays(passwordValidityAge))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public bool IsFirstLogin(int? userId)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        //    if (user.PasswordChangedCount > 0 || user.PasswordChangedCount == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}

        ////public IPagedList<AuditLog> GetAllAuditLogs(int page, int pageSize, string dateTo, string dateFrom, string selectedAction, string selectedModule, int? userId)
        ////{
        ////    var fromDate = string.IsNullOrEmpty(dateFrom) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(dateFrom);
        ////    var toDate = string.IsNullOrEmpty(dateTo) ? DateTime.Now.AddDays(1).Date : Convert.ToDateTime(dateTo).AddDays(1);
        ////    return _context.AuditLogs.ToList().Where(x => (fromDate == null || x.UpdatedDate >= fromDate) && (toDate == null || x.UpdatedDate <= toDate) && (userId == null || x.CreatedUser == userId) && ((selectedModule == null || selectedModule == "") || x.TableName.ToUpper() == selectedModule.ToUpper()) && ((selectedAction == null || selectedAction == "") || x.EventType.ToUpper() == selectedAction.ToUpper())).OrderByDescending(o => o.UpdatedDate).ToPagedList(page, pageSize);
        ////}

        //public IEnumerable<AuditLog> GetAllAuditLogs()
        //{
        //    return _context.AuditLogs;
        //}

        //public List<string> GetAllActions()
        //{
        //    return _context.AuditLogs.ToList().GroupBy(x => x.EventType).Select(y => y.Key).ToList();
        //}

        //public List<string> GetAllModules()
        //{
        //    return _context.AuditLogs.ToList().GroupBy(x => x.TableName).Select(y => y.Key).ToList();
        //}
    }
}
