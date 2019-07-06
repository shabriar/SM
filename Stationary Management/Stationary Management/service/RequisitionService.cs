using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCHM.Entities;
using SCHM.Repo;
using Stationary_Management.Entity;

namespace SCHM.Services
{
    public class RequisitionService
    {
        private SCHMDbContext _context;
        private StoreUnitOfWork _storeUnitOfWork;
        private ProductUnitOfWork _ProductUnitOfWork;
        private RequisitionUnitOfWork _RequisitionUnitOfWork;

        public RequisitionService()
        {
            _context = new SCHMDbContext();
            _storeUnitOfWork = new StoreUnitOfWork(_context);
            _ProductUnitOfWork = new ProductUnitOfWork(_context);
            _RequisitionUnitOfWork = new RequisitionUnitOfWork(_context);
        }

        public IEnumerable<Requisition> GetAllRequisition()
        {
            var departments = _RequisitionUnitOfWork.RequisitionRepository.GetAll();
            return departments.OrderBy(x => x.ProductsId);
        }

        public Requisition GetRequisitionById(int id)
        {
            return _RequisitionUnitOfWork.RequisitionRepository.GetById(id);
        }

        public int AddStore(Requisition requisition)
        {
            var newRequisition = new Requisition
            {
                Products = requisition.Products,
                ProductsId = requisition.ProductsId,
                RequisitionQuantity = requisition.RequisitionQuantity,
                RequisitionNo = requisition.RequisitionNo,
                Status = requisition.Status,
                
               
                CreatedBy = requisition.CreatedBy
            };
            var ProductStockIn = _ProductUnitOfWork.ProductRepository.GetById(requisition.ProductsId);
            ProductStockIn.StockAmount -= requisition.RequisitionQuantity;
            _ProductUnitOfWork.ProductRepository.Update(ProductStockIn);
            _ProductUnitOfWork.Save();

            _RequisitionUnitOfWork.RequisitionRepository.Add(newRequisition);
            _RequisitionUnitOfWork.Save();
            return newRequisition.Id;
        }
        public void EditRequisition(Requisition requisition)
        {
            var RequisitionsEntry = GetRequisitionById(requisition.Id);
            RequisitionsEntry.Products = requisition.Products;
            RequisitionsEntry.ProductsId = requisition.ProductsId;
            RequisitionsEntry.RequisitionQuantity = requisition.RequisitionQuantity;
            RequisitionsEntry.RequisitionNo = requisition.RequisitionNo;
            RequisitionsEntry.Status = requisition.Status;


            RequisitionsEntry.UpdatedAt = requisition.UpdatedAt;
            RequisitionsEntry.UpdatedBy = requisition.UpdatedBy;

            _RequisitionUnitOfWork.RequisitionRepository.Update(RequisitionsEntry);
            _RequisitionUnitOfWork.Save();
        }

        public void DeleteRequisition(int id, string currUserId)
        {
            _RequisitionUnitOfWork.RequisitionRepository.Disable(id);
            _RequisitionUnitOfWork.Save(currUserId);
        }
       

        public void Dispose()
        {
            _storeUnitOfWork.Dispose();
        }
    }
}
