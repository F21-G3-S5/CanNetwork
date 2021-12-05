using CanNetwork.Context;
using CanNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanNetwork.Controllers
{
    public class CountryController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Country
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.Countries.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public JsonResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(country);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Delete(int id)
        {
            var country = db.Countries.SingleOrDefault(c => c.Id == id);
            if (country != null)
            {
                db.Countries.Remove(country);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var country = db.Countries.SingleOrDefault(c => c.Id == id);
            if (country != null)
            {
                return Json(country, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(Country country)
        {
            db.Entry(country).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = 1 });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}