using SCHM.Services;
using SCHM.Web.Models;
using Stationary_Management.Entity;
using Stationary_Management.service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Models
{
    [NotMapped]
    public class ProformaInvoiceModel : ProformaInvoice
    {
        private ProformaInvoiceService _proformaInvoiceService;
        private CustomerService _customerService;
        //private GeneralConfigService _generalConfigService;
        //private PaymentTermsService _paymentTermsService;
        private ProductService _productService;
        //private CompanyService _companyService;
        //private CurrencyService _currencyService;
        // private AgentService _generalConfigService;


        public List<Customer> BuyerList;
        public List<Customer> SellerList;
        public IEnumerable<SelectListItem> PortOfShipmentList;
        public IEnumerable<SelectListItem> CountryOfOriginList;

        //public List<PaymentTerms> PaymentTermsList;
        public List<ProformaInvoiceProductRelation> PFIProductList { get; set; }
        public IEnumerable<Products> ProductList { get; set; }
        public IEnumerable<SelectListItem> AgentList { get; set; }
        //public Company Company { get; set; }
        public int CurrentUserId { get; set; }
        public bool IsPercentChecked { get; set; }  //For check Box to select percentage. If selected (True) percentage will be calculated other wise flat value .

        [Required]
        [Display(Name = "Port Of Shipment")]
        public List<int> PortOfShipmentIds { get; set; }
        public List<string> CountryOfOriginIds { get; set; }
        public IEnumerable<SelectListItem> PaymentTermsSelectList { get; set; }
        public ProformaInvoiceModel()
        {
            _proformaInvoiceService = new ProformaInvoiceService();
            _customerService = new CustomerService();
            //_generalConfigService = new GeneralConfigService();
            _productService = new ProductService();
            //_companyService = new CompanyService();
            //_paymentTermsService = new PaymentTermsService();
            //_currencyService = new CurrencyService();

            BuyerList = _customerService.GetAllBuyer().ToList();
            SellerList = _customerService.GetAllSeller().ToList();
            //PortOfShipmentList = new SelectList(_generalConfigService.GetAllCountries(), "Id", "ConfigValue");
            //CountryOfOriginList = new SelectList(_generalConfigService.GetAllCountries(), "ConfigValue", "ConfigValue");
            //AgentList = new SelectList(_generalConfigService.GetAllAgents(), "Id", "ConfigValue");
            //PaymentTermsList = _paymentTermsService.GetAllPaymentTerms().ToList();
            //Company = _companyService.GetCompanies();
            CurrentUserId = AuthenticatedUser.GetUserFromIdentity().UserId;
            PaymentTermsSelectList = new SelectList(Enumerable.Empty<SelectListItem>());

            ProductList = new List<Products>();
            ProductList = _proformaInvoiceService.GetAllProductsForPfi(base.BuyerId);
        }
        public ProformaInvoiceModel(int? id) : this()
        {
            if (id != null)
            {
                var proformaInvoiceEntry = _proformaInvoiceService.GetProformaInvoiceById(id.Value);
                Id = proformaInvoiceEntry.Id;
                PfiNumber = proformaInvoiceEntry.PfiNumber;
                PfiDate = proformaInvoiceEntry.PfiDate;
                PoNo = proformaInvoiceEntry.PoNo;
                Buyer = proformaInvoiceEntry.Buyer;
                Seller = proformaInvoiceEntry.Seller;
                BuyerId = proformaInvoiceEntry.BuyerId;
                SellerId = proformaInvoiceEntry.SellerId;
                PfiValue = proformaInvoiceEntry.PfiValue;
                TotalQuantity = proformaInvoiceEntry.TotalQuantity;
                SeaFreightChargePerUnit = proformaInvoiceEntry.SeaFreightChargePerUnit;
                AirFreightChargePerUnit = proformaInvoiceEntry.AirFreightChargePerUnit;
                IndentValidity = proformaInvoiceEntry.IndentValidity;
                PfiCategory = proformaInvoiceEntry.PfiCategory;
                ProductType = proformaInvoiceEntry.ProductType;
                //PaymentTermsId = proformaInvoiceEntry.PaymentTermsId;
                IncoTerms = proformaInvoiceEntry.IncoTerms;
                //PaymentTerms = proformaInvoiceEntry.PaymentTerms;
                Status = proformaInvoiceEntry.Status;
                IsPercentChecked = (proformaInvoiceEntry.DiscountType.ToString() == "Percent") ? true : false;
                DiscountValue = proformaInvoiceEntry.DiscountValue;
                DiscountType = proformaInvoiceEntry.DiscountType;
                IsClosed = proformaInvoiceEntry.IsClosed;
                //CurrencyId = proformaInvoiceEntry.CurrencyId;
                ParentPfiId = proformaInvoiceEntry.ParentPfiId;
                ParentPfi = proformaInvoiceEntry.ParentPfi;
                //Currency = proformaInvoiceEntry.Currency;
                CreatedAt = proformaInvoiceEntry.CreatedAt;
                CreatedBy = proformaInvoiceEntry.CreatedBy;
                CreatedByUser = proformaInvoiceEntry.CreatedByUser;
                UpdatedAt = proformaInvoiceEntry.UpdatedAt;
                UpdatedBy = proformaInvoiceEntry.UpdatedBy;
                UpdatedByUser = proformaInvoiceEntry.UpdatedByUser;
                Remarks = proformaInvoiceEntry.Remarks;
                RevisedText = proformaInvoiceEntry.RevisedText;
                ForthComingLcDate = proformaInvoiceEntry.ForthComingLcDate;
                CountryOfOrigins = proformaInvoiceEntry.CountryOfOrigins;

                ProductsRelationICollection = proformaInvoiceEntry.ProductsRelationICollection;
                //PfiPortOfShipmentCollection = proformaInvoiceEntry.PfiPortOfShipmentCollection;
                //PaymentTermsSelectList = new SelectList(_paymentTermsService.GetPaymentTermsByIncoterms(proformaInvoiceEntry.IncoTerms), "Id", "TermsName", proformaInvoiceEntry.PaymentTermsId);
                //LcPfiCollection = proformaInvoiceEntry.LcPfiCollection;

                //PortOfShipmentIds = proformaInvoiceEntry.PfiPortOfShipmentCollection.Select(x => x.CountryId).ToList();
                // country of origins //
                if (CountryOfOrigins != null)
                {
                    CountryOfOriginIds = CountryOfOrigins.Split('/').ToList();
                }
            }
        }
        //public ProformaInvoiceModel(int? id, bool loadReview) : this()
        //{
        //    if (id != null)
        //    {
        //        var proformaInvoiceEntry = _proformaInvoiceService.GetProformaInvoiceById(id.Value);
        //        List<ProformaInvoice> proformaInvoiceRevisionList = _proformaInvoiceService.GetAllRivision(id.Value);
        //        if (proformaInvoiceRevisionList.Any())
        //        {
        //            var lastRivision = proformaInvoiceRevisionList.OrderByDescending(s => s.Id).FirstOrDefault();
        //            proformaInvoiceEntry = lastRivision;

        //            proformaInvoiceEntry.PfiPortOfShipmentCollection = null;
        //            proformaInvoiceEntry.ProductsRelationICollection = null;

        //            proformaInvoiceEntry.PfiPortOfShipmentCollection = _proformaInvoiceService.GetPortOfShipmentByPfi(lastRivision.Id);
        //            proformaInvoiceEntry.ProductsRelationICollection = _proformaInvoiceService.GetProductsRelationByPfi(lastRivision.Id);
        //            proformaInvoiceEntry.Id = id.Value;
        //            proformaInvoiceEntry.ParentPfiId = null;
        //        }

        //        Id = proformaInvoiceEntry.Id;
        //        PfiNumber = proformaInvoiceEntry.PfiNumber;
        //        PfiDate = proformaInvoiceEntry.PfiDate;
        //        PoNo = proformaInvoiceEntry.PoNo;
        //        Buyer = proformaInvoiceEntry.Buyer;
        //        Seller = proformaInvoiceEntry.Seller;
        //        BuyerId = proformaInvoiceEntry.BuyerId;
        //        SellerId = proformaInvoiceEntry.SellerId;
        //        PfiValue = proformaInvoiceEntry.PfiValue;
        //        TotalQuantity = proformaInvoiceEntry.TotalQuantity;
        //        SeaFreightChargePerUnit = proformaInvoiceEntry.SeaFreightChargePerUnit;
        //        AirFreightChargePerUnit = proformaInvoiceEntry.AirFreightChargePerUnit;
        //        IndentValidity = proformaInvoiceEntry.IndentValidity;
        //        PfiCategory = proformaInvoiceEntry.PfiCategory;
        //        ProductType = proformaInvoiceEntry.ProductType;
        //        PaymentTermsId = proformaInvoiceEntry.PaymentTermsId;
        //        PaymentTerms = proformaInvoiceEntry.PaymentTerms;
        //        IncoTerms = proformaInvoiceEntry.IncoTerms;
        //        Status = proformaInvoiceEntry.Status;
        //        ParentPfiId = proformaInvoiceEntry.ParentPfiId;
        //        //IsClosed = proformaInvoiceEntry.IsClosed;
        //        CurrencyId = proformaInvoiceEntry.CurrencyId;
        //        Currency = proformaInvoiceEntry.Currency;
        //        CountryOfOrigins = proformaInvoiceEntry.CountryOfOrigins;
        //        CreatedAt = proformaInvoiceEntry.CreatedAt;
        //        CreatedBy = proformaInvoiceEntry.CreatedBy;
        //        CreatedByUser = proformaInvoiceEntry.CreatedByUser;
        //        UpdatedAt = proformaInvoiceEntry.UpdatedAt;
        //        UpdatedBy = proformaInvoiceEntry.UpdatedBy;
        //        UpdatedByUser = proformaInvoiceEntry.UpdatedByUser;
        //        Remarks = proformaInvoiceEntry.Remarks;
        //        CountryOfOrigins = proformaInvoiceEntry.CountryOfOrigins;
        //        base.ProductsRelationICollection = proformaInvoiceEntry.ProductsRelationICollection;

        //        base.PfiPortOfShipmentCollection = proformaInvoiceEntry.PfiPortOfShipmentCollection;
        //        PaymentTermsSelectList = new SelectList(_paymentTermsService.GetPaymentTermsByIncoterms(proformaInvoiceEntry.IncoTerms), "Id", "TermsName", proformaInvoiceEntry.PaymentTermsId);

        //        PortOfShipmentIds = proformaInvoiceEntry.PfiPortOfShipmentCollection.Select(x => x.CountryId).ToList();
        //        // country of origins //
        //        if (CountryOfOrigins != null)
        //        {
        //            CountryOfOriginIds = CountryOfOrigins.Split('/').ToList();
        //        }
        //    }
        //}
        public IEnumerable<ProformaInvoice> GetPfiPagedList(ProformaInvoiceSearchModel model)
        {
            var pfiPagedList = _proformaInvoiceService.GetPfiPagedList(model.Page, model.PageSize, model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.Category, model.SAcmId,
                model.Sort, model.SortDir, out int totalRecords, out int recordsFiltered);
            model.TotalRecords = recordsFiltered;
            return pfiPagedList;
        }
        public IEnumerable<ProformaInvoice> GetAllProformaInvoice(ProformaInvoiceSearchModel model)
        {
            return _proformaInvoiceService.GetAllPfi(model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.Category, model.SAcmId);
        }
        public IEnumerable<ProformaInvoice> GetAllProformaInvoice(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, string category, int? acmId)
        {
            return _proformaInvoiceService.GetAllPfi(sDateFrom, sDateTo, pfiNumber, buyerId, category, acmId);
        }
        public int AddProformaInvoice()
        {
            base.Status = 1;
            base.CreatedBy = CurrentUserId;
            base.CreatedAt = DateTime.Now;
            //base.CurrencyId = _currencyService.GetUsdCurrency().Id;
            DiscountType = IsPercentChecked ? "Percent" : "Flat";

            // Country Of Origins //
            string strCountryOfOrigins = "";
            if (CountryOfOriginIds != null)
            {
                foreach (var item in CountryOfOriginIds)
                {
                    strCountryOfOrigins += item + "/";
                }
                char[] charsToTrim = { '/', ' ' };
                strCountryOfOrigins = strCountryOfOrigins.Trim(charsToTrim);
            }
            base.CountryOfOrigins = strCountryOfOrigins;
            int returnId = _proformaInvoiceService.AddProformaInvoice(this);
            foreach (var productRelation in PFIProductList)
            {
                if (productRelation.ProductId != 0)
                {
                    productRelation.PfiId = returnId;
                    productRelation.CreatedBy = CurrentUserId;
                    productRelation.CreatedAt = DateTime.Now;
                    _proformaInvoiceService.AddProductRelation(productRelation);
                }
            }
            // PORT OF SHIPMENNT ASSIGN //
            //var singapore = new CountryMasterModel().GetCountryByName("Singapore");
            //if (singapore != null && PortOfShipmentIds.Any(x => x == singapore.Id))
            //{
            //    var newPfiPortOfShipment = new PfiPortOfShipment()
            //    {
            //        PfiId = returnId,
            //        CountryId = singapore.Id
            //    };
            //    _proformaInvoiceService.AddPfiPortOfShipment(newPfiPortOfShipment);
            //}
            //foreach (var posId in PortOfShipmentIds.Where(x => x != singapore.Id))
            //{
            //    var newPfiPortOfShipment = new PfiPortOfShipment()
            //    {
            //        PfiId = returnId,
            //        CountryId = posId
            //    };
            //    _proformaInvoiceService.AddPfiPortOfShipment(newPfiPortOfShipment);
            //}

            return returnId;
        }
        public void EditProformaInvoice()
        {
            base.UpdatedAt = DateTime.Now;
            base.UpdatedBy = CurrentUserId;
            base.Status = 1;
            DiscountType = IsPercentChecked ? "Percent" : "Flat";
            base.ProductsRelationICollection = PFIProductList.ToList();

            // Country Of Origins //
            string strCountryOfOrigins = "";
            if (CountryOfOriginIds != null)
            {
                foreach (var item in CountryOfOriginIds)
                {
                    strCountryOfOrigins += item + "/";
                }
                char[] charsToTrim = { '/', ' ' };
                strCountryOfOrigins = strCountryOfOrigins.Trim(charsToTrim);
            }
            base.CountryOfOrigins = strCountryOfOrigins;

            var shipmentCountries = new List<int> { };
            //var singapore = new CountryMasterModel().GetCountryByName("Singapore");
            //if (singapore != null && PortOfShipmentIds.Any(x => x == singapore.Id))
            //{
            //    shipmentCountries.Add(singapore.Id);
            //}
            //foreach (var posId in PortOfShipmentIds.Where(x => x != singapore.Id))
            //{
            //    shipmentCountries.Add(posId);
            //}
            //base.PfiPortOfShipmentCollection = shipmentCountries.Select(x => new PfiPortOfShipment() { CountryId = x }).ToList();
            _proformaInvoiceService.EditProformaInvoice(this);

        }
        public void DeleteProformaInvoice(int id)
        {
            _proformaInvoiceService.DeleteProformaInvoice(id, AuthenticatedUser.GetUserFromIdentity().UserId.ToString());
        }
        public List<ProformaInvoice> GetAllRivision(int id)
        {
            return _proformaInvoiceService.GetAllRivision(id);
        }
        public ProformaInvoice GetPreviousRevision(int id)
        {
            return _proformaInvoiceService.GetPreviousRevision(id);
        }
        public ProformaInvoice GetProformaInvoicesById(int id)
        {
            return _proformaInvoiceService.GetProformaInvoiceById(id);
        }
        public IEnumerable<ProformaInvoice> GetAllPIForLcByBuyerId(int buyerId)
        {
            return _proformaInvoiceService.GetAllPIForLcByBuyerId(buyerId);
        }
        public ProformaInvoiceProductRelation GetPIProductRelationByCustomerId(int customerId, int productId)
        {
            return _proformaInvoiceService.GetPIProductRelationByCustomerId(customerId, productId);
        }
        public ProformaInvoiceProductRelation GetPIProductRelationById(int piId, int productId)
        {
            return _proformaInvoiceService.GetPIProductRelationById(piId, productId);
        }

        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableProducts(int pfiId)
        //{
        //    return _proformaInvoiceService.GetComInvoiceableProducts(pfiId);
        //}
        //public IEnumerable<LcPendingOrderDataModel> GetComInvoiceablePendingOrderData(int pfiId)
        //{
        //    return _proformaInvoiceService.GetComInvoiceablePendingOrderData(pfiId);
        //}
        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoiceableEditProducts(int pfiId, int ciId)
        //{
        //    return _proformaInvoiceService.GetComInvoiceableEditProducts(pfiId, ciId);
        //}
        //public IEnumerable<ProformaInvoiceProductRelation> GetComInvoicedProducts(int pfiId, int ciId)
        //{
        //    return _proformaInvoiceService.GetComInvoicedProducts(pfiId, ciId);
        //}

        public void ClosePfi(int id)
        {
            _proformaInvoiceService.ClosePfi(id, CurrentUserId);
        }

        public void CancelPFi(int id)
        {
            _proformaInvoiceService.CancelPFi(id, CurrentUserId);
        }
        public void ReopenPfi(int id)
        {
            _proformaInvoiceService.ReopenPfi(id, CurrentUserId);
        }

        //public IEnumerable<ProformaInvoice> GetPfiWithOutLcPagedList(ProformaInvoiceSearchModel model)
        //{
        //    var pfiPagedList = _proformaInvoiceService.GetPfiWithOutLcPagedList(model.Page, model.PageSize, model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.Category, model.SAcmId,
        //        model.Sort, model.SortDir, out int totalRecords, out int recordsFiltered);
        //    model.TotalRecords = recordsFiltered;
        //    return pfiPagedList;
        //}
        //public IEnumerable<ProformaInvoice> GetForthComingLcPagedList(ProformaInvoiceSearchModel model)
        //{
        //    var pfiPagedList = _proformaInvoiceService.GetForthComingLcPagedList(model.Page, model.PageSize, model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.Category, model.SAcmId,
        //        model.Sort, model.SortDir, out int totalRecords, out int recordsFiltered);
        //    model.TotalRecords = recordsFiltered;
        //    return pfiPagedList;
        //}
        public IEnumerable<ProformaInvoice> GetPfiCloseCancelPagedList(ProformaInvoiceSearchModel model)
        {
            var pfiPagedList = _proformaInvoiceService.GetPfiCloseCancelPagedList(model.Page, model.PageSize, model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.Category, model.SAcmId,
                model.Sort, model.SortDir, out int totalRecords, out int recordsFiltered);
            model.TotalRecords = recordsFiltered;
            return pfiPagedList;
        }

        //public IEnumerable<ProformaInvoiceProductRelation> GetPFiLCProducts(POProductSearchModel model)
        //{
        //    var pFiLCProducts = _proformaInvoiceService.GetPagedPFiLCProducts(model.Page, model.PageSize, model.SDateFrom, model.SDateTo, model.PfiNumber, model.BuyerId, model.SAcmId, model.ProductId,
        //       model.Sort, model.SortDir, out int totalRecords, out int recordsFiltered);
        //    model.TotalRecords = recordsFiltered;
        //    return pFiLCProducts;
        //}
        public IEnumerable<ProformaInvoiceProductRelation> GetPFiLCProducts(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId, int? productId)
        {
            return _proformaInvoiceService.GetPFiLCProducts(sDateFrom, sDateTo, pfiNumber, buyerId, sAcmId, productId);
        }
        public IEnumerable<ProformaInvoice> GetPfiWithOutLc(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        {
            return _proformaInvoiceService.GetPfiWithOutLc(sDateFrom, sDateTo, pfiNumber, buyerId, sAcmId);
        }
        //public IEnumerable<ProformaInvoice> GetForthComingLc(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        //{
        //    return _proformaInvoiceService.GetForthComingLc(sDateFrom, sDateTo, pfiNumber, buyerId, sAcmId);
        //}
        public IEnumerable<ProformaInvoice> GetPfiCloseCancel(string sDateFrom, string sDateTo, string pfiNumber, int? buyerId, int? sAcmId)
        {
            return _proformaInvoiceService.GetPfiCloseCancel(sDateFrom, sDateTo, pfiNumber, buyerId, sAcmId);
        }


        public double? GetTotalQty(string SDateFrom, string SDateTo, int? BuyerId, string PfiNumber, int? SAcmId)
        {
            return _proformaInvoiceService.GetTotalQty(SDateFrom, SDateTo, BuyerId, PfiNumber, SAcmId);
        }
        public double? GetTotalAmount(string SDateFrom, string SDateTo, int? BuyerId, string PfiNumber, int? SAcmId)
        {
            return _proformaInvoiceService.GetTotalAmount(SDateFrom, SDateTo, BuyerId, PfiNumber, SAcmId);
        }
        public double? GetPoWithOutLcTotalPfiValue(string SDateFrom, string SDateTo, int? BuyerId, string PfiNumber, int? SAcmId)
        {
            return _proformaInvoiceService.GetPoWithOutLcTotalPfiValue(SDateFrom, SDateTo, BuyerId, PfiNumber, SAcmId);
        }
        public double? GetCloseCancelPfiValue(string SDateFrom, string SDateTo, int? BuyerId, string PfiNumber, int? SAcmId)
        {
            return _proformaInvoiceService.GetCloseCancelPfiValue(SDateFrom, SDateTo, BuyerId, PfiNumber, SAcmId);
        }

        /// <param name="First double">Total Quantity </param>
        /// <param name="Second double">Total Value/Price</param>
        public Tuple<double, double> GetTotalProductQtyValue(string sDateFrom, string sDateTo, int? buyerId, string pfiNumber, int? sAcmId, int? productId)
        {
            return _proformaInvoiceService.GetTotalProductQtyValue(sDateFrom, sDateTo, buyerId, pfiNumber, sAcmId, productId);
        }

        public void AddOrEditForthComingLcDate(int id, DateTime? forthComingLcDate)
        {
            _proformaInvoiceService.AddOrEditForthComingLcDate(id, forthComingLcDate);
        }
        // For Api 
        //public IEnumerable<Product> GetAllProductsForPfi(int buyerId)
        //{
        //    return _proformaInvoiceService.GetAllProductsForPfi(buyerId);
        //}
        //public ProformaInvoice GetBuyerRelativeDataById(int buyerId)
        //{
        //    return _proformaInvoiceService.GetBuyerRelativeDataById(buyerId);
        //}
    }
}