using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stationary_Management.Entity
{
    [Table("ProformaInvoices")]
    public class ProformaInvoice : AuditableEntity
    {
        [Display(Name = "PFI No")]
        public string PfiNumber { get; set; }

        [Display(Name = "PFI Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PfiDate { get; set; }
        [Display(Name = "Buyer")]
        public int? BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual Customer Buyer { get; set; }
        [Display(Name = "Seller")]
        public int? SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Customer Seller { get; set; }
        [Display(Name = "PFI Value")]
        public double? PfiValue { get; set; }

        [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }

        [Display(Name = "Sea Freight Charge/Unit")]
        public double? SeaFreightChargePerUnit { get; set; }
        [Display(Name = "Air Freight Charge/Unit")]
        public double? AirFreightChargePerUnit { get; set; }

        //[Display(Name = "Payment Terms")]
        //public int? PaymentTermsId { get; set; }
        //[ForeignKey("PaymentTermsId")]
        //public virtual PaymentTerms PaymentTerms { get; set; }

        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Display(Name = "Incoterms")]
        public string IncoTerms { get; set; }

        [Display(Name = "Category")]
        public string PfiCategory { get; set; }

        [Display(Name = "Indent Validity")]
        public string IndentValidity { get; set; }

        [Display(Name = "Parent PFI")]
        public int? ParentPfiId { get; set; }
        [ForeignKey("ParentPfiId")]
        public virtual ProformaInvoice ParentPfi { get; set; }

        [Display(Name = "Revised Version")]
        public int? RevisedVersion { get; set; }
        [Display(Name = "Discount Type")]
        public string DiscountType { get; set; }
        [Display(Name = "Discount Value")]
        public double? DiscountValue { get; set; }
        [Display(Name = "PO No")]
        public string PoNo { get; set; }
        //[Display(Name = "Currency")]
        //public int? CurrencyId { get; set; }
        //[ForeignKey("CurrencyId")]
        //public virtual Currency Currency { get; set; }
        [Display(Name = "Is Closed")]
        public bool? IsClosed { get; set; }
        [Display(Name = "Revised Text")]
        public string RevisedText { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "ForthComing LC Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? ForthComingLcDate { get; set; }

        [Display(Name = "Country Of Origin")]
        public string CountryOfOrigins { get; set; }

        [Display(Name = "Ext")]
        public string Ext1 { get; set; }
        public virtual ICollection<ProformaInvoiceProductRelation> ProductsRelationICollection { get; set; }
        //public virtual ICollection<PfiPortOfShipment> PfiPortOfShipmentCollection { get; set; }
        //public virtual ICollection<LcPfi> LcPfiCollection { get; set; }
        //public virtual ICollection<CommercialInvoiceProduct> CommercialInvoiceProductCollection { get; set; }
    }
}