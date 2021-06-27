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
    public class StoreModel : Store
    {
        private StoreService _StoreService;        
        private ProductService _productService;
        public IEnumerable<Products> ProductsList { get; set; }

        public StoreModel()
        {
            _StoreService = new StoreService();
            _productService = new ProductService();
            ProductsList = _productService.GetAllProducts();
        }
        public StoreModel(int? id) : this()
        {

            if (id != null)
            {
                var StoreEntry = _StoreService.GetStoreById(id.Value);
                Id = StoreEntry.Id;
                Products = StoreEntry.Products;
                ProductsId = StoreEntry.ProductsId;
                Quantity = StoreEntry.Quantity;
                CreatedBy = StoreEntry.CreatedBy;
                CreatedByUser = StoreEntry.CreatedByUser;
                UpdatedAt = StoreEntry.UpdatedAt;
                UpdatedBy = StoreEntry.UpdatedBy;
                UpdatedByUser = StoreEntry.UpdatedByUser;
                Status = StoreEntry.Status;
            }
        }
        public IEnumerable<Store> GetAllStore()
        {
            return _StoreService.GetAllStore();
        }
        public void Add()
        {
            base.Status = 1;
            base.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _StoreService.AddStore(this);
        }
        public void Edit()
        {
            base.UpdatedAt = DateTime.Now;
            base.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _StoreService.EditStore(this);
        }
        public void Delete(int id)
        {
            _StoreService.DeleteStore(id, AuthenticatedUser.GetUserFromIdentity().UserId.ToString());
        }

       
        public void Dispose()
        {
            _StoreService.Dispose();
        }
    }
}