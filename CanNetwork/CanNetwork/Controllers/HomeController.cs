using CanNetwork.Context;
using CanNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanNetwork.Controllers
{
    public class HomeController : Controller
    {
        MyDbContext db = new MyDbContext();
        public ActionResult Index()
        {
            HomeViewModel HVM = new HomeViewModel
            {
                Countries = db.Countries.ToList(),
                Cities = db.Cities.ToList(),
                RegisterUsers = db.RegisterUsers.ToList(),
                Jobs = db.Jobs.ToList()
            };

            if (Convert.ToBoolean(Session["flag"]) == true)
            {
                Session["message"] = "Password Changed Successfully!";
            }

            ViewBag.success = TempData["Message"];
            ViewBag.msg = TempData["msg"];
            TempData.Keep();
            TempData["Message"] = null;
            return View(HVM);
        }

        /************************Manage Contact / Feedbacks*********************/
        // Get Contact Form
        public ActionResult Contact()
        {
            if (Session["AdminUser"] == null)
            {
                return View();
            } else {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var check = db.Contacts.Where(c => c.Name == contact.Name && c.Email == contact.Email).Count();
                if(check <= 0)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    TempData["Message"] = "Thanks For Send Message!";
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "You Send Message Before!";
                }
            }
            return View();
        }

        public ActionResult Feedbacks() {
            if(Session["AdminUser"] != null)
            {   
                var feedbacks = db.Contacts.ToList();
                return View(feedbacks);
            }
            else
            {
                return RedirectToAction("LoginForAll","RegisterUser");
            }
        }
        public JsonResult FeedbackDetails(int id)
        {
            var feedback = db.Contacts.Where(f => f.Id == id).SingleOrDefault();
            return Json(new { result = feedback }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeedbaack(int id)
        {
            var feedback = db.Contacts.Where(f => f.Id == id).SingleOrDefault();
            db.Contacts.Remove(feedback);
            db.SaveChanges();
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}