using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Repo
{
    public class UserUnitOfWork : IDisposable
    {
        private SCHMDbContext _Context { get; set; }
        private UserRepository _UserRepository { get; set; }
       // private LoginRecordRepository _loginRecordRepository { get; set; }


        public UserUnitOfWork(SCHMDbContext context)
        {
            _Context = context;
            _UserRepository = new UserRepository(_Context);
            //_loginRecordRepository = new LoginRecordRepository(_Context);
        }

        public UserRepository UserRepository
        {
            get
            {
                return _UserRepository;
            }
        }
        //public LoginRecordRepository LoginRecordRepository
        //{
        //    get
        //    {
        //        return _loginRecordRepository;
        //    }
        //}

        public void Save(string loggedInUserId)
        {
            _Context.SaveChanges();
        }
        public void Save()
        {
            _Context.SaveChanges();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
