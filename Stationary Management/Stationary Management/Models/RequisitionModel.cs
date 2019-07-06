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
    public class RequisitionModel : Requisition
    {
        private RequisitionService _requisitionService;
        private ProductService _productService;
        public IEnumerable<Products> ProductsList { get; set; }


        public RequisitionModel()
        {
            _requisitionService = new RequisitionService();
            _productService = new ProductService();
            ProductsList = _productService.GetAllProducts();
        }

        public RequisitionModel(int? id) : this()
        {

            if (id != null)
            {
                var RequisitionEntry = _requisitionService.GetRequisitionById(id.Value);
                Id = RequisitionEntry.Id;
                Products = RequisitionEntry.Products;
                ProductsId = RequisitionEntry.ProductsId;
                RequisitionQuantity = RequisitionEntry.RequisitionQuantity;
                RequisitionNo = RequisitionEntry.RequisitionNo;
                Status = RequisitionEntry.Status;

                CreatedBy = RequisitionEntry.CreatedBy;
                CreatedByUser = RequisitionEntry.CreatedByUser;
                UpdatedAt = RequisitionEntry.UpdatedAt;
                UpdatedBy = RequisitionEntry.UpdatedBy;
                UpdatedByUser = RequisitionEntry.UpdatedByUser;
                Status = RequisitionEntry.Status;
            }
        }
        public IEnumerable<Requisition> GetAllRequisitions()
        {
            return _requisitionService.GetAllRequisition();
        }
        public void Add()
        {
            base.Status = 1;
            base.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _requisitionService.AddStore(this);
        }
        public void Edit()
        {
            base.UpdatedAt = DateTime.Now;
            base.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            _requisitionService.EditRequisition(this);
        }
        public void Delete(int id)
        {
            _requisitionService.DeleteRequisition(id, AuthenticatedUser.GetUserFromIdentity().UserId.ToString());
        }

       
        public void Dispose()
        {
            _requisitionService.Dispose();
        }
    }
}