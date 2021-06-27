using SCHM.Repo;
using Stationary_Management.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class CustomerRepository : Repository<Customer>
    {
        private SCHMDbContext _context;
        public CustomerRepository(SCHMDbContext context) : base(context)
        {
            _context = context;
        }
        public Customer GetDefaultSeller()
        {
            return _context.Customers.FirstOrDefault(x => x.IsSeller && x.Name.ToLower() == "Huntsman (Singapore) Pte. Ltd.".ToLower());
        }
        //public IEnumerable<User> GetAllAcM()
        //{
        //    return _context.Customers.Where(x => x.IsBuyer && !x.IsDeleted && x.AcmId.HasValue).GroupBy(x => x.AcmId).Select(x => x.FirstOrDefault().AcmUser);
        //}
        public int GetCountByMonth(int year, int month)
        {
            return _context.Customers.Count(x => x.CreatedAt.Value.Year == year && x.CreatedAt.Value.Month == month);
        }
        public bool IsCustomerNameExist(string CustomerName, string InitialCustomerName)
        {
            bool isNotExist = true;
            if (CustomerName != string.Empty && InitialCustomerName == "undefined")
            {
                var isExist = _context.Customers.Any(x => !x.IsDeleted && x.Name.ToLower().Equals(CustomerName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (CustomerName != string.Empty && InitialCustomerName != "undefined")
            {
                var isExist = _context.Customers.Any(x => !x.IsDeleted && x.Name.ToLower() == CustomerName.ToLower() && x.Name.ToLower() != InitialCustomerName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }



        public bool CheckCustomerCodeNotExist(string customerName)
        {
            bool isNotExist = true;
            if (customerName != string.Empty)
            {
                var isExist = _context.Customers.Any(x => !x.IsDeleted && x.Name.ToLower().Equals(customerName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }

        #region For Api
        public IEnumerable<Customer> GetAllBuyerByUserId(int id)
        {
            var customers = _context.Customers.Where(x => x.AcmUser.Id == id && x.IsBuyer == true);
            return customers;
        }
        #endregion
    }
}