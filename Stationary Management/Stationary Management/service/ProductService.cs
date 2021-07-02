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
    public class ProductService
    {
        private SCHMDbContext _context;
        private ProductUnitOfWork _ProductUnitOfWork;

        public ProductService()
        {
            _context = new SCHMDbContext();
            _ProductUnitOfWork = new ProductUnitOfWork(_context);
        }

        public IEnumerable<Products> GetAllProducts()
        {
            var departments = _ProductUnitOfWork.ProductRepository.GetAll();
            return departments.OrderBy(x => x.ProductCode);
        }

        public Products GetProductById(int id)
        {
            return _ProductUnitOfWork.ProductRepository.GetById(id);
        }

        public int AddPro(Products products)
        {
            var newProducts = new Products
            {
                ProductName = products.ProductName,
                ProductCode = products.ProductCode,
                Details = products.Details,
                StockAmount = products.StockAmount,
                UnitPriceUsd = products.UnitPriceUsd,
               
                CreatedBy = products.CreatedBy
            };

            _ProductUnitOfWork.ProductRepository.Add(newProducts);
            _ProductUnitOfWork.Save();
            return newProducts.Id;
        }
        public void EditProducts(Products products)
        {
            var Productsntry = GetProductById(products.Id);
            Productsntry.ProductCode = products.ProductCode;
            Productsntry.ProductName = products.ProductName;
            Productsntry.Details = products.Details;
            Productsntry.StockAmount = products.StockAmount;
            Productsntry.UnitPriceUsd = products.UnitPriceUsd;

            Productsntry.UpdatedAt = products.UpdatedAt;
            Productsntry.UpdatedBy = products.UpdatedBy;
            _ProductUnitOfWork.ProductRepository.Update(Productsntry);
            _ProductUnitOfWork.Save();
        }

        public void DeletePro(int id, string currUserId)
        {
            _ProductUnitOfWork.ProductRepository.Disable(id);
            _ProductUnitOfWork.Save(currUserId);
        }
       

        public void Dispose()
        {
            _ProductUnitOfWork.Dispose();
        }
    }
}
