using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CanNetwork.Context;
using CanNetwork.Models;

namespace CanNetwork.Controllers
{
    public class CitiesController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Cities
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                return View(db.Cities.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        
        }
        public JsonResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var city = db.Cities.SingleOrDefault(c => c.Id == id);
            if (city != null)
            {
                db.Cities.Remove(city);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var city = db.Cities.SingleOrDefault(c => c.Id == id);
            if (city != null)
            {
                return Json(city, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(City city)
        {
            db.Entry(city).State = EntityState.Modified;
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
