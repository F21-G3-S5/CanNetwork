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
    public class JobController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Job
        public ActionResult Index()
        {
            if (Session["SeekerUser"] != null)
            {
                var seekerName = Session["SeekerUser"].ToString();
                var seekerState = db.SeekerRegistrations.Where(s => s.Name == seekerName).FirstOrDefault();
                ViewBag.SeekerState = seekerState.IsActive;
            }

            if (Session["UserName"] != null)
            {
                var name = Session["UserName"].ToString();
                var userState = db.RegisterUsers.Where(s => s.Name == name).FirstOrDefault();
                ViewBag.UserState = userState.IsActive;
            }

            var jobs = db.Jobs.Include(j => j.City).Include(j => j.JobCategory);
            ViewBag.JobsCount = jobs.Count();
            return View(jobs.ToList());
        }

        // Search: job
        [HttpPost]
        public ActionResult Index(string searchName, string searchVector)
        {
            var jobs = db.Jobs.Include(j => j.City);

            if (String.IsNullOrEmpty(searchName))
            {
                ViewBag.JobsCount = jobs.Count();
                return View(jobs.ToList());
            }
            else
            {
                if (searchVector == "JobTitle")
                {
                    var matchedJobs = jobs.Where(j => j.JobTitle.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
                if (searchVector == "JobCategory.JobCategoryName")
                {
                    var matchedJobs = jobs.Where(j => j.JobCategory.JobCategoryName.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
                if (searchVector == "JobType.JobTypeName")
                {
                    var matchedJobs = jobs.Where(j => j.JobType.JobTypeName.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
                if (searchVector == "City.CityName")
                {
                    var matchedJobs = jobs.Where(j => j.City.CityName.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
                if (searchVector == "Country.CountryName")
                {
                    var matchedJobs = jobs.Where(j => j.JobTitle.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
                if (searchVector == "CareerLevel")
                {
                    var matchedJobs = jobs.Where(j => j.CareerLevel.ToLower().StartsWith(searchName.ToLower())).ToList();
                    ViewBag.JobsCount = matchedJobs.Count();
                    return View(matchedJobs);
                }
            }
            return View();
        }


        // GET: Job/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            if (Session["SeekerUser"] != null)
            {
                var seekerName = Session["SeekerUser"].ToString();
                var seekerState = db.SeekerRegistrations.Where(s => s.Name == seekerName).FirstOrDefault();
                ViewBag.SeekerState = seekerState.IsActive;
            }
            return View(job);
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null && Session["UserId"] != null)
            {
                //ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                return View();
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
                job.RegisterUserId = userId;
                job.JobState = true;
                job.PublishedDate = DateTime.Now;
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("ShowPublishedJobs");
            }

            //ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
            return View(job);
        }

        public JsonResult GetCitiesInCountry(int CountryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<City> cities = db.Cities.Where(c => c.CountryId == CountryId).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null && Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Jobs.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName", job.CityId);
                ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName", job.CountryId);
                ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName", job.JobTypeId);
                ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName", job.JobCategoryId);
                return View(job);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Job job)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
                job.RegisterUserId = userId;
                job.PublishedDate = DateTime.Now;

                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowPublishedJobs");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName", job.CityId);
            ViewBag.CountyId = new SelectList(db.Countries, "Id", "CountryName", job.CountryId);
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName", job.JobTypeId);
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName", job.JobCategoryId);
            return View(job);
        }

        // GET: Job/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Jobs.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                return View(job);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("ShowPublishedJobs");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetJobInCity (int id)
        {
            var jobs = db.Jobs.Where(j => j.CityId == id).ToList();
            var selectedCity = db.Cities.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.CityName = selectedCity.CityName;
            ViewBag.JobsCount = jobs.Count();
            return View(jobs);
        }

        public ActionResult GetJobInCompany(int id)
        {
            var jobs = db.Jobs.Where(j => j.RegisterUserId == id).ToList();
            var selectedCompany = db.RegisterUsers.Where(ru => ru.Id == id).FirstOrDefault();
            ViewBag.CompanyName = selectedCompany.CompanyName;
            ViewBag.JobsCount = jobs.Count();
            return View(jobs);
        }

        /************ Seeker ***************/
        public JsonResult ApplyJob(int id)
        {
            if (Session["SeekerId"] != null && Session["SeekerUser"] != null)
            {
                var SeekerId = Convert.ToInt32(Session["SeekerId"]);
                var JobId = id;

                var seeker = db.SeekerRegistrations.Find(SeekerId);
                var job = db.Jobs.Find(JobId);

                if(seeker.IsActive == false)
                {
                    return Json(new { res = "Blocked" }, JsonRequestBehavior.AllowGet);
                }


                if(job.JobState == false)
                {
                    return Json(new { res = "Closed" }, JsonRequestBehavior.AllowGet);
                }

                var dateTime = DateTime.Now;

                var applyJobCheck = db.ApplyedJobs.Where(a => a.JobId == JobId && a.SeekerRegistrationId == SeekerId).Count();

                if (applyJobCheck != 0)
                {
                    return Json(new { res = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ApplyedJob ApplyedJob = new ApplyedJob();

                    ApplyedJob.JobId = JobId;
                    ApplyedJob.SeekerRegistrationId = SeekerId;
                    ApplyedJob.ApplyDate = dateTime;

                    db.ApplyedJobs.Add(ApplyedJob);
                    db.SaveChanges();
                    return Json(new { res = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { res = "unauthorized" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowApplyedJobs(int? id){
            if (Session["SeekerId"] != null || Session["AdminUser"] != null || Session["UserName"] != null) { 
                var seekerId = Convert.ToInt32(Session["SeekerId"]);
                var applyedJobs = db.ApplyedJobs.Where(aj => aj.SeekerRegistrationId == seekerId || aj.SeekerRegistrationId == id).ToList();
                return View(applyedJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        public JsonResult DeleteApplyedJob(int id)
        {
            var ApplyJob = db.ApplyedJobs.Where(a => a.Id == id).FirstOrDefault();
            if (ApplyJob != null)
            {
                db.ApplyedJobs.Remove(ApplyJob);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LikedJob(int id) {
            if (Session["SeekerId"] != null && Session["SeekerUser"] != null)
            {
                var SeekerId = Convert.ToInt32(Session["SeekerId"]);
                var JobId = id;

                var dateTime = DateTime.Now;

                var likedJobCheck = db.LikedJobs.Where(lj => lj.JobId == JobId && lj.SeekerRegistrationId == SeekerId).Count();

                if (likedJobCheck != 0)
                {
                    return Json(new { result = 2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LikedJobs likedJob = new LikedJobs();

                    likedJob.JobId = JobId;
                    likedJob.SeekerRegistrationId = SeekerId;
                    likedJob.LikedDate = dateTime;

                    db.LikedJobs.Add(likedJob);
                    db.SaveChanges();
                    return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowLikedJobs()
        {
            if(Session["SeekerUser"] != null)
            {
                var seekerId = Convert.ToInt32(Session["SeekerId"]);
                var likedJobs = db.LikedJobs.Where(lj => lj.SeekerRegistrationId == seekerId).ToList();
                return View(likedJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        public JsonResult DeleteLikedJob(int id)
        {
            if(id > 0)
            {
                var likedJob = db.LikedJobs.Where(lj => lj.Id == id).SingleOrDefault();
                db.LikedJobs.Remove(likedJob);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReportedJob(int id)
        {
            if (Session["SeekerId"] != null && Session["SeekerUser"] != null)
            {
                var SeekerId = Convert.ToInt32(Session["SeekerId"]);
                var JobId = id;

                var dateTime = DateTime.Now;

                var reportedJobCheck = db.ReportedJobs.Where(rj => rj.JobId == JobId && rj.SeekerRegistrationId == SeekerId).Count();

                if (reportedJobCheck != 0)
                {
                    return Json(new { result = 2 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ReportedJob reportedJob = new ReportedJob();

                    reportedJob.JobId = JobId;
                    reportedJob.SeekerRegistrationId = SeekerId;
                    reportedJob.ReportedDate = dateTime;

                    db.ReportedJobs.Add(reportedJob);
                    db.SaveChanges();
                    return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowMatchedJobs(int? id)
        {
            if(Session["SeekerUser"] != null || Session["AdminUser"] != null || Session["UserName"] != null)
            {
                var seekerId = Convert.ToInt32(Session["SeekerId"]);
                var seeker = db.SeekerRegistrations.Where(s => s.Id == seekerId || s.Id == id).FirstOrDefault();
                var seekerJobTitle = seeker.SearchOnJobTitle;
                var matchedJobs = db.Jobs.Where(j => j.JobCategory.JobCategoryName == seekerJobTitle).ToList();

                return View(matchedJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        [HttpPost]
        public ActionResult ShowMatchedJobs(string searchName)
        {
            var seekerId = Convert.ToInt32(Session["SeekerId"]);
            var seeker = db.SeekerRegistrations.Where(s => s.Id == seekerId).SingleOrDefault();
            var seekerCompanyCategory = seeker.SearchOnJobTitle;

            var jobs = db.Jobs.Where(j => j.JobCategory.JobCategoryName == seekerCompanyCategory).ToList();

            if (String.IsNullOrEmpty(searchName))
            {
                return View(jobs);
            }
            else
            {
                var result = jobs.Where(s => s.JobTitle.ToLower().StartsWith(searchName.ToLower())).ToList();
                return View(result);
            }
        }

        /************ Provider ************/
        public ActionResult ShowPublishedJobs()
        {
            if (Session["UserId"] != null)
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var jobs = db.Jobs.Where(j => j.RegisterUserId == userId).ToList();
                return View(jobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        public ActionResult ShowApplyersForEachJob()
        {
            if(Session["UserName"] != null && Session["UserId"] != null)
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var Jobs = from app in db.ApplyedJobs
                           join job in db.Jobs
                           on app.JobId equals job.Id
                           where job.RegisterUserId == userId
                           select app;

                var groups = from j in Jobs
                             group j by j.Job.JobTitle
                             into gr
                             select new JobViewModel
                             {
                                 JobTitle = gr.Key,
                                 ApplyedJobs = gr
                             };

                return View(groups.ToList());
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        public ActionResult JobApplyersDetails(string jobTitle) 
        {
            if (Session["UserName"] != null && Session["UserId"] != null)
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var Jobs = from app in db.ApplyedJobs
                           join job in db.Jobs
                           on app.JobId equals job.Id
                           where job.RegisterUserId == userId
                           select app;

                var groups = from j in Jobs
                             group j by j.Job.JobTitle
                             into gr
                             select new JobViewModel
                             {
                                 JobTitle = gr.Key,
                                 ApplyedJobs = gr
                             };

                var group = groups.Where(g => g.JobTitle == jobTitle).SingleOrDefault();

                Session["JobTitle"] = jobTitle.ToString();

                return View(group);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        /************ Admin ************/
        public ActionResult AllOpenJobs()
        {
            if(Session["AdminUser"] != null)
            {
                var openJobs = db.Jobs.Where(j => j.JobState == true).ToList();
                return View(openJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public ActionResult AllCloseJobs()
        {
            if (Session["AdminUser"] != null)
            {
                var closeJobs = db.Jobs.Where(j => j.JobState == false).ToList();
                return View(closeJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public ActionResult ShowReportedJobs()
        {
            if(Session["AdminUser"] != null)
            {
                var reportedJobs = db.ReportedJobs.ToList();
                return View(reportedJobs);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        public JsonResult DeleteReportedJob(int id)
        {
            if (id > 0)
            {
                var reportedJob = db.ReportedJobs.Where(rj => rj.Id == id).SingleOrDefault();
                db.ReportedJobs.Remove(reportedJob);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditJobState(int id)
        {
            Job job = db.Jobs.Where(j => j.Id == id).FirstOrDefault();
            if(job != null)
            {
                return Json(new { id = job.Id, jobState = job.JobState }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { val = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditJobState([Bind(Include = "Id, JobState")] Job job)
        {
            var editJob = db.Jobs.Where(s => s.Id == job.Id).FirstOrDefault();

            editJob.JobState = job.JobState;

            db.Entry(editJob).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = 1 });
        }
    }
}
