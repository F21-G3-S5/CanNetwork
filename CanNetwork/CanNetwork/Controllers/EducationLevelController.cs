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
    public class EducationLevelController : Controller
    {
        MyDbContext db = new MyDbContext();
        // GET: Language
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.EducationLevels.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public JsonResult Create(EducationLevel educationLevel)
        {
            if (ModelState.IsValid)
            {
                db.EducationLevels.Add(educationLevel);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var educationLevel = db.EducationLevels.SingleOrDefault(el => el.Id == id);
            if (educationLevel != null)
            {
                db.EducationLevels.Remove(educationLevel);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var educationLevel = db.EducationLevels.SingleOrDefault(el => el.Id == id);
            if (educationLevel != null)
            {
                return Json(educationLevel, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(EducationLevel educationLevel)
        {
            db.Entry(educationLevel).State = EntityState.Modified;
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