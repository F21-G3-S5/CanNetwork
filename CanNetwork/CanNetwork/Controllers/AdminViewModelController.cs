using CanNetwork.Context;
using CanNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanNetwork.Controllers
{
    public class AdminViewModelController : Controller
    {
        private MyDbContext db = new MyDbContext();
        // GET: AdminViewModel
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null)
            {
                var Jobs = db.Jobs.ToList();
                var ReportedJobs = db.ReportedJobs.ToList();
                
                var Users = db.RegisterUsers.ToList();
                var Seekers = db.SeekerRegistrations.ToList();

                var JobCategory = db.JobCategories.ToList();
                var Cities = db.Cities.ToList();
                var Languages = db.Languages.ToList();
                var EducationLevels = db.EducationLevels.ToList();
                var Universities = db.Universities.ToList();
                var Countries = db.Countries.ToList();
                var Nationalities = db.Nationalities.ToList();
                var JobTypes = db.JobTypes.ToList();
                var JobTitles = db.JobTitles.ToList();

                var Blogs = db.Blogs.ToList();

                var Contacts = db.Contacts.ToList();

                AdminViewModel AVM = new AdminViewModel();

                AVM.Jobs = Jobs;
                AVM.ReportedJobs = ReportedJobs;

                AVM.RegisterUser = Users;
                AVM.SeekerRegistration = Seekers;
                
                AVM.City = Cities;
                AVM.JobCategory = JobCategory;
                AVM.Languages = Languages;
                AVM.EducationLevels = EducationLevels;
                AVM.Universities = Universities;
                AVM.Countries = Countries;
                AVM.Nationalities = Nationalities;
                AVM.JobTypes = JobTypes;
                AVM.JobTitles = JobTitles;

                AVM.Blogs = Blogs;

                AVM.Contacts = Contacts;

                ViewBag.cityLength = db.Cities.ToList().Count();
                ViewBag.jcLength = db.JobCategories.ToList().Count();
                ViewBag.languageLength = db.Languages.ToList().Count();
                ViewBag.elLength = db.EducationLevels.ToList().Count();
                ViewBag.universitiesLength = db.Universities.ToList().Count();
                ViewBag.countriesLength = db.Countries.ToList().Count();
                ViewBag.nationalitiesLength = db.Nationalities.ToList().Count();
                ViewBag.jobTypesLength = db.JobTypes.ToList().Count();
                ViewBag.jobTitlesLength = db.JobTitles.ToList().Count();


                var newProviders = db.RegisterUsers.Where(ru=>ru.RegistrationDate.Month == DateTime.Now.Month).ToList().Count();
                var newSeekers = db.SeekerRegistrations.Where(s => s.RegistrationDate.Month == DateTime.Now.Month).ToList().Count();
                ViewBag.allNewUsers = newProviders + newSeekers;

                ViewBag.userLength = db.RegisterUsers.ToList().Count();
                ViewBag.seekerLength = db.SeekerRegistrations.ToList().Count();
                ViewBag.allUsers = ViewBag.seekerLength + ViewBag.userLength;
                ViewBag.activeUsers = db.RegisterUsers.Where(ru => ru.IsActive == true).Count() + db.SeekerRegistrations.Where(s => s.IsActive == true).Count();
                ViewBag.blockedUsers = db.RegisterUsers.Where(ru => ru.IsActive == false).Count() + db.SeekerRegistrations.Where(s => s.IsActive == false).Count();

                ViewBag.allJobs = db.Jobs.ToList().Count();
                ViewBag.openJobs = db.Jobs.Where(j=>j.JobState == true).Count();
                ViewBag.closeJobs = db.Jobs.Where(j => j.JobState == false).Count();
                ViewBag.reportedJobs = db.ReportedJobs.ToList().Count();

                ViewBag.blogPosts = db.Blogs.ToList().Count();

                ViewBag.feedbacks = db.Contacts.ToList().Count();

                return View(AVM);
            }
            else {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
 
        public ActionResult ShowAllAdmins()
        {
            if(Session["AdminUser"] != null)
            {
                var currentAdminName = Convert.ToString(Session["AdminUser"]);
                var allAdmins = db.Admins.Where(a => a.Name != currentAdminName).ToList();
                return View(allAdmins);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        [HttpGet]
        public ActionResult AddNewAdmin()
        {
            if (Session["AdminUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        [HttpPost]
        public ActionResult AddNewAdmin(Admin admin)
        {
            if (admin == null)
            {
                return HttpNotFound();
            }
            else
            {
                var newAdminName = admin.Name;
                var check = db.Admins.Any(a => a.Name.Contains(newAdminName));
                if (check)
                {
                    ViewBag.err = "This Name Is Aleady Exist Try Another Name!";
                    return View(admin);
                }
                else
                {
                    db.Admins.Add(admin);
                    db.SaveChanges();

                    return RedirectToAction("ShowAllAdmins");
                }
            }    
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["AdminUser"] != null)
            {
                var admin = db.Admins.Find(id);
                return View(admin);
            } 
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("ShowAllAdmins");
        }
    }
}