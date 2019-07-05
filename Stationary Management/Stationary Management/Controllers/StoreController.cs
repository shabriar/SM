using SCHM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Controllers
{
    
     [Authorize]
   // [Roles("Global_SupAdmin,Configuration")]
    public class StoreController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View(new StoreModel().GetAllStore().Where(x => !x.IsDeleted));
        }
        public ActionResult Add()
        {
            return View(new StoreModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(StoreModel model)
        {
            if (ModelState.IsValid)
            {
                model.Add();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View(new StoreModel(id));
        }

        public ActionResult Edit(int id)
        {
            return View(new StoreModel(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoreModel model)
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
            new StoreModel().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Import()
        {
            return View();
        }

    }
}