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
    public class JobTitleController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: JobTitle
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.JobTitles.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        public JsonResult Create(JobTitle jobTitle)
        {
            if (ModelState.IsValid)
            {
                db.JobTitles.Add(jobTitle);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var jobTitle = db.JobTitles.SingleOrDefault(c => c.Id == id);
            if (jobTitle != null)
            {
                db.JobTitles.Remove(jobTitle);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var jobTitle = db.JobTitles.SingleOrDefault(c => c.Id == id);
            if (jobTitle != null)
            {
                return Json(jobTitle, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(JobTitle jobTitle)
        {
            db.Entry(jobTitle).State = EntityState.Modified;
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