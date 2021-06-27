using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stationary_Management.Entity
{
    [Table("Customers")]
    public class Customer : AuditableEntity
    {
        //Auto generate
        [Display(Name = "Customer ID/Code")]
        public string CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Address")]
        public string HOAddress { get; set; }
        [Display(Name = "Factory Address")]
        public string FactoryAddress { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        //[Display(Name = "Country")]
        //public int? CountryId { get; set; }
        //[ForeignKey("CountryId")]
        //public virtual GeneralConfig Country { get; set; }
        [Display(Name = "Irc No")]
        public string IrcNo { get; set; }
        [Display(Name = "Tin No")]
        public string TinNo { get; set; }
        [Display(Name = "Bin No")]
        public string BinNo { get; set; }
        [Display(Name = "Bond License No")]
        public string ImportLicenseNo { get; set; }
        [Display(Name = "Bond License Date Of Issue")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ImportLicenseDateOfIssue { get; set; }
        [Display(Name = "Bond License Date Of Expiry")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ImportLicenseDateOfExpiry { get; set; }
        [Display(Name = "Import Reg. No")]
        public string ImportRegistrationNo { get; set; }
        [Display(Name = "Bangladesh Bank Reg. No")]
        public string BangladeshBankRegistrationNo { get; set; }

        [Display(Name = "AcM")]
        public int? AcmId { get; set; }
        [ForeignKey("AcmId")]
        public virtual User AcmUser { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank AC Number")]
        public string BankAcNumber { get; set; }
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        [Display(Name = "Is Seller")]
        public bool IsSeller { get; set; }
        [Display(Name = "Is Buyer")]
        public bool IsBuyer { get; set; }
        [Display(Name = "Fax")]
        public string FaxNo { get; set; }
        [Display(Name = "Parent Group")]
        public int? ParentGroupId { get; set; }
        [ForeignKey("ParentGroupId")]
        public virtual Customer ParentGroup { get; set; }
        [Display(Name = "Ext")]
        public string Ext1 { get; set; }
        // public ICollection<TermsAndConditions> TermsAndConditions { get; set; }

        //public virtual ICollection<LoanEntry> LoanEntries { get; set; }
    }
}