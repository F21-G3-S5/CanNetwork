using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CanNetwork.Context;
using CanNetwork.Models;

namespace CanNetwork.Controllers
{
    public class JobTypeController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: JobType
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.JobTypes.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }

        }
        public JsonResult Create(JobType jobType)
        {
            if (ModelState.IsValid)
            {
                db.JobTypes.Add(jobType);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var jobType = db.JobTypes.SingleOrDefault(c => c.Id == id);
            if (jobType != null)
            {
                db.JobTypes.Remove(jobType);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit(int id)
        {
            var jobType = db.JobTypes.SingleOrDefault(c => c.Id == id);
            if (jobType != null)
            {
                return Json(jobType, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(JobType jobType)
        {
            db.Entry(jobType).State = EntityState.Modified;
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