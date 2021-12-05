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
    public class UniversityController : Controller
    {
        MyDbContext db = new MyDbContext();
        // GET: Language
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.Universities.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public JsonResult Create(University university)
        {
            if (ModelState.IsValid)
            {
                db.Universities.Add(university);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var university = db.Universities.SingleOrDefault(u => u.Id == id);
            if (university != null)
            {
                db.Universities.Remove(university);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var university = db.Universities.SingleOrDefault(u => u.Id == id);
            if (university != null)
            {
                return Json(university, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(University university)
        {
            db.Entry(university).State = EntityState.Modified;
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