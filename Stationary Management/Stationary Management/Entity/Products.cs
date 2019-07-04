using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Entity
{
    public class Products
    {
        [Display(Name = "Item Code")]
        public string ProductCode { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Hs Code")]
        public string HsCode { get; set; }

        //[AllowHtml]
        [Display(Name = "Description")]
        public string Details { get; set; }

        [Display(Name = "Stock Amount")]
        public double StockAmount { get; set; }
    }
}