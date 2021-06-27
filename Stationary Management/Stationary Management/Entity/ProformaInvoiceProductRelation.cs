using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SCHM.Entities;

namespace Stationary_Management.Entity
{
    [Table("ProformaInvoiceProductRelation")]
    public class ProformaInvoiceProductRelation : AuditableEntity //Entity
    {

        public int PfiId { get; set; }
        [ForeignKey("PfiId")]
        public virtual ProformaInvoice ProformaInvoice { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products Product { get; set; }

        //public int? AgentId { get; set; }
        //[ForeignKey("AgentId")]
        //public virtual GeneralConfig Agent { get; set; }


        [Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }

        [Display(Name = "Quantity")]
        public double Quantity { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }

        [Display(Name = "Shipment Mode")]
        public string ShipmentMode { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        //public virtual ICollection<CommercialInvoiceProduct> CommercialInvoiceProductCollection { get; set; }
    }
}