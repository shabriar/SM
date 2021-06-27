using PagedList;
using SCHM.Web.Models;
using Stationary_Management.Entity;
using Stationary_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Controllers
{
    public class ProformaInvoiceController : Controller
    {
        //[OutputCache(Duration = 30)]
        [Roles("Global_SupAdmin,Proforma_Invoice_Create,Proforma_Invoice_Edit,Proforma_Invoice_Report")]
        public ActionResult Index(ProformaInvoiceSearchModel model)
        {

                var acmId = new UserModel().GetUserById(AuthenticatedUser.GetUserFromIdentity().UserId);
                model.AcMSelectList = model.AcMSelectList.Where(x => x.Value == acmId.ToString());
                model.SAcmId = acmId.Id;

            model.ProformaInvoicePagedList = new StaticPagedList<ProformaInvoice>(new ProformaInvoiceModel().GetPfiPagedList(model), model.Page, model.PageSize, model.TotalRecords);
            return View(model);
        }
        //[OutputCache(Duration = 30)]
        [Roles("Global_SupAdmin,Proforma_Invoice_Create")]
        public ActionResult Add()
        {
            var model = new ProformaInvoiceModel();
            model.IsPercentChecked = true;
            //model.PortOfShipmentIds = new List<int>();
            //var Country = new CountryMasterModel().GetCountryByName("Singapore");
            //int CountryIdOdSingapore = Country.Id;
            //model.PortOfShipmentIds.Add(CountryIdOdSingapore);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProformaInvoiceModel proformaInvoiceModel)
        {

            if (ModelState.IsValid)
            {
                int retrnId = proformaInvoiceModel.AddProformaInvoice();
                return RedirectToAction("Details", "ProformaInvoice", new { id = retrnId });
            }
            return View(proformaInvoiceModel);
        }
        [Roles("Global_SupAdmin,Proforma_Invoice_Edit")]
        public ActionResult Edit(int id)
        {
            var proformaInvoiceModel = new ProformaInvoiceModel(id);
            if (!(proformaInvoiceModel.Id > 0))
            {
                return HttpNotFound();
            }
            return View(proformaInvoiceModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProformaInvoiceModel proformaInvoiceModel)
        {
            if (ModelState.IsValid)
            {
                proformaInvoiceModel.EditProformaInvoice();
                return RedirectToAction("Details", "ProformaInvoice", new { id = proformaInvoiceModel.Id });
            }
            return View(proformaInvoiceModel);
        }
        public ActionResult Details(int id)
        {
            var proformaInvoiceModel = new ProformaInvoiceModel(id);
            if (!(proformaInvoiceModel.Id > 0))
            {
                return HttpNotFound();
            }
            return View(proformaInvoiceModel);
        }
    }
}