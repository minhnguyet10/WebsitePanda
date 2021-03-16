using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;

namespace Website_Panda.Controllers
{
    public class BlogController : ManagementController
    {
        private DB_Optical db = new DB_Optical();
        // GET: Blog
        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        // GET: Blog/Details/5
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

        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBlog, Image, Title, Subtitle, Content, CreatedDate")] Blog blog, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        blog.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                blog.CreatedDate = DateTime.Now;

                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBlog, Image, Title, Subtitle, Content, CreatedDate")] Blog blog, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        blog.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                blog.CreatedDate = DateTime.Now;

                db.Entry(blog).State = EntityState.Modified;
                if (Image == null)
                    db.Entry(blog).Property(m => m.Image).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Blog/Delete/5
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
