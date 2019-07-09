using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stationary_Management.Entity
{
    public class Requisition: AuditableEntity
    {
        [Display(Name = "Product")]
        public int? ProductsId { get; set; }
        [ForeignKey("ProductsId")]
        public virtual Products Products { get; set; }

        [Display(Name = "Requisition Quantity")]
        public double RequisitionQuantity { get; set; }
        [Display(Name = "Requisition No")]
        public double RequisitionNo { get; set; }

        [Display(Name = "Requisition Status")]
        public  bool ReqStatus { get; set; }
    }
}