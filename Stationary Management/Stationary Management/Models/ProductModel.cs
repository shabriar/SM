using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCHM.Entities;
using SCHM.Services;
using Stationary_Management.Entity;

namespace SCHM.Web.Models
{
    [NotMapped]
    public class ProductModel : Products
    {
        private ProductService _ProductService;

        

        public ProductModel()
        {
            _ProductService = new ProductService();
        }

        public ProductModel(int? id) : this()
        {

            if (id != null)
            {
                var productEntry = _ProductService.GetProductById(id.Value);
                Id = productEntry.Id;
                ProductCode = productEntry.ProductCode;
                ProductName = productEntry.ProductName;
                Details = productEntry.Details;
                StockAmount = productEntry.StockAmount;
                UnitPriceUsd = productEntry.UnitPriceUsd;

                CreatedBy = productEntry.CreatedBy;
                CreatedByUser = productEntry.CreatedByUser;
                UpdatedAt = productEntry.UpdatedAt;
                UpdatedBy = productEntry.UpdatedBy;
                UpdatedByUser = productEntry.UpdatedByUser;
                Status = productEntry.Status;
            }
        }
        public IEnumerable<Products> GetAllProductss()
        {
            return _ProductService.GetAllProducts();
        }
        public void Add()
        {
            base.Status = 1;
            base.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _ProductService.AddPro(this);
        }
        public void Edit()
        {
            base.UpdatedAt = DateTime.Now;
            base.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _ProductService.EditProducts(this);
        }
        public void Delete(int id)
        {
            _ProductService.DeletePro(id, AuthenticatedUser.GetUserFromIdentity().UserId.ToString());
        }

       
        public void Dispose()
        {
            _ProductService.Dispose();
        }
    }
}