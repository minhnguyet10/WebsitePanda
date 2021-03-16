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
    public class BannerController : ManagementController
    {
        private DB_Optical db = new DB_Optical();

        // GET: Banner
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }

        // GET: Banner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Banner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBanner,Image,DisplayOrder,Link,Description,CreatedDate")] Banner banner, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        banner.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                banner.CreatedDate = DateTime.Now;

                db.Banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Banner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Banner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBanner,Image,DisplayOrder,Link,Description,CreatedDate")] Banner banner, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        banner.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                banner.CreatedDate = DateTime.Now;

                db.Entry(banner).State = EntityState.Modified;
                if (Image == null)
                    db.Entry(banner).Property(m => m.Image).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        // GET: Banner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
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
