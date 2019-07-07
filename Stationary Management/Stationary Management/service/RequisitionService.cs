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
                ProductsId = requisition.ProductsId,
                RequisitionQuantity = requisition.RequisitionQuantity,
                RequisitionNo = requisition.RequisitionNo,
                ReqStatus = requisition.ReqStatus,
                
               
                CreatedBy = requisition.CreatedBy
            };
            _RequisitionUnitOfWork.RequisitionRepository.Add(newRequisition);
            _RequisitionUnitOfWork.Save();
            return newRequisition.Id;
        }
        public void EditRequisition(Requisition requisition)
        {
            var RequisitionsEntry = GetRequisitionById(requisition.Id);
            RequisitionsEntry.ProductsId = requisition.ProductsId;           
            RequisitionsEntry.ReqStatus = requisition.ReqStatus;
            var ProductStockIn = _ProductUnitOfWork.ProductRepository.GetById(requisition.ProductsId);
            ProductStockIn.StockAmount += RequisitionsEntry.RequisitionQuantity;
            ProductStockIn.StockAmount -= requisition.RequisitionQuantity;
            _ProductUnitOfWork.ProductRepository.Update(ProductStockIn);
            _ProductUnitOfWork.Save();
            RequisitionsEntry.RequisitionQuantity = requisition.RequisitionQuantity;

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
        public void Approve(int id)
        {
            var RequisitionsEntry = GetRequisitionById(id);
            if (RequisitionsEntry != null)
            {
                RequisitionsEntry.ReqStatus = true;
                _RequisitionUnitOfWork.RequisitionRepository.Update(RequisitionsEntry);
                _RequisitionUnitOfWork.Save();

                var ProductStockIn = _ProductUnitOfWork.ProductRepository.GetById(RequisitionsEntry.ProductsId);
                ProductStockIn.StockAmount -= RequisitionsEntry.RequisitionQuantity;
                _ProductUnitOfWork.ProductRepository.Update(ProductStockIn);
                _ProductUnitOfWork.Save();
            }
        }
    }
}
