using SCHM.Repo;
using Stationary_Management.Common;
using Stationary_Management.Entity;
using Stationary_Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.service
{
    public class ProformaInvoiceService
    {
        private SCHMDbContext _context;
        private ProformaInvoiceUnitOfWork _proformaInvoiceUnitOfWork;
        //private PfiPortOfShipmentUnitOfWork _pfiPortOfShipmentUnitOfWork;
        private CustomerUnitOfWork _customerUnitOfWork;
        private ProductUnitOfWork _productUnitOfWork;
        public ProformaInvoiceService()
        {
            _context = new SCHMDbContext();
            _proformaInvoiceUnitOfWork = new ProformaInvoiceUnitOfWork(_context);
            //_pfiPortOfShipmentUnitOfWork = new PfiPortOfShipmentUnitOfWork(_context);
            _productUnitOfWork = new ProductUnitOfWork(_context);
        }
        public IEnumerable<Products> GetAllProductsForPfi(int? buyerId)
        {
            var productList = new List<Products>();
            var pfiProductList = _proformaInvoiceUnitOfWork._proformaInvoiceProductRelationRepository.GetAll(x => x.ProformaInvoice.BuyerId == buyerId);

            productList.AddRange(pfiProductList.Select(y => y.Product));
            productList.AddRange(_productUnitOfWork.ProductRepository.GetAll());

            return productList.Distinct().ToList();
        }

        public int AddProformaInvoice(ProformaInvoice proformaInvoice)
        {
            var newProformaInvoice = new ProformaInvoice
            {
                PfiNumber = GetProformaInvoiceNo(proformaInvoice.PfiDate.Year),
                PfiDate = proformaInvoice.PfiDate,
                PoNo = proformaInvoice.PoNo,
                BuyerId = proformaInvoice.BuyerId,
                SellerId = GetDefaultSeller().Id,
                PfiValue = proformaInvoice.PfiValue,
                TotalQuantity = proformaInvoice.TotalQuantity,
                SeaFreightChargePerUnit = proformaInvoice.SeaFreightChargePerUnit,
                AirFreightChargePerUnit = proformaInvoice.AirFreightChargePerUnit,
                ProductType = proformaInvoice.ProductType,
                PfiCategory = proformaInvoice.PfiCategory,
                IndentValidity = proformaInvoice.IndentValidity,
                //PaymentTermsId = proformaInvoice.PaymentTermsId,
                IncoTerms = proformaInvoice.IncoTerms,
                DiscountType = proformaInvoice.DiscountType,
                DiscountValue = proformaInvoice.DiscountValue,
                Status = proformaInvoice.Status,
                //CurrencyId = proformaInvoice.CurrencyId,
                CountryOfOrigins = proformaInvoice.CountryOfOrigins,

                CreatedAt = proformaInvoice.CreatedAt,
                CreatedBy = proformaInvoice.CreatedBy,
                IsClosed = false,
                Remarks = proformaInvoice.Remarks
            };
            _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.Add(newProformaInvoice);
            _proformaInvoiceUnitOfWork.Save();
            return newProformaInvoice.Id;
        }
        //public double GetPendingPfiAmountForLc()
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetPendingPfiAmountForLc();
        //}


        public void EditProformaInvoice(ProformaInvoice proformaInvoice)
        {
            int returnId = 0;

            var proformaInvoiceEntry = GetProformaInvoiceById(proformaInvoice.Id);
            if (proformaInvoiceEntry != null)
            {

                int? revisionNumber = GetRevisedVirsionByPFI(proformaInvoice.Id);
                #region add new log for previous version of PFI
                var newProformaInvoice = new ProformaInvoice

                {
                    PfiNumber = proformaInvoiceEntry.PfiNumber,
                    PfiDate = proformaInvoiceEntry.PfiDate,
                    PoNo = proformaInvoiceEntry.PoNo,
                    BuyerId = proformaInvoiceEntry.BuyerId,
                    SellerId = GetDefaultSeller().Id,
                    PfiValue = proformaInvoiceEntry.PfiValue,
                    TotalQuantity = proformaInvoiceEntry.TotalQuantity,
                    SeaFreightChargePerUnit = proformaInvoiceEntry.SeaFreightChargePerUnit,
                    AirFreightChargePerUnit = proformaInvoiceEntry.AirFreightChargePerUnit,
                    ProductType = proformaInvoiceEntry.ProductType,
                    PfiCategory = proformaInvoiceEntry.PfiCategory,
                    IndentValidity = proformaInvoiceEntry.IndentValidity,
                    //PaymentTermsId = proformaInvoiceEntry.PaymentTermsId,
                    IncoTerms = proformaInvoiceEntry.IncoTerms,
                    Status = proformaInvoiceEntry.Status,
                    CreatedAt = proformaInvoice.UpdatedAt,
                    CreatedBy = proformaInvoice.UpdatedBy,
                    ParentPfiId = proformaInvoiceEntry.Id,
                    //CurrencyId = proformaInvoiceEntry.CurrencyId,
                    Remarks = proformaInvoiceEntry.Remarks,

                    RevisedVersion = revisionNumber,
                    DiscountType = proformaInvoiceEntry.DiscountType,

                    DiscountValue = proformaInvoice.DiscountValue,
                    RevisedText = proformaInvoiceEntry.RevisedText,
                    IsClosed = false,
                    CountryOfOrigins = proformaInvoiceEntry.CountryOfOrigins,
                };
                _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.Add(newProformaInvoice);
                _proformaInvoiceUnitOfWork.Save();
                returnId = newProformaInvoice.Id;
                foreach (var productRelation in proformaInvoiceEntry.ProductsRelationICollection.ToList())
                {
                    var newProductRelation = new ProformaInvoiceProductRelation()
                    {
                        PfiId = returnId,

                        ProductId = productRelation.ProductId,
                        //AgentId = productRelation.AgentId,
                        UnitPrice = productRelation.UnitPrice,
                        Quantity = productRelation.Quantity,
                        TotalPrice = productRelation.TotalPrice,
                        ShipmentMode = productRelation.ShipmentMode,
                        Remarks = productRelation.Remarks,
                        Status = productRelation.Status,
                        CreatedBy = productRelation.CreatedBy,
                        //CreatedDate = productRelation.CreatedDate
                    };
                    _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.Add(newProductRelation);
                    _proformaInvoiceUnitOfWork.Save();
                }
                //foreach (var portShipment in proformaInvoiceEntry.PfiPortOfShipmentCollection.ToList())
                //{
                //    var newPortShipment = new PfiPortOfShipment()
                //    {
                //        CountryId = portShipment.CountryId,
                //        PfiId = returnId
                //    };
                //    _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.Add(newPortShipment);
                //    _pfiPortOfShipmentUnitOfWork.Save();
                //}
                #endregion

                #region Update Parent PFi
                proformaInvoiceEntry.PfiNumber = proformaInvoiceEntry.PfiNumber + "R";
                proformaInvoiceEntry.PfiDate = proformaInvoice.PfiDate;
                proformaInvoiceEntry.PoNo = proformaInvoice.PoNo;
                proformaInvoiceEntry.BuyerId = proformaInvoice.BuyerId;
                proformaInvoiceEntry.PfiValue = proformaInvoice.PfiValue;
                proformaInvoiceEntry.TotalQuantity = proformaInvoice.TotalQuantity;
                proformaInvoiceEntry.SeaFreightChargePerUnit = proformaInvoice.SeaFreightChargePerUnit;
                proformaInvoiceEntry.AirFreightChargePerUnit = proformaInvoice.AirFreightChargePerUnit;
                proformaInvoiceEntry.ProductType = proformaInvoice.ProductType;
                proformaInvoiceEntry.PfiCategory = proformaInvoice.PfiCategory;
                proformaInvoiceEntry.IndentValidity = proformaInvoice.IndentValidity;
                //proformaInvoiceEntry.PaymentTermsId = proformaInvoice.PaymentTermsId;
                proformaInvoiceEntry.IncoTerms = proformaInvoice.IncoTerms;
                proformaInvoiceEntry.Status = proformaInvoice.Status;
                proformaInvoiceEntry.UpdatedAt = proformaInvoice.UpdatedAt;
                proformaInvoiceEntry.UpdatedBy = proformaInvoice.UpdatedBy;
                proformaInvoiceEntry.DiscountType = proformaInvoice.DiscountType;
                proformaInvoiceEntry.DiscountValue = proformaInvoice.DiscountValue;
                proformaInvoiceEntry.RevisedText = proformaInvoice.RevisedText;
                proformaInvoiceEntry.CountryOfOrigins = proformaInvoice.CountryOfOrigins;
                proformaInvoiceEntry.Remarks = proformaInvoice.Remarks;
                _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.Update(proformaInvoiceEntry);
                _proformaInvoiceUnitOfWork.Save();

                #region Remove  Old Third Table Rows
                foreach (var productRelation in proformaInvoiceEntry.ProductsRelationICollection.ToList())
                {
                    _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.DeleteFromDb(productRelation.Id);
                    _proformaInvoiceUnitOfWork.Save();
                }
                //foreach (var portShipment in proformaInvoiceEntry.PfiPortOfShipmentCollection.ToList())
                //{
                //    _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.DeletePermanent(portShipment.Id);
                //    _pfiPortOfShipmentUnitOfWork.Save();
                //}
                #endregion

                #region Add New Third Tables
                foreach (var productRelation in proformaInvoice.ProductsRelationICollection.ToList())
                {
                    productRelation.PfiId = proformaInvoiceEntry.Id;
                    _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.Add(productRelation);
                    _proformaInvoiceUnitOfWork.Save();
                }
                //foreach (var portShipment in proformaInvoice.PfiPortOfShipmentCollection.ToList())
                //{
                //    portShipment.PfiId = proformaInvoiceEntry.Id;
                //    _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.Add(portShipment);
                //    _pfiPortOfShipmentUnitOfWork.Save();
                //}
                #endregion
                #endregion
            }
        }



        public IEnumerable<ProformaInvoice> GetPfiPagedList(int index, int length, string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? acmId,
             string sortColumnName, string sortDirection, out int recordsTotal, out int recordsFiltered)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetDynamic(out recordsTotal, out recordsFiltered, x => ((x.ParentPfiId == null) &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) &&
              (acmId == null || x.Buyer.AcmId == acmId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) &&
              (sDateTo == null || x.PfiDate < toDate) &&
              (category == null || x.PfiCategory == category)), sortColumnName + " " + sortDirection, "",
            index, length).ToList();
            return proformaInvoiceList;
        }

        public IEnumerable<ProformaInvoice> GetAllPfi(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? acmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);
            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => ((x.ParentPfiId == null) &&
             (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
             (buyerId == null || x.BuyerId == buyerId) &&
              (sDateFrom == null || x.PfiDate >= fromDate) &&
             (sDateTo == null || x.PfiDate < toDate) &&
             (category == null || x.PfiCategory == category) &&
             (acmId == null || x.Buyer.AcmId == acmId)), x => x.OrderByDescending(y => y.CreatedAt)).ToList();
            return proformaInvoiceList;
        }



        public int GetProformaInvoicesCount()
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetCount();
        }
        public int GetParentPFICount()
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetParentPFICount();
        }

        public ProformaInvoice GetProformaInvoiceById(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetById(id);
        }

        public IEnumerable<ProformaInvoice> GetAllProformaInvoicesByBuyerId(int buyerId)
        {
            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => (!x.IsDeleted) && (x.BuyerId == buyerId), x => x.OrderByDescending(y => y.PfiNumber)).ToList();
            return proformaInvoiceList;
        }
        public IEnumerable<ProformaInvoice> GetAllPIForLcByBuyerId(int buyerId)
        {
            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => (!x.IsDeleted) && (x.IsClosed != true) && (x.Status != 0)  && (x.BuyerId == buyerId) && (!x.ParentPfiId.HasValue), x => x.OrderByDescending(y => y.PfiNumber)).ToList();
            return proformaInvoiceList;
        }

        public ProformaInvoiceProductRelation GetPIProductRelationById(int piId, int productId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetPIProductRelationById(piId, productId);
        }

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableEditProducts(int pfiId, int ciId)
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetComInvoiceableEditProducts(pfiId, ciId);
        //}

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoicedProducts(int pfiId, int ciId)
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetComInvoicedProducts(pfiId, ciId);
        //}

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableProducts(int pfiId)
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetComInvoiceableProducts(pfiId);
        //}
        //public IEnumerable<LcPendingOrderDataModel> GetComInvoiceablePendingOrderData(int pfiId)
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetComInvoiceablePendingOrderData(pfiId);
        //}
        public Customer GetDefaultSeller()
        {
            _customerUnitOfWork = new CustomerUnitOfWork(_context);
            return _customerUnitOfWork.CustomerRepository.GetDefaultSeller();
        }


        public string GetProformaInvoiceNo(int year)
        {
            return (_proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetCountByYear(year) + 1).ToString("0000") + "/HM/" + year.ToString("0000");
            // return DateTime.Now.Year.ToString("0000") + "/" + (_proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetCountByYear(DateTime.Now.Year) + 1).ToString("000000");
        }

        //public ICollection<PfiPortOfShipment> GetPortOfShipmentByPfi(int id)
        //{
        //    return _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.GetAllByPfi(id);
        //}

        public ICollection<ProformaInvoiceProductRelation> GetProductsRelationByPfi(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetAllByPFI(id);
        }

        public void CancelPFi(int id, int currentUserId)
        {
            var pfiEntry = _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.GetById(id);
            if (pfiEntry != null)
            {
                pfiEntry.Status = (byte)EnumActiveDactiveStatus.Inactive;
                pfiEntry.UpdatedBy = currentUserId;
                pfiEntry.UpdatedAt = DateTime.Now;
                _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.Update(pfiEntry);
                _proformaInvoiceUnitOfWork.Save();
            }
        }
        public IEnumerable<ProformaInvoice> GetPfiWithOutLcPagedList(int page, int pageSize, string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? sAcmId, string sort, string sortDir, out int totalRecords, out int recordsFiltered)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetDynamic(out totalRecords, out recordsFiltered, x => x.IsClosed != true && x.Status != (byte)EnumActiveDactiveStatus.Inactive  && ((x.ParentPfiId == null) &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) &&
              (sDateTo == null || x.PfiDate < toDate)), sort + " " + sortDir, "",
            page, pageSize).ToList();
            return proformaInvoiceList;
        }
        public IEnumerable<ProformaInvoice> GetPfiWithOutLc(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => x.IsClosed != true && x.Status != (byte)EnumActiveDactiveStatus.Inactive && x.ParentPfiId == null &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
              (sDateTo == null || x.PfiDate < toDate)).ToList();
            return proformaInvoiceList;
        }

        //public IEnumerable<ProformaInvoice> GetForthComingLcPagedList(int page, int pageSize, string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? sAcmId, string sort, string sortDir, out int totalRecords, out int recordsFiltered)
        //{
        //    var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
        //    var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

        //    var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetDynamic(out totalRecords, out recordsFiltered, x => !x.LcPfiCollection.Any() && x.ForthComingLcDate.HasValue && ((x.ParentPfiId == null) &&
        //      (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
        //      (buyerId == null || x.BuyerId == buyerId) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
        //       (sDateFrom == null || x.ForthComingLcDate >= fromDate) &&
        //      (sDateTo == null || x.ForthComingLcDate < toDate)), sort + " " + sortDir, "",
        //    page, pageSize).ToList();
        //    return proformaInvoiceList;
        //}
        //public IEnumerable<ProformaInvoice> GetForthComingLc(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        //{
        //    var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
        //    var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

        //    var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => x.ParentPfiId == null && !x.LcPfiCollection.Any() && x.ForthComingLcDate.HasValue &&
        //      (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
        //      (buyerId == null || x.BuyerId == buyerId) &&
        //       (sDateFrom == null || x.ForthComingLcDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
        //      (sDateTo == null || x.ForthComingLcDate < toDate)).OrderBy(x => x.ForthComingLcDate).ToList();
        //    return proformaInvoiceList;
        //}
        public double? GetCloseCancelPfiValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetCloseCancelPfiValue(sDateFrom, sDateTo, buyerId, pfiNumber, sAcmId);
        }

        public Tuple<double, double> GetTotalProductQtyValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId, int? productId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var list = _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetAll(x => (
              (pfiNumber == null || x.ProformaInvoice.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.ProformaInvoice.BuyerId == buyerId) && (sAcmId == null || x.ProformaInvoice.Buyer.AcmId == sAcmId) && (productId == null || x.ProductId == productId) &&
               (sDateFrom == null || x.ProformaInvoice.PfiDate >= fromDate) &&
              (sDateTo == null || x.ProformaInvoice.PfiDate < toDate)));
            return Tuple.Create(list.Any() ? list.Sum(s => s.Quantity) : 0, list.Sum(s => s.TotalPrice ?? 0));
        }

        public double? GetPoWithOutLcTotalPfiValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetPoWithOutLcTotalPfiValue(sDateFrom, sDateTo, buyerId, pfiNumber, sAcmId);

        }


        public double? GetTotalAmount(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetTotalAmount(sDateFrom, sDateTo, buyerId, pfiNumber, sAcmId);
        }

        public double? GetTotalQty(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetTotalQty(sDateFrom, sDateTo, buyerId, pfiNumber, sAcmId);
        }

        public IEnumerable<ProformaInvoice> GetPfiCloseCancel(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => x.ParentPfiId == null && (x.Status == (byte)EnumActiveDactiveStatus.Inactive || x.IsClosed == true) &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
              (sDateTo == null || x.PfiDate < toDate)).ToList();
            return proformaInvoiceList;
        }

        public IEnumerable<ProformaInvoiceProductRelation> GetPFiLCProducts(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId, int? productId)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetAll(x => x.ProformaInvoice.ParentPfiId == null &&
              (pfiNumber == null || x.ProformaInvoice.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.ProformaInvoice.BuyerId == buyerId) && (productId == null || x.ProductId == productId) &&
               (sDateFrom == null || x.ProformaInvoice.PfiDate >= fromDate) && (sAcmId == null || x.ProformaInvoice.Buyer.AcmId == sAcmId) &&
              (sDateTo == null || x.ProformaInvoice.PfiDate < toDate)).ToList();
            return proformaInvoiceList.OrderByDescending(o => o.Id);
        }
        //public IEnumerable<ProformaInvoiceProductRelation> GetPagedPFiLCProducts(int page, int pageSize, string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId, int? productId, string sort, string sortDir, out int totalRecords, out int recordsFiltered)
        //{
        //    var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
        //    var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

        //    var pFiLCProducts = _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetDynamic(out totalRecords, out recordsFiltered, x =>
        //      x.ProformaInvoice.ParentPfiId == null && (pfiNumber == null || x.ProformaInvoice.PfiNumber.Contains(pfiNumber)) &&
        //      (buyerId == null || x.ProformaInvoice.BuyerId == buyerId) && (sAcmId == null || x.ProformaInvoice.Buyer.AcmId == sAcmId) && (productId == null || x.ProductId == productId) &&
        //       (sDateFrom == null || x.ProformaInvoice.PfiDate >= fromDate) &&
        //      (sDateTo == null || x.ProformaInvoice.PfiDate < toDate), sort + " " + sortDir, "",
        //    page, pageSize).ToList();
        //    return pFiLCProducts;
        //}

        public IEnumerable<ProformaInvoice> GetPfiCloseCancelPagedList(int page, int pageSize, string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? sAcmId, string sort, string sortDir, out int totalRecords, out int recordsFiltered)
        {
            var fromDate = string.IsNullOrEmpty(sDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(sDateFrom);
            var toDate = (string.IsNullOrEmpty(sDateTo) ? DateTime.Now : Convert.ToDateTime(sDateTo)).AddDays(1);

            var proformaInvoiceList = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetDynamic(out totalRecords, out recordsFiltered, x => (((x.IsClosed.HasValue ? x.IsClosed.Value : false) || x.Status == (byte)EnumActiveDactiveStatus.Inactive) && (x.ParentPfiId == null) &&
              (pfiNumber == null || x.PfiNumber.Contains(pfiNumber)) &&
              (buyerId == null || x.BuyerId == buyerId) && (sAcmId == null || x.Buyer.AcmId == sAcmId) &&
               (sDateFrom == null || x.PfiDate >= fromDate) &&
              (sDateTo == null || x.PfiDate < toDate)), sort + " " + sortDir, "",
            page, pageSize).ToList();
            return proformaInvoiceList;
        }

        public void ClosePfi(int id, int currentUserId)
        {
            var pfiEntry = _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.GetById(id);
            if (pfiEntry != null)
            {
                pfiEntry.IsClosed = true;
                pfiEntry.UpdatedAt = DateTime.Now;
                pfiEntry.UpdatedBy = currentUserId;
                _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.Update(pfiEntry);
                _proformaInvoiceUnitOfWork.Save();
            }
        }
        public void ReopenPfi(int id, int currentUserId)
        {
            var pfiEntry = _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.GetById(id);
            if (pfiEntry != null)
            {
                pfiEntry.IsClosed = false;
                pfiEntry.Status = (byte)EnumActiveDactiveStatus.Active;
                pfiEntry.UpdatedAt = DateTime.Now;
                pfiEntry.UpdatedBy = currentUserId;
                _proformaInvoiceUnitOfWork._ProformaInvoiceRepository.Update(pfiEntry);
                _proformaInvoiceUnitOfWork.Save();
            }
        }
        public string GetRevisionVersionString(int number)
        {
            string addR = null;
            for (int i = 0; i < number; i++)
            {
                addR += "R";
            }
            return addR;
        }
        private Nullable<int> GetRevisedVirsionByPFI(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetRevisedVirsionByPFI(id) + 1;
        }

        public void DeleteProformaInvoice(int id, string currUserId)
        {
            _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.DeletePermanent(id);
            _proformaInvoiceUnitOfWork.Save(currUserId);
        }



        public void AddProductRelation(ProformaInvoiceProductRelation proformaInvoiceProductRelation)
        {
            var newProductRelation = new ProformaInvoiceProductRelation
            {
                PfiId = proformaInvoiceProductRelation.PfiId,
                ProductId = proformaInvoiceProductRelation.ProductId,
                //AgentId = proformaInvoiceProductRelation.AgentId,
                UnitPrice = proformaInvoiceProductRelation.UnitPrice,
                Quantity = proformaInvoiceProductRelation.Quantity,
                TotalPrice = proformaInvoiceProductRelation.TotalPrice,
                ShipmentMode = proformaInvoiceProductRelation.ShipmentMode,
                CreatedBy = proformaInvoiceProductRelation.CreatedBy,
                //CreatedDate = proformaInvoiceProductRelation.CreatedDate

            };
            _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.Add(newProductRelation);
            _proformaInvoiceUnitOfWork.Save(proformaInvoiceProductRelation.CreatedBy.ToString());
        }
        public void DeleteProductRelation(int id)
        {
            _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.DeletePermanent(id);
            _proformaInvoiceUnitOfWork.Save();
        }

        //public void DeletePfiPortOfShipmentCollection(int id)
        //{
        //    _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.DeletePermanent(id);
        //    _pfiPortOfShipmentUnitOfWork.Save();
        //}

        //public void AddPfiPortOfShipment(PfiPortOfShipment pfiPortOfShipment)
        //{
        //    var newPfiPortOfShipment = new PfiPortOfShipment()
        //    {
        //        PfiId = pfiPortOfShipment.PfiId,
        //        CountryId = pfiPortOfShipment.CountryId
        //    };
        //    _pfiPortOfShipmentUnitOfWork.PfiPortOfShipmentRepository.Add(newPfiPortOfShipment);
        //    _pfiPortOfShipmentUnitOfWork.Save();
        //}



        public List<ProformaInvoice> GetAllRivision(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAllRivision(id);
        }
        public bool HasChild(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.HasChild(id);
        }
        public ProformaInvoice GetPreviousRevision(int id)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetPreviousRevision(id);
        }
        public ProformaInvoiceProductRelation GetPIProductRelationByCustomerId(int customerId, int productId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceProductRelationRepository.GetPIProductRelationByCustomerId(customerId, productId);
        }
        public void AddOrEditForthComingLcDate(int id, DateTime? forthComingLcDate)
        {
            var model = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetById(id);
            if (model != null)
            {
                model.ForthComingLcDate = forthComingLcDate;
                _proformaInvoiceUnitOfWork.Save();
            }
        }
        public ProformaInvoice GetBuyerRelativeDataById(int buyerId)
        {
            return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetBuyerRelativeDataById(buyerId);
        }
        // chart //
        //public IEnumerable<TopPfiProductChartData> GetTopPfiForPieChart(int lastDaysCount, int topCount)
        //{
        //    return _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetTopPfiForPieChart(lastDaysCount, topCount);
        //}
        public double GetMonthlyPfiAmount(int year, int month)
        {
            double pfiProductAmount = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => x.ParentPfiId == null && x.PfiDate.Year == year && x.PfiDate.Month == month).Sum(x => x.PfiValue.Value);
            return pfiProductAmount;
        }
        public double? GetAcMWiseTargetAmount(int year, int month, int acmId)
        {
            var targetAmount = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => x.ForthComingLcDate != null && x.ForthComingLcDate.Value.Year == year && x.ForthComingLcDate.Value.Month == month && x.Buyer.AcmId == acmId).Sum(s => s.PfiValue);
            return targetAmount;
        }

        public void Dispose()
        {
            _proformaInvoiceUnitOfWork.Dispose();
        }

        #region for API
        //public List<PfiApiDataModel> GetAllPfiReportForApi(int? SBuyerId, string SPfiCategory, int? SMonth, int? SYear, int loggedUserId)
        //{
        //    var model = _proformaInvoiceUnitOfWork.ProformaInvoiceRepository.GetAll(x => (x.ParentPfiId == null) && (x.Buyer.AcmId == loggedUserId) &&
        //    (SPfiCategory == null || x.PfiCategory == SPfiCategory) &&
        //   (SBuyerId == null || x.BuyerId == SBuyerId) &&
        //   (SMonth == null || x.PfiDate.Month == SMonth) &&
        //   (SYear == null || x.PfiDate.Year == SYear)
        //   ).OrderByDescending(o => o.PfiDate).Select(s => new PfiApiDataModel()
        //   {
        //       BuyerOrSellerName = s.Buyer != null ? s.Buyer.Name : "",
        //       PfiId = s.Id,
        //       PfiDate = s.PfiDate,
        //       PfiNumber = s.PfiNumber,
        //       PfiValue = s.PfiValue,

        //   }).ToList();
        //    return model;
        //}
        #endregion
    }
}