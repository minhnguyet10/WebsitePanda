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
    public class ProductController : ManagementController
    {
        private DB_Optical db = new DB_Optical();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "IDProduct,ProductName,Title,Description,Information,Image,MoreImage1,MoreImage2,Price,PromotionPrice,Quantity,Color,CategoryID,BrandID,CreatedDate,Status")] Product product, HttpPostedFileBase Image, HttpPostedFileBase MoreImage1, HttpPostedFileBase MoreImage2)
        {
            if (ModelState.IsValid)
            {

                if (Image != null && Image.ContentLength > 0)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        product.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                if (MoreImage1 != null && MoreImage1.ContentLength > 0)
                    try
                    {
                        var fileName1 = Path.GetFileName(MoreImage1.FileName);
                        product.MoreImage1 = fileName1;
                        var path1 = Path.Combine(Server.MapPath("~/Images"), fileName1);
                        Image.SaveAs(path1);
                    }
                    catch
                    {

                    }
                if (MoreImage2 != null && MoreImage2.ContentLength > 0)
                    try
                    {
                        var fileName2 = Path.GetFileName(MoreImage2.FileName);
                        product.MoreImage2 = fileName2;
                        var path2 = Path.Combine(Server.MapPath("~/Images"), fileName2);
                        Image.SaveAs(path2);
                    }
                    catch
                    {

                    }
               
                product.CreatedDate = DateTime.Now;

                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDProduct,ProductName,Title,Description,Information,Image,MoreImage1,MoreImage2,MoreImage3,Price,PromotionPrice,Quantity,Color,CategoryID,BrandID,Detail,CreatedDate,Status")] Product product, HttpPostedFileBase Image, HttpPostedFileBase MoreImage1, HttpPostedFileBase MoreImage2)
        {
            if (ModelState.IsValid)
            {
                //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                if (product.Image != null)
                    try
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        product.Image = fileName;
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        Image.SaveAs(path);
                    }
                    catch
                    {

                    }
                if (MoreImage1 != null && MoreImage1.ContentLength > 0)
                    try
                    {
                        var fileName1 = Path.GetFileName(MoreImage1.FileName);
                        product.MoreImage1 = fileName1;
                        var path1 = Path.Combine(Server.MapPath("~/Images"), fileName1);
                        Image.SaveAs(path1);
                    }
                    catch
                    {

                    }
                if (MoreImage2 != null && MoreImage2.ContentLength > 0)
                    try
                    {
                        var fileName2 = Path.GetFileName(MoreImage2.FileName);
                        product.MoreImage2 = fileName2;
                        var path2 = Path.Combine(Server.MapPath("~/Images"), fileName2);
                        Image.SaveAs(path2);
                    }
                    catch
                    {

                    }               
                product.CreatedDate = DateTime.Now;

                db.Entry(product).State = EntityState.Modified;
                if (Image == null)
                    db.Entry(product).Property(m => m.Image).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
