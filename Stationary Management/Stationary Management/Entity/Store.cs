using SCHM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stationary_Management.Entity
{
    public class Store :AuditableEntity
    {
        [Display(Name = "Product")]
        public int? ProductsId { get; set; }
        [ForeignKey("ProductsId")]
        public virtual Products Products  { get; set; }

        [Display(Name = "Quantity")]
        public double Quantity { get; set; }
    }
}