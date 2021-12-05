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
    public class NationalityController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Nationality
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.Nationalities.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }

        }
        public JsonResult Create(Nationality nationality)
        {
            if (ModelState.IsValid)
            {
                db.Nationalities.Add(nationality);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var nationality = db.Nationalities.SingleOrDefault(c => c.Id == id);
            if (nationality != null)
            {
                db.Nationalities.Remove(nationality);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var nationality = db.Nationalities.SingleOrDefault(c => c.Id == id);
            if (nationality != null)
            {
                return Json(nationality, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(Nationality nationality)
        {
            db.Entry(nationality).State = EntityState.Modified;
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