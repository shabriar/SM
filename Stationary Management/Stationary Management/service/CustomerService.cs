using SCHM.Repo;
using Stationary_Management.Entity;
using Stationary_Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.service
{
    public class CustomerService
    {
        private SCHMDbContext _context;
        private CustomerUnitOfWork _customerUnitOfWork;
        public CustomerService()
        {
            _context = new SCHMDbContext();
            _customerUnitOfWork = new CustomerUnitOfWork(_context);
        }
        //public IEnumerable<Customer> GetAllCustomersList()
        //{
        //    return _customerUnitOfWork.CustomerRepository.GetAll();
        //}

        public IEnumerable<Customer> GetAllCustomer(string sCustomerID, string sName, int? sAcm)
        {
            return _customerUnitOfWork.CustomerRepository.GetAll(x => (sCustomerID == null || x.CustomerId.Contains(sCustomerID)) &&
        (sName == null || x.Name.Contains(sName)) && (sAcm == null || x.AcmId == sAcm) && x.IsBuyer, x => x.OrderBy(y => y.CustomerId));
        }

        public IEnumerable<Customer> GetAllSeller()
        {
            return _customerUnitOfWork.CustomerRepository.GetAll(x => x.IsSeller);

        }

        public IEnumerable<Customer> GetAllBuyer()
        {
            return _customerUnitOfWork.CustomerRepository.GetAll(x => x.IsBuyer);
        }
        //public IEnumerable<User> GetAllAcM()
        //{
        //    return _customerUnitOfWork.CustomerRepository.GetAllAcM();
        //}
        public int AddCustomer(Customer customer)
        {
            var newCustomer = new Customer()
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                HOAddress = customer.HOAddress,
                FactoryAddress = customer.FactoryAddress,
                Phone = customer.Phone,
                Email = customer.Email,
                IrcNo = customer.IrcNo,
                TinNo = customer.TinNo,
                BinNo = customer.BinNo,
                ImportLicenseNo = customer.ImportLicenseNo,
                ImportLicenseDateOfIssue = customer.ImportLicenseDateOfIssue,
                ImportLicenseDateOfExpiry = customer.ImportLicenseDateOfExpiry,
                ImportRegistrationNo = customer.ImportRegistrationNo,
                BangladeshBankRegistrationNo = customer.BangladeshBankRegistrationNo,
                AcmId = customer.AcmId,

                BankName = customer.BankName,
                BankAcNumber = customer.BankAcNumber,
                Branch = customer.Branch,
                SwiftCode = customer.SwiftCode,
                FaxNo = customer.FaxNo,
                IsBuyer = customer.IsBuyer,
                IsSeller = customer.IsSeller,
                City = customer.City,
                PostalCode = customer.PostalCode,
                //CountryId = customer.CountryId,
                ParentGroupId = customer.ParentGroupId,

                Status = customer.Status,
                IsDeleted = customer.IsDeleted,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy
            };
            _customerUnitOfWork.CustomerRepository.Add(newCustomer);
            _customerUnitOfWork.Save(customer.CreatedBy.ToString());
            return newCustomer.Id;
        }

        public Customer GetCustomerById(int id)
        {
            return _customerUnitOfWork.CustomerRepository.GetById(id);
        }

        public bool IsCustomerNameExist(string Name, string InitialName)
        {
            return _customerUnitOfWork.CustomerRepository.IsCustomerNameExist(Name, InitialName);
        }

        public int EditCustomer(Customer customer)
        {
            var customerEntry = _customerUnitOfWork.CustomerRepository.GetById(customer.Id);
            if (customerEntry != null)
            {
                customerEntry.CustomerId = customer.CustomerId;
                customerEntry.Name = customer.Name;
                customerEntry.FactoryAddress = customer.FactoryAddress;
                customerEntry.HOAddress = customer.HOAddress;
                customerEntry.Phone = customer.Phone;
                customerEntry.Email = customer.Email;
                customerEntry.IrcNo = customer.IrcNo;
                customerEntry.TinNo = customer.TinNo;
                customerEntry.BinNo = customer.BinNo;
                customerEntry.ImportLicenseNo = customer.ImportLicenseNo;
                customerEntry.ImportLicenseDateOfIssue = customer.ImportLicenseDateOfIssue;
                customerEntry.ImportLicenseDateOfExpiry = customer.ImportLicenseDateOfExpiry;
                customerEntry.ImportRegistrationNo = customer.ImportRegistrationNo;
                customerEntry.BangladeshBankRegistrationNo = customer.BangladeshBankRegistrationNo;
                customerEntry.AcmId = customer.AcmId;
                customerEntry.BankName = customer.BankName;
                customerEntry.BankAcNumber = customer.BankAcNumber;
                customerEntry.Branch = customer.Branch;
                customerEntry.SwiftCode = customer.SwiftCode;
                customerEntry.FaxNo = customer.FaxNo;
                customerEntry.IsBuyer = customer.IsBuyer;
                customerEntry.IsSeller = customer.IsSeller;
                customerEntry.ParentGroupId = customer.ParentGroupId;

                customerEntry.UpdatedAt = customer.UpdatedAt;
                customerEntry.UpdatedBy = customer.UpdatedBy;
                _customerUnitOfWork.CustomerRepository.Update(customerEntry);
                _customerUnitOfWork.Save(customerEntry.UpdatedBy.ToString());
            }
            return customer.Id;
        }



        public void DeleteCustomer(int id, string currentUserId)
        {
            _customerUnitOfWork.CustomerRepository.Delete(id);
            _customerUnitOfWork.Save(currentUserId);
        }
        public string AutoGenerateCustomerId()
        {
            return DateTime.Now.ToString("yyyyMM") + (_customerUnitOfWork.CustomerRepository.GetCountByMonth(DateTime.Now.Year, DateTime.Now.Month) + 1).ToString("000");
        }

        public int GetCustomersCount()
        {
            return _customerUnitOfWork.CustomerRepository.GetCount(x => x.IsBuyer == true);
        }
        public bool CheckCustomerCodeNotExist(string CustomerName)
        {
            return _customerUnitOfWork.CustomerRepository.CheckCustomerCodeNotExist(CustomerName);
        }

        #region Api
        public IEnumerable<Customer> GetAllBuyerByUserId(int id)
        {
            return _customerUnitOfWork.CustomerRepository.GetAllBuyerByUserId(id);
        }
        #endregion

    }
}