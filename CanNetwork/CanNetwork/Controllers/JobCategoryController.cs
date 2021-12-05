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
    public class JobCategoryController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Job Categories
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View(db.JobCategories.ToList());
            }
            else
            {
                return RedirectToAction("AdminLogin", "RegisterUser");
            }

        }
        public JsonResult Create(JobCategory jobCategory)
        {
            if (ModelState.IsValid)
            {
                db.JobCategories.Add(jobCategory);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var jobCategory = db.JobCategories.SingleOrDefault(c => c.Id == id);
            if (jobCategory != null)
            {
                db.JobCategories.Remove(jobCategory);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var jobCategory = db.JobCategories.SingleOrDefault(c => c.Id == id);
            if (jobCategory != null)
            {
                return Json(jobCategory, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(JobCategory jobCategory)
        {
            db.Entry(jobCategory).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = 1 });
        }

    }
}