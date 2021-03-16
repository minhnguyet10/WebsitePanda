using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;
using System.Data.Entity;
using System.Configuration;
using Website_Panda.Models.Account;

namespace Website_Panda.Controllers
{
    public class HomeController : Controller
    {
        DB_Optical db = new DB_Optical();
        // GET: Home
        public ActionResult Index()
        {
            //SEO 
            ViewBag.Title = ConfigurationManager.AppSettings["HomeTitle"];
            ViewBag.Keywords = ConfigurationManager.AppSettings["HomeKeyword"];
            ViewBag.Descriptions = ConfigurationManager.AppSettings["HomeDescription"];
            return View();
        }
        public ActionResult Menu()
        {
            var cate = db.Categories;
            var brand = db.Brands;
            ViewBag.cate = cate;
            ViewBag.brand = brand;
            return View();
        }
        public ActionResult CategoryList()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult BrandList()
        {
            return View(db.Brands.ToList());
        }
        public ActionResult Product(long? id)
        {
            var result = from r in db.Products
                         where r.CategoryID == id
                         select r;
            return PartialView("Product", result);
        }
        public ActionResult AllProduct(int? page, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var result = db.Products.Where(s => s.ProductName.Contains(search) || s.Category.CategoryName.Contains(search) || s.Brand.BrandName.Contains(search));
                return View(result.ToList().ToPagedList(page ?? 1, 9));
            }
            return View(db.Products.ToList().ToPagedList(page ?? 1, 9));
            //return PartialView("AllProduct", db.Products.ToList().ToPagedList(page ?? 1, 9));
        }

        public ActionResult ProductList(long? id, int? page)
        {
            var result = from r in db.Products
                         where r.CategoryID == id
                         select r;
            return PartialView("ProductList", result.ToList().ToPagedList(page ?? 1, 9));
        }
        public ActionResult ProductBrandList(long? id, int? page)
        {
            var result = from r in db.Products
                         where r.BrandID == id
                         select r;
            return PartialView("ProductBrandList", result.ToList().ToPagedList(page ?? 1, 9));
        }
        public ActionResult MoreProduct(long? id)
        {
            var result = from r in db.Products
                         where r.CategoryID == id
                         select r;
            return PartialView("MoreProduct", result);
        }

        public ActionResult ProductDetail(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            //if (pro != null)
            //{
            //    GetViewHistory().Add(pro);
            //}
            return View(product);
        }
        public ActionResult Banner()
        {
            return View(db.Banners.ToList());
        }
      
        public ActionResult NewProduct()
        {
            return View(db.Products.OrderByDescending(m => m.CreatedDate).Take(4).ToList());
        }
        public ActionResult HotProduct()
        {
            var result = from r in db.Products
                         where r.PromotionPrice != null
                         select r;
            return PartialView("HotProduct", result);            
        }
        
        public ActionResult PandaContact()
        {
            return View(db.Contacts.FirstOrDefault());
        }
        public ActionResult Message()
        { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Message([Bind(Include = "IDMessage,Name,Email,Subject, MessageContact,Createday")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.Createday = DateTime.Now;
                db.Messages.Add(message);
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMessage.html"));
                content = content.Replace("{{Name}}", message.Name.ToString());
                content = content.Replace("{{Email}}", message.Email.ToString());
                content = content.Replace("{{MessageContact}}", message.MessageContact.ToString());
                var toMail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new Mail().SendMail(toMail, "Bạn có phản hồi mới từ Panda", content);
                db.SaveChanges();
                return RedirectToAction("SentMessage");
            }
            
            return View(message);
        }
        public ActionResult SentMessage()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult BlogList(int? id, int? page)
        {
            return View(db.Blogs.ToList().ToPagedList(page ?? 1, 2));
        }
        public ActionResult BlogDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            var blg = db.Blogs.SingleOrDefault(s => s.IDBlog == id);
            return View(blg);
        }
        public ActionResult MoreBlog()
        {
            return View(db.Blogs.ToList());
        }
    }
}