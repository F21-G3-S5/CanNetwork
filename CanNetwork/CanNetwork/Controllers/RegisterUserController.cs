using CanNetwork.Context;
using CanNetwork.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CanNetwork.Controllers
{
    public class RegisterUserController : Controller
    {
        MyDbContext db = new MyDbContext();

        /************ Login / forget-reset password ****/
        public ActionResult LoginForAll()
        {
            if (Session["AdminUser"] == null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult LoginForAll(string name, string password)
        {
            var admin = db.Admins.Where(a => a.Name == name && a.Password == password).FirstOrDefault();
            var user = db.RegisterUsers.Where(u => u.Name == name && u.Password == password).FirstOrDefault();
            var seeker = db.SeekerRegistrations.Where(s => s.Name == name && s.Password == password).FirstOrDefault();

            if (admin != null)
            {
                Session["AdminId"] = admin.Id.ToString();
                Session["AdminUser"] = name.ToString();
                return RedirectToAction("Index", "AdminViewModel");
            }
            else if (user != null)
            {
                Session["UserId"] = user.Id.ToString();
                Session["UserName"] = name.ToString();
                return RedirectToAction("Index", "Home");
            }
            else if (seeker != null)
            {
                Session["SeekerId"] = seeker.Id.ToString();
                Session["SeekerUser"] = name.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMsg = "User Name or Password Is Wrong Please Try Again";
            }
            return View();
        }

        public ActionResult ResetPassword()
        {
            if (Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult ResetPassword(string email, string securityQuestionAnswer, string newPassword)
        {
            var user = db.RegisterUsers.Where(ru => ru.Email == email).FirstOrDefault();
            if (user == null)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Email == email).FirstOrDefault();
                if (seeker == null)
                {
                    ViewBag.errMsg = "E-mail Not Found!";
                    return View();
                }
                else
                {
                    if(seeker.SecurityQuestion == securityQuestionAnswer)
                    {
                        seeker.Password = newPassword;
                        db.Entry(seeker).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("LoginForAll");
                    }
                    else
                    {
                        ViewBag.errMsg = "Old Password Not Valid!";
                        return View();
                    }
                }
            }
            else
            {
                if (user.SecurityQuestion == securityQuestionAnswer)
                {
                    user.Password = newPassword;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("LoginForAll");
                }
                else
                {
                    ViewBag.errMsg = "Old Password Not Valid!";
                    return View();
                }
            }
        }
      
        public ActionResult ChangePassword(string name)
        {
            TempData["name"] = name;

            if (Session["UserName"] != null || Session["SeekerUser"] != null && name != null) { 
               return View();
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            string name = TempData["name"].ToString();
            var user = db.RegisterUsers.Where(ru => ru.Name == name).FirstOrDefault();
            if (user == null)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Name == name).FirstOrDefault();
                if (seeker == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    if(seeker.Password == oldPassword)
                    {
                        seeker.Password = newPassword;
                        db.Entry(seeker).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["flag"] = true;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                if (user.Password == oldPassword)
                {
                    user.Password = newPassword;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["message"] = "Password Changed Successfully!";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        /************************ Provider Actions **************************/
        public ActionResult Register()
        {
            if (Session["AdminUser"] == null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Register(RegisterUser registerUser, HttpPostedFileBase providerImage)
        {
            if (ModelState.IsValid)
            {
                if (db.RegisterUsers.Where(ru => ru.Name == registerUser.Name).Count() > 0 || db.SeekerRegistrations.Where(sr => sr.Name == registerUser.Name).Count() > 0 || db.Admins.Where(a => a.Name == registerUser.Name).Count() > 0)
                {
                    ViewBag.Notification = "The Name Is Exist Already Use Other One!";
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                    ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                    return View(registerUser);
                }
                else
                {
                    if (providerImage == null)
                    {
                        registerUser.ProviderImage = "https://via.placeholder.com/150/000000/";
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/ProvidersImage"), providerImage.FileName);
                    providerImage.SaveAs(path);

                    registerUser.ProviderImage = providerImage.FileName;
                    registerUser.RegistrationDate = DateTime.Now;
                    registerUser.IsActive = true;
                    db.RegisterUsers.Add(registerUser);
                    db.SaveChanges();
                    Session["UserId"] = registerUser.Id.ToString();
                    Session["UserName"] = registerUser.Name.ToString();
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                    ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                return View(registerUser);
            }
        }

        public ActionResult DashBoard(int id)
        {
            if(Session["UserName"] != null && Session["UserId"] != null && id > 0)
            {
                var user = db.RegisterUsers.Where(ru => ru.Id == id).SingleOrDefault();
                var publishedJobs = db.Jobs.Where(j => j.RegisterUserId == user.Id).Take(3);
                var matchedSeekers = db.SeekerRegistrations.Where(s => s.SearchOnJobTitle == user.CompanyField).Take(3);

                ProviderViewModel PVM = new ProviderViewModel();

                PVM.Jobs = publishedJobs;
                PVM.SeekerRegistrations = matchedSeekers;

                return View(PVM);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        public ActionResult UserProfile(int id)
        {
            if (Session["UserName"] != null || Session["SeekerUser"] != null)
            {
                var user = db.RegisterUsers.Where(ru => ru.Id == id).FirstOrDefault();
                if (user != null)
                {
                    return View(user);
                }
            }
            else {
                return RedirectToAction("LoginForAll");
            }
            return View();
        }

        public ActionResult EditProfile(int id)
        {
            if (Session["UserName"] != null && Session["UserId"] != null && id > 0)
            {
                var user = db.RegisterUsers.Where(ru => ru.Id == id).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName", user.JobTitle.JobTitleName);
                    ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");

                    return View(user);
                }
            }
            else {
                return RedirectToAction("LoginForAll");
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(RegisterUser registerUser, HttpPostedFileBase photo)
        {
            var user = db.RegisterUsers.Where(ru => ru.Id == registerUser.Id).FirstOrDefault();
            if(registerUser != null)
            {
                if (photo != null) {
                    var oldImage = Path.Combine(Server.MapPath("~/Upload/ProvidersImage"), user.ProviderImage);
                    System.IO.File.Delete(oldImage);
                    string path = Path.Combine(Server.MapPath("~/Upload/ProvidersImage"), photo.FileName);
                    photo.SaveAs(path);
                    user.ProviderImage = photo.FileName;
                }
                user.RegistrationDate = DateTime.Now;
                user.Email = registerUser.Email;
                user.JobTitle = registerUser.JobTitle;
                user.PhoneNumber = registerUser.PhoneNumber;
                user.CompanyName = registerUser.CompanyName;
                user.CompanyField = registerUser.CompanyField;
                user.JobTitleId = registerUser.JobTitleId;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("UserProfile",new { id = registerUser.Id});
            }
            ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName", user.JobTitle.JobTitleName);
            ViewBag.CompanyField = new SelectList(db.JobCategories, "Id", "JobCategoryName");
            return View(registerUser);
        }

        public JsonResult LikeSeeker(int id)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var seeker = db.SeekerRegistrations.Where(s => s.Id == id).SingleOrDefault();

            var check = db.LikedCompanies.Where(lc => lc.RegisterUserId == userId && lc.SeekerRegistrationId == seeker.Id).Count();

            if (userId > 0 && seeker != null && check == 0)
            {
                LikedCompany likedCompany = new LikedCompany();

                likedCompany.RegisterUserId = userId;
                likedCompany.SeekerRegistrationId = seeker.Id;
                likedCompany.LikedCompanyDate = DateTime.Now;

                db.LikedCompanies.Add(likedCompany);
                db.SaveChanges();
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowMatchedSeekers()
        {
            if (Session["UserName"] != null)
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var user = db.RegisterUsers.Where(ru => ru.Id == userId).SingleOrDefault();
                var userCompanyCategory = user.CompanyField;

                var seekers = db.SeekerRegistrations.Where(s => s.SearchOnJobTitle == userCompanyCategory).ToList();
                Session["JobTitle"] = null;
                return View(seekers);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        [HttpPost]
        public ActionResult ShowMatchedSeekers(string searchName)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var user = db.RegisterUsers.Where(ru => ru.Id == userId).SingleOrDefault();
            var userCompanyCategory = user.CompanyField;

            var seekers = db.SeekerRegistrations.Where(s => s.SearchOnJobTitle == userCompanyCategory).ToList();
            Session["JobTitle"] = null;

            if (String.IsNullOrEmpty(searchName))
            {
                return View(seekers);
            }
            else
            {
                var result = seekers.Where(s => s.Name.ToLower().StartsWith(searchName.ToLower())).ToList();
                return View(result);
            }
        }

        /************************ Seeker Actions **************************/
        public ActionResult SeekerRegister()
        {
            if (Session["AdminUser"] == null && Session["UserName"] == null && Session["SeekerUser"] == null)
            {
                //ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
                ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
                ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
                ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult SeekerRegister(SeekerRegistration seekerRegistration, HttpPostedFileBase seekerPhoto, HttpPostedFileBase seekerCv)
        {
            if (ModelState.IsValid)
            {
                if (db.SeekerRegistrations.Any(sr => sr.Name == seekerRegistration.Name) && db.RegisterUsers.Any(sr => sr.Name == seekerRegistration.Name))
                {
                    ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                    ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                    ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
                    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
                    ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
                    ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                    ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                    ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                    return View(seekerRegistration);
                }
                else
                {
                    if (seekerPhoto == null)
                    {
                        seekerRegistration.SeekerImage = "https://via.placeholder.com/150/000000/";
                    }
                    else
                    {
                        string path = Path.Combine(Server.MapPath("~/Upload/SeekersImage"), seekerPhoto.FileName);
                        seekerPhoto.SaveAs(path);
                        seekerRegistration.SeekerImage = seekerPhoto.FileName;

                    }
                    string path2 = Path.Combine(Server.MapPath("~/Upload/SeekersCv"), seekerCv.FileName);
                    seekerCv.SaveAs(path2);
                    seekerRegistration.SeekerCV = seekerCv.FileName;

                    seekerRegistration.RegistrationDate = DateTime.Now;
                    seekerRegistration.IsActive = true;

                    db.SeekerRegistrations.Add(seekerRegistration);
                    db.SaveChanges();
                    
                    Session["SeekerId"] = seekerRegistration.Id.ToString();
                    Session["SeekerUser"] = seekerRegistration.Name.ToString();
                    
                    ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                    ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                    ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
                    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
                    ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
                    ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                    ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                    ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                    
                    return RedirectToAction("SeekerDashBoard", "RegisterUser", new { id = seekerRegistration.Id });
                }
            }
            else
            {
                ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
                ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
                ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
                ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                return View(seekerRegistration);
            }
        }
        
        public ActionResult SeekerDashBoard(int id)
        {
            if (Session["SeekerUser"] != null && Session["SeekerId"] != null && id > 0)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Id == id).SingleOrDefault();
                var applyedJobs = db.ApplyedJobs.Where(aj => aj.SeekerRegistrationId == seeker.Id).Take(3);
                var likedJobs = db.LikedJobs.Where(lj => lj.SeekerRegistrationId == seeker.Id).Take(3);
                var likedCompanies = db.LikedCompanies.Where(lc => lc.SeekerRegistrationId == seeker.Id).Take(3);
                var matchedJobs = db.Jobs.Where(j => j.JobCategory.JobCategoryName == seeker.SearchOnJobTitle).Take(3);

                SeekerViewModel SVM = new SeekerViewModel
                {
                    Jobs = matchedJobs,
                    ApplyedJobs = applyedJobs,
                    LikedJobs = likedJobs,
                    LikedCompanies = likedCompanies
                };

                return View(SVM);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        public ActionResult SeekerProfile(int id)
        {
            var seeker = db.SeekerRegistrations.Where(s => s.Id == id).SingleOrDefault();

            if (Session["SeekerUser"] != null || Session["UserName"] != null && id > 0)
            {
                if (seeker != null)
                {
                    return View(seeker);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        public ActionResult EditSeekerProfile(int id)
        {
            if (Session["SeekerUser"] != null && Session["SeekerId"] != null && id > 0)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Id == id).FirstOrDefault();
                if (seeker != null)
                {
                    ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
                    ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
                    ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
                    ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
                    ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
                    ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
                    ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
                    ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
                    ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
                    return View(seeker);
                }
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditSeekerProfile(SeekerRegistration seekerRegistration, HttpPostedFileBase seekerPhoto, HttpPostedFileBase seekerCv)
        {
            if (seekerRegistration != null)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Id == seekerRegistration.Id).FirstOrDefault();
                if (seekerPhoto != null)
                {
                    var oldImage = Path.Combine(Server.MapPath("~/Upload/SeekersImage"), seeker.SeekerImage);
                    System.IO.File.Delete(oldImage);
                    string path = Path.Combine(Server.MapPath("~/Upload/SeekersImage"), seekerPhoto.FileName);
                    seekerPhoto.SaveAs(path);
                    seeker.SeekerImage = seekerPhoto.FileName;
                }
                if (seekerCv != null)
                {
                    var oldCv = Path.Combine(Server.MapPath("~/Upload/SeekersCv"), seeker.SeekerCV);
                    System.IO.File.Delete(oldCv);
                    string path = Path.Combine(Server.MapPath("~/Upload/SeekersCv"), seekerCv.FileName);
                    seekerCv.SaveAs(path);
                    seeker.SeekerCV = seekerCv.FileName;
                }

                // Personal Info
                seeker.Email = seekerRegistration.Email;
                seeker.Gender = seekerRegistration.Gender;
                seeker.SearchOnJobTitle = seekerRegistration.SearchOnJobTitle;
                seeker.CityId = seekerRegistration.CityId;
                seeker.CountryId = seekerRegistration.CountryId;
                seeker.NationalityId = seekerRegistration.NationalityId;

                // Education Info
                seeker.GraduationDate = seekerRegistration.GraduationDate;
                seeker.LanguageId = seekerRegistration.LanguageId;
                seeker.UniversityId = seekerRegistration.UniversityId;
                seeker.EducationLevelId = seekerRegistration.EducationLevelId;

                // Previous Work Info
                seeker.CompanyName = seekerRegistration.CompanyName;
                seeker.Salary = seekerRegistration.Salary;
                seeker.Duration = seekerRegistration.Duration;
                seeker.JobTitleId = seekerRegistration.JobTitleId;
                seeker.JobTypeId = seekerRegistration.JobTypeId;
                seeker.JobCategoryId = seekerRegistration.JobCategoryId;
                
                // Current Work Info
                seeker.CurrentCompanyName = seekerRegistration.CurrentCompanyName;
                seeker.CurrentSalary = seekerRegistration.CurrentSalary;
                seeker.CurrentDuration = seekerRegistration.CurrentDuration;
                seeker.CurrentJobTitle = seekerRegistration.CurrentJobTitle;
                seeker.CurrentJobType = seekerRegistration.CurrentJobType;
                seeker.CurrentJobCategory = seekerRegistration.CurrentJobCategory;

                db.Entry(seeker).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("SeekerProfile", new { id = seeker.Id });
            }
            //ViewBag.CityId = new SelectList(db.Cities, "Id", "CityName");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "CountryName");
            ViewBag.NationalityId = new SelectList(db.Nationalities, "Id", "NationalityName");
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "LanguageName");
            ViewBag.UniversityId = new SelectList(db.Universities, "Id", "UniversityName");
            ViewBag.EducationLevelId = new SelectList(db.EducationLevels, "Id", "EducationLevelName");
            ViewBag.JobTitleId = new SelectList(db.JobTitles, "Id", "JobTitleName");
            ViewBag.JobTypeId = new SelectList(db.JobTypes, "Id", "JobTypeName");
            ViewBag.JobCategoryId = new SelectList(db.JobCategories, "Id", "JobCategoryName");
            return View(seekerRegistration);
        }

        public ActionResult ShowLikedCompanies()
        {
            if (Session["SeekerUser"] != null)
            {
                var seekerId = Convert.ToInt32(Session["SeekerId"]);
                var likedCompanies = db.LikedCompanies.Where(lc => lc.SeekerRegistrationId == seekerId).ToList();
                return View(likedCompanies);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        /*************************** Admin ******************/
        public ActionResult NewUsersInThisMonth()
        {
            if(Session["AdminUser"] != null)
            {
                var allNewSeekers = db.SeekerRegistrations.Where(s=>s.RegistrationDate.Month == DateTime.Now.Month).ToList();
                var allNewProviders = db.RegisterUsers.Where(ru => ru.RegistrationDate.Month == DateTime.Now.Month).ToList();
                SeekerProviderViewModel SPVModel = new SeekerProviderViewModel
                {
                    RegisterUsers = allNewProviders,
                    SeekerRegistrations = allNewSeekers
                };
                return View(SPVModel);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        
        public ActionResult AllSeekersAndProviders()
        {
            if(Session["AdminUser"] != null)
            {
                var allSeekers = db.SeekerRegistrations.ToList().Take(3);
                var allProviders = db.RegisterUsers.ToList().Take(3);

                SeekerProviderViewModel SPVM = new SeekerProviderViewModel
                {
                    SeekerRegistrations = allSeekers,
                    RegisterUsers = allProviders
                };

                return View(SPVM);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        public ActionResult AllActiveUsers()
        {
            if (Session["AdminUser"] != null)
            {
                var allActiveSeekers = db.SeekerRegistrations.Where(s=>s.IsActive == true).ToList().Take(3);
                var allActiveProviders = db.RegisterUsers.Where(ru => ru.IsActive == true).ToList().Take(3);

                SeekerProviderViewModel SPVM = new SeekerProviderViewModel
                {
                    SeekerRegistrations = allActiveSeekers,
                    RegisterUsers = allActiveProviders
                };

                return View(SPVM);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        public ActionResult AllActiveProvidersDetails()
        {

            if (Session["AdminUser"] != null)
            {
                var allActiveProviders = db.RegisterUsers.Where(ru => ru.IsActive == true).ToList();

                return View(allActiveProviders);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        public ActionResult AllActiveSeekersDetails()
        {

            if (Session["AdminUser"] != null)
            {
                var allActiveSeekers = db.SeekerRegistrations.Where(ru => ru.IsActive == true).ToList();

                return View(allActiveSeekers);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        public ActionResult AllBlockUsers()
        {
            if (Session["AdminUser"] != null)
            {
                var allBlockSeekers = db.SeekerRegistrations.Where(s => s.IsActive == false).ToList();
                var allBlockedProviders = db.RegisterUsers.Where(ru => ru.IsActive == false).ToList();

                SeekerProviderViewModel SPVM = new SeekerProviderViewModel
                {
                    SeekerRegistrations = allBlockSeekers,
                    RegisterUsers = allBlockedProviders
                };

                return View(SPVM);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        public ActionResult AllBlockedProvidersDetails()
        {

            if (Session["AdminUser"] != null)
            {
                var allBlockedProviders = db.RegisterUsers.Where(ru => ru.IsActive == false).ToList();

                return View(allBlockedProviders);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }
        public ActionResult AllBlockedSeekersDetails()
        {
            if (Session["AdminUser"] != null)
            {
                var allBlockedSeekers = db.SeekerRegistrations.Where(s => s.IsActive == false).ToList();

                return View(allBlockedSeekers);
            }
            else
            {
                return RedirectToAction("LoginForAll");
            }
        }

        public ActionResult ShowAllUsers()
        {
            if (Session["AdminUser"] != null)
            {
                return View(db.RegisterUsers.ToList());
            }
            return RedirectToAction("LoginForAll");
        }
        public ActionResult UserDetails(int id)
        {
            if (Session["AdminUser"] != null)
            {
                var user = db.RegisterUsers.Where(u => u.Id == id).FirstOrDefault();
                return View(user);
            }
            return RedirectToAction("LoginForAll");
        }

        [HttpGet]
        public JsonResult EditProvider(int id)
        {
            var provider = db.RegisterUsers.Where(s => s.Id == id).SingleOrDefault();
            return Json(new { id = provider.Id, isActive = provider.IsActive }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditProvider([Bind(Include = "Id, IsActive")] RegisterUser registerUser)
        {
            var editProvider = db.RegisterUsers.Where(s => s.Id == registerUser.Id).FirstOrDefault();

            editProvider.IsActive = registerUser.IsActive;

            db.Entry(editProvider).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = 1 });
        }

        public ActionResult ShowAllSeekers()
        {
            if (Session["AdminUser"] != null)
            {
                return View(db.SeekerRegistrations.ToList());
            }
            return RedirectToAction("LoginForAll");
        }
        public ActionResult SeekerDetails(int id)
        {
            if (Session["AdminUser"] != null || Session["UserName"] != null)
            {
                var seeker = db.SeekerRegistrations.Where(s => s.Id == id).FirstOrDefault();
                ViewBag.IsActive = seeker.IsActive;
                return View(seeker);
            }
            return RedirectToAction("LoginForAll");
        }

        [HttpGet]
        public JsonResult EditSeeker(int id)
        {
            var seeker = db.SeekerRegistrations.Where(s => s.Id == id).SingleOrDefault();
            return Json(seeker, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditSeeker(SeekerRegistration seeker)
        {
            var editSeeker = db.SeekerRegistrations.Where(s => s.Id == seeker.Id).FirstOrDefault();

            editSeeker.IsActive = seeker.IsActive;

            db.Entry(editSeeker).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = 1, seeker = editSeeker });
        }

        /*************************** LogOut For All ***************/
        public ActionResult LogOut()
        {
            if (Session["SeekerUser"] != null || Session["AdminUser"] != null || Session["UserName"] != null)
            {
                Session.Clear();
                return RedirectToAction("LoginForAll");
            }
            return View();
        }
    }
}