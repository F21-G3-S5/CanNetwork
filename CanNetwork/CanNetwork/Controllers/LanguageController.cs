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
    public class LanguageController : Controller
    {
        MyDbContext db = new MyDbContext();
        // GET: Language
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.Languages.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public JsonResult Create(Language language)
        {
            if (ModelState.IsValid)
            {
                db.Languages.Add(language);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var language = db.Languages.SingleOrDefault(l => l.Id == id);
            if (language != null)
            {
                db.Languages.Remove(language);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var language = db.Languages.SingleOrDefault(l => l.Id == id);
            if (language != null)
            {
                return Json(language, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(Language language)
        {
            db.Entry(language).State = EntityState.Modified;
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