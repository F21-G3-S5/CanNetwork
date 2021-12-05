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
    public class BlogsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Blogs
        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
       
        }
        
        [HttpPost]
        public ActionResult Index(string searchName)
        {
            var blogs = db.Blogs;

            if (String.IsNullOrEmpty(searchName))
            {
                return View(blogs.ToList());
            }
            else
            {
                var matchedBlogs = blogs.Where(b => b.PostTitle.ToLower().StartsWith(searchName.ToLower())).ToList();

                return View(matchedBlogs);
            }
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            if(Session["AdminUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                var fileName = System.IO.Path.GetFileName(photo.FileName);
                var path = System.IO.Path.Combine(Server.MapPath("~/Upload/PostsImage"), fileName);
                photo.SaveAs(path);
                
                blog.PostImage = fileName;
                blog.PublishDate = DateTime.Now;

                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["AdminUser"] != null){
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Blog blog = db.Blogs.Find(id);
                if (blog == null)
                {
                    return HttpNotFound();
                }
                return View(blog);
            }
            return RedirectToAction("LoginForAll", "RegisterUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog, HttpPostedFileBase editPhoto)
        {
            if (ModelState.IsValid)
            {
                var oldImage = System.IO.Path.Combine(Server.MapPath("~/Upload/PostsImage"), blog.PostImage);
                if (editPhoto != null)
                {
                    System.IO.File.Delete(oldImage);
                    var fileName = System.IO.Path.GetFileName(editPhoto.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Upload/PostsImage"), fileName);
                    editPhoto.SaveAs(path);
                    blog.PostImage = fileName;
                }
                blog.PublishDate = DateTime.Now;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminUser"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Blog blog = db.Blogs.Find(id);
                if (blog == null)
                {
                    return HttpNotFound();
                }
                return View(blog);
            }
            else
            {
                return RedirectToAction("LoginForAll", "RegisterUser");
            }
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
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
