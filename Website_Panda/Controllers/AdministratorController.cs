using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;

namespace Website_Panda.Controllers
{
    public class AdministratorController : ManagementController
    {
        private DB_Optical db = new DB_Optical();

        // GET: CustomerAccount
        public ActionResult IndexCustomerAccount()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult DeleteCus(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer cus = db.Customers.Find(id);
            if (cus == null)
            {
                return HttpNotFound();
            }
            return View(cus);
        }
        // POST: Administrator/Delete/5
        [HttpPost, ActionName("DeleteCus")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCus(long id)
        {
            Customer cus = db.Customers.Find(id); 
            db.Customers.Remove(cus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DetailCus(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        // GET: Administrator
        public ActionResult Index()
        {
            return View(db.Administrators.ToList());
        }

        // GET: Administrator/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Message
        public ActionResult IndexMessage()
        {
            return View(db.Messages.ToList());
        }
        public ActionResult DetailMessage(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message mess = db.Messages.Find(id);
            if (mess == null)
            {
                return HttpNotFound();
            }
            return View(mess);
        }
    }
}
