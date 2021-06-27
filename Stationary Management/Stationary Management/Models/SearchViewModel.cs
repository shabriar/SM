
using PagedList;
using SCHM.Web.Models;
using Stationary_Management.Entity;
using Stationary_Management.service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Models
{
    public class ProformaInvoiceSearchModel
    {
        [Display(Name = "Date Range")]
        public String SDateFrom { get; set; }
        public String SDateTo { get; set; }

        [Display(Name = "PFI Number")]
        public string PfiNumber { get; set; }

        [Display(Name = "AcM")]
        public int? SAcmId { get; set; }

        [Display(Name = "Buyer")]
        public int? BuyerId { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }


        public String Sort { get; set; }
        public String SortDir { get; set; }
        public Int32 TotalRecords { get; set; }

        public List<Customer> BuyerList { get; set; }

        public IEnumerable<SelectListItem> AcMSelectList { get; set; }

        public IPagedList<ProformaInvoice> ProformaInvoicePagedList;
        public ProformaInvoiceSearchModel()
        {
            Page = 1;
            PageSize = 50;
            BuyerList = new CustomerService().GetAllSeller().ToList();
            // SellerList = new CustomerModel().GetAllSeller().ToList();
            AcMSelectList = new SelectList(new UserModel().GetAllUser().ToList(), "Id", "ShortName");
            Sort = "CreatedAt";
            SortDir = "DESC";
        }
    }
}