using SCHM.Repo;
using Stationary_Management.Common;
using Stationary_Management.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Repository
{
    public class ProformaInvoiceRepository : Repository<ProformaInvoice>
    {
        public SCHMDbContext _context;

        public ProformaInvoiceRepository(SCHMDbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCountByYear(int year)
        {
            return _context.ProformaInvoices.Count(x => x.PfiDate.Year == year && !x.ParentPfiId.HasValue);
        }


        public int GetRevisedVirsionByPFI(int id)
        {
            return _context.ProformaInvoices.Count(s => s.ParentPfiId == id);
        }

        public List<ProformaInvoice> GetAllRivision(int id)
        {
            return _context.ProformaInvoices.Where(s => s.ParentPfiId == id).OrderByDescending(o => o.Id).ToList();
        }
        public bool HasChild(int id)
        {
            return _context.ProformaInvoices.Any(s => s.ParentPfiId == id);
        }

        public ProformaInvoice GetPreviousRevision(int id)
        {
            var pfi = _context.ProformaInvoices.Find(id);
            if (pfi.RevisedVersion > 1)
            {
                return _context.ProformaInvoices.FirstOrDefault(s => s.ParentPfiId == pfi.ParentPfiId && s.RevisedVersion == pfi.RevisedVersion - 1);
            }
            else
            {
                return pfi.ParentPfi;
            }
        }

        public int GetParentPFICount()
        {
            return _context.ProformaInvoices.Count(x => !x.IsDeleted && !x.ParentPfiId.HasValue);
        }

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableProducts(int pfiId)
        //{
        //    var proformaInvoiceProduct = new List<ProformaInvoiceProductRelation>();
        //    var ciProducts = new List<CommercialInvoiceProduct>();

        //    var pfi = _context.ProformaInvoices.Find(pfiId);
        //    var pfiProducts = pfi.ProductsRelationICollection;

        //    var lc = pfi.LcPfiCollection.FirstOrDefault().Lc;
        //    var comInvoices = lc.CommercialInvoiceCollection;

        //    foreach (var ci in comInvoices)
        //    {
        //        ciProducts.AddRange(ci.CommercialInvoiceProductCollection);
        //    }
        //    foreach (var item in pfiProducts)
        //    {
        //        var ciProductQty = ciProducts.Where(x => x.PfiId == pfiId && x.ProductId == item.ProductId).Sum(x => x.Quantity);
        //        if (ciProductQty > 0)
        //        {
        //            if (ciProductQty < item.Quantity)
        //            {
        //                //item.Quantity = item.Quantity - ciProductQty;
        //                proformaInvoiceProduct.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            proformaInvoiceProduct.Add(item);
        //        }
        //    }
        //    return proformaInvoiceProduct;
        //}
        //public IEnumerable<LcPendingOrderDataModel> GetComInvoiceablePendingOrderData(int pfiId)
        //{
        //    var lcPendingOrderDataModelList = new List<LcPendingOrderDataModel>();
        //    var ciProducts = new List<CommercialInvoiceProduct>();

        //    var pfi = _context.ProformaInvoices.Find(pfiId);
        //    var pfiProducts = pfi.ProductsRelationICollection;

        //    var lc = pfi.LcPfiCollection.FirstOrDefault().Lc;
        //    var comInvoices = lc.CommercialInvoiceCollection;

        //    foreach (var ci in comInvoices)
        //    {
        //        ciProducts.AddRange(ci.CommercialInvoiceProductCollection);
        //    }
        //    foreach (var item in pfiProducts)
        //    {
        //        var ciProductQty = ciProducts.Where(x => x.PfiId == pfiId && x.ProductId == item.ProductId && x.ShipmentMode == item.ShipmentMode).Sum(x => x.Quantity);
        //        if (ciProductQty > 0)
        //        {
        //            if (ciProductQty < item.Quantity)
        //            {
        //                var modelItem = new LcPendingOrderDataModel()
        //                {
        //                    ProductId = item.ProductId,
        //                    ProductName = item.Product.ProductName,
        //                    Quantity = item.Quantity,
        //                    UnitPrice = item.UnitPrice
        //                };
        //                modelItem.TotalPendingQty = item.ProformaInvoice.CommercialInvoiceProductCollection.Where(x => x.PfiId == item.PfiId && x.ProductId == modelItem.ProductId).Sum(y => y.Quantity);
        //                lcPendingOrderDataModelList.Add(modelItem);
        //            }
        //        }
        //        else
        //        {
        //            var modelItem = new LcPendingOrderDataModel()
        //            {
        //                ProductId = item.ProductId,
        //                ProductName = item.Product.ProductName,
        //                Quantity = item.Quantity,
        //                UnitPrice = item.UnitPrice
        //            };
        //            modelItem.TotalPendingQty = item.ProformaInvoice.CommercialInvoiceProductCollection.Where(x => x.PfiId == item.PfiId && x.ProductId == modelItem.ProductId).Sum(y => y.Quantity);
        //            lcPendingOrderDataModelList.Add(modelItem);
        //        }
        //    }
        //    return lcPendingOrderDataModelList;
        //}

        //public double GetPendingPfiAmountForLc()
        //{
        //    var pendingPfi = _context.ProformaInvoices.Where(x => !x.IsDeleted && x.IsClosed == false && x.Status != 0 && !x.ParentPfiId.HasValue && !x.LcPfiCollection.Any()).Sum(x => x.PfiValue) ?? 0;
        //    return pendingPfi;
        //}

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableEditProducts(int pfiId, int ciId)
        //{
        //    var proformaInvoiceProduct = new List<ProformaInvoiceProductRelation>();
        //    var ciProducts = new List<CommercialInvoiceProduct>();

        //    var pfi = _context.ProformaInvoices.Find(pfiId);
        //    var pfiProducts = pfi.ProductsRelationICollection;

        //    var lc = pfi.LcPfiCollection.FirstOrDefault().Lc;
        //    var comInvoices = lc.CommercialInvoiceCollection.Where(x => x.Id != ciId);

        //    foreach (var ci in comInvoices)
        //    {
        //        ciProducts.AddRange(ci.CommercialInvoiceProductCollection);
        //    }
        //    foreach (var item in pfiProducts)
        //    {
        //        var ciProductQty = ciProducts.Where(x => x.PfiId == pfiId && x.ProductId == item.ProductId).Sum(x => x.Quantity);
        //        if (ciProductQty > 0)
        //        {
        //            if (ciProductQty < item.Quantity)
        //            {
        //                //item.Quantity = item.Quantity - ciProductQty;
        //                proformaInvoiceProduct.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            proformaInvoiceProduct.Add(item);
        //        }
        //    }
        //    return proformaInvoiceProduct;

        //}

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoicedProducts(int pfiId, int ciId)
        //{
        //    var proformaInvoiceProduct = new List<ProformaInvoiceProductRelation>();
        //    var ciProducts = new List<CommercialInvoiceProduct>();

        //    var pfi = _context.ProformaInvoices.Find(pfiId);
        //    var pfiProducts = pfi.ProductsRelationICollection;

        //    var lc = pfi.LcPfiCollection.FirstOrDefault().Lc;
        //    var comInvoices = lc.CommercialInvoiceCollection.Where(x => x.Id == ciId);

        //    foreach (var ci in comInvoices)
        //    {
        //        ciProducts.AddRange(ci.CommercialInvoiceProductCollection);
        //    }
        //    foreach (var item in pfiProducts)
        //    {
        //        var ciProductQty = ciProducts.Where(x => x.PfiId == pfiId && x.ProductId == item.ProductId).Sum(x => x.Quantity);
        //        if (ciProductQty > 0)
        //        {
        //            proformaInvoiceProduct.Add(item);
        //        }
        //    }
        //    return proformaInvoiceProduct;

        //    //var product = _context.ProformaInvoiceProductRelation.Where(x => x.PfiId == pfiId && x.CommercialInvoiceProductCollection.Any(y => y.InvoiceId == ciId)).ToList();
        //    //return product;
        //}

        public double? GetTotalAmount(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);
            var list = _context.ProformaInvoices.Where(x => x.ParentPfiId == null &&
               (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
               (buyerId == null || x.BuyerId == buyerId) &&
                (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
               (sDateTo == null || x.PfiDate < toDate));
            return list.Any() ? list.Sum(s => s.PfiValue.Value) : 0;
        }

        public double? GetTotalQty(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);
            var list = _context.ProformaInvoices.Where(x => x.ParentPfiId == null &&
             (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
             (buyerId == null || x.BuyerId == buyerId) &&
              (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
             (sDateTo == null || x.PfiDate < toDate));
            return list.Any() ? list.Sum(s => s.ProductsRelationICollection.Sum(p => p.Quantity)) : 0;
        }

        public double? GetPoWithOutLcTotalPfiValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var list = _context.ProformaInvoices.Where(x => x.ParentPfiId == null && x.Status != (byte)EnumActiveDactiveStatus.Inactive &&
          (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
          (buyerId == null || x.BuyerId == buyerId) &&
           (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
          (sDateTo == null || x.PfiDate < toDate));
            return list.Any() ? list.Sum(s => s.PfiValue) : 0;
        }

        public double? GetCloseCancelPfiValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var list = _context.ProformaInvoices.Where(x => x.ParentPfiId == null && (x.Status == (byte)EnumActiveDactiveStatus.Inactive || x.IsClosed == true) &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
              (sDateTo == null || x.PfiDate < toDate));
            return list.Any() ? list.Sum(s => s.PfiValue) : 0;
        }

        public ProformaInvoice GetBuyerRelativeDataById(int buyerId)
        {
            var buyerRelativeData = _context.ProformaInvoices.Where(x => x.BuyerId == buyerId).OrderByDescending(o => o.Id).FirstOrDefault();
            return buyerRelativeData;
        }

        // chart //
        //public IEnumerable<TopPfiProductChartData> GetTopPfiForPieChart(int lastDaysCount, int topCount)
        //{
        //    DateTime lastDate = DateTime.Now.AddDays(-lastDaysCount + 1);
        //    var searchData = _context.ProformaInvoiceProductRelation.Where(x => x.ProformaInvoice.CreatedAt > lastDate);
        //    var groupList = searchData.GroupBy(g => g.ProductId).OrderByDescending(s => s.Sum(f => f.Quantity)).Take(topCount);

        //    return groupList.Select(s => new TopPfiProductChartData()
        //    {
        //        ProductCode = s.FirstOrDefault().Product.ProductCode,
        //        ProductName = s.FirstOrDefault().Product.ProductName,
        //        Quantity = s.Sum(f => f.Quantity)
        //    });
        //}
    }
}