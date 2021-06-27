using SCHM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stationary_Management.Controllers
{

    [Authorize]
    [Roles("Global_SupAdmin,Configuration")]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View(new ProductModel().GetAllProductss().Where(x => !x.IsDeleted));
        }
        public ActionResult Add()
        {
            return View(new ProductModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductModel model)
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
            return View(new ProductModel(id));
        }

        public ActionResult Edit(int id)
        {
            return View(new ProductModel(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel model)
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
            new ProductModel().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Import()
        {
            return View();
        }
        
    }
}