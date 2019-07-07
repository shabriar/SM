using SCHM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Controllers
{
   
    [Authorize]

    public class RequisitionController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View(new RequisitionModel().GetAllRequisitions().Where(x => !x.IsDeleted));
        }
        public ActionResult Add()
        {
            return View(new RequisitionModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RequisitionModel model)
        {
           
                model.Add();
                return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            return View(new RequisitionModel(id));
        }
        public ActionResult Approve(int id)
        {          
            new RequisitionModel().Approve(id);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            return View(new RequisitionModel(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequisitionModel model)
        {
            if (ModelState.IsValid)
            {
                model.Edit();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            new RequisitionModel().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Import()
        {
            return View();
        }

    }
}