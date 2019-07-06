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
    public class StoreService
    {
        private SCHMDbContext _context;
        private StoreUnitOfWork _storeUnitOfWork;
        private ProductUnitOfWork _ProductUnitOfWork;

        public StoreService()
        {
            _context = new SCHMDbContext();
            _storeUnitOfWork = new StoreUnitOfWork(_context);
            _ProductUnitOfWork = new ProductUnitOfWork(_context);
        }

        public IEnumerable<Store> GetAllStore()
        {
            var departments = _storeUnitOfWork.StoreRepository.GetAll();
            return departments.OrderBy(x => x.ProductsId);
        }

        public Store GetStoreById(int id)
        {
            return _storeUnitOfWork.StoreRepository.GetById(id);
        }

        public int AddStore(Store stores)
        {
            var newStore = new Store
            {
                Products = stores.Products,
                ProductsId = stores.ProductsId,
                Quantity = stores.Quantity,
                
               
                CreatedBy = stores.CreatedBy
            };
            var ProductStockIn = _ProductUnitOfWork.ProductRepository.GetById(stores.ProductsId);
            ProductStockIn.StockAmount += stores.Quantity;
            _ProductUnitOfWork.ProductRepository.Update(ProductStockIn);
            _ProductUnitOfWork.Save();

            _storeUnitOfWork.StoreRepository.Add(newStore);
            _storeUnitOfWork.Save();
            return newStore.Id;
        }
        public void EditStore(Store stores)
        {
            var Storesntry = GetStoreById(stores.Id);
            Storesntry.Products = stores.Products;
            Storesntry.ProductsId = stores.ProductsId;
            Storesntry.Quantity = stores.Quantity;


            Storesntry.UpdatedAt = stores.UpdatedAt;
            Storesntry.UpdatedBy = stores.UpdatedBy;

            _storeUnitOfWork.StoreRepository.Update(Storesntry);
            _storeUnitOfWork.Save();
        }

        public void DeleteStore(int id, string currUserId)
        {
            _storeUnitOfWork.StoreRepository.Disable(id);
            _storeUnitOfWork.Save(currUserId);
        }
       

        public void Dispose()
        {
            _storeUnitOfWork.Dispose();
        }
    }
}
