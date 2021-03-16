using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.Account;
using Website_Panda.Models.DAL;
using Website_Panda.Models.Login;
using Website_Panda.Models.OrderModel;

namespace Website_Panda.Controllers
{
    public class CustomerAccountController : Controller
    {
        private DB_Optical db = new DB_Optical();
        public ActionResult ForgotPassword()
        { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Include = "Id,Username,Email,Name")] ForgotPassword fp)
        {
            if (ModelState.IsValid)
            {
                var check = db.Customers.Where(s => s.UserName.Equals(fp.Username) && s.Email_Cus.Equals(fp.Email)).FirstOrDefault(); 
                if(check ==null)
                {
                    ViewBag.message = "Username or Email invalid!";  
                }
                else {
                var result = from r in db.Customers
                                where r.UserName == fp.Username && r.Email_Cus==fp.Email
                                select r;
                var infoCus = result.ToList().First();
                fp.Password = infoCus.Password;
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendPassword.html"));
                content = content.Replace("{{Name}}", fp.Name.ToString());
                content = content.Replace("{{Email}}", fp.Email.ToString());
                content = content.Replace("{{Password}}", fp.Password.ToString());
                var toMail = fp.Email;
                new Mail().SendMail(toMail, "Forgot Password?", content);
                db.SaveChanges();
                return RedirectToAction("LoginCus");}
            }

            return View(fp);
        }
        [HttpGet]
        public ActionResult LoginCus()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCus(Customer cus)
        {
            var check = db.Customers.Where(s => s.UserName.Equals(cus.UserName)
            && s.Password.Equals(cus.Password)).FirstOrDefault();
            if (check == null)
            {
                ViewBag.LoginError = "Username or Password invalid!";
            }
            else
            {
                try
                {
                    var result = from r in db.Customers
                                 where r.UserName == cus.UserName
                                 select r;
                    var cusTemp = result.ToList().First();
                    var userSession = new UserLogin();
                    userSession.UserName = cusTemp.LastName + " " + cusTemp.FirstName;
                    userSession.UserID = cusTemp.IdCus;
                    Session.Add(Common.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ViewBag.LoginError = "Username or Password invalid!";
                }

            }
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        // GET: CustomerAccount
        //public ActionResult Index()
        //{
        //    return View(db.Customers.ToList());
        //}

        // GET: CustomerAccount/Details/5
        public ActionResult Details(long? id)
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

        // GET: CustomerAccount/Create
        public ActionResult CreateAccount()
        {
            return View();
        }

        // POST: CustomerAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(Customer cus)
        {

            var check = db.Customers.FirstOrDefault(s => s.UserName == cus.UserName);
            var checkEmail = db.Customers.FirstOrDefault(s => s.Email_Cus == cus.Email_Cus);
            if (check == null)
            {
                if (cus.UserName != null || cus.Password !=null || cus.ConfirmPassword !=null)
                {
                    if(cus.Email_Cus != null)
                    { 
                        if(checkEmail==null)
                        {
                            cus.CreatedDay = DateTime.Now;
                            cus.CustomerRank = "Khách hàng bậc đồng";
                            db.Customers.Add(cus);
                            db.SaveChanges();
                            return RedirectToAction("LoginCus");
                        }
                        else { ViewBag.CheckUser = "Email is existed!"; }
                    }
                    //cus.CreatedDay = DateTime.Now;
                    //db.Customers.Add(cus);
                    //db.SaveChanges();
                    return View();
                }
                else
                {
                    ViewBag.CheckUser = "UserName or Password or ConfirmPassword is not null!";
                }
            }
            else
            {
                ViewBag.CheckUser = "Username is existed!";
            }
            return View();
        }

        // GET: CustomerAccount/Edit/5
        public ActionResult Edit(long? id)
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

        // POST: CustomerAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCus, FirstName, LastName, Email_Cus, Address_Cus, Phone_Cus")]Customer customer)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(customer).State = EntityState.Modified;
                if (customer.CreatedDay == null)
                    db.Entry(customer).Property(m => m.CreatedDay).IsModified = false;
                if (customer.UserName == null)
                    db.Entry(customer).Property(m => m.UserName).IsModified = false;
                if (customer.Password == null)
                    db.Entry(customer).Property(m => m.Password).IsModified = false;
                if (customer.ConfirmPassword == null)
                    db.Entry(customer).Property(m => m.ConfirmPassword).IsModified = false;
                if (customer.Score == null) { 
                    db.Entry(customer).Property(m => m.Score).IsModified = false;
                }
                if (customer.CustomerRank == null)
                {
                    db.Entry(customer).Property(m => m.CustomerRank).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("/Details/" + customer.IdCus);
            }
            return View(customer);
        }

        // GET: CustomerAccount/Edit/5
        public ActionResult ChangePassword(long? id)
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

        // POST: CustomerAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "IdCus, Password, ConfirmPassword,FirstName, LastName, Email_Cus, Address_Cus, Phone_Cus")]Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                if (customer.CreatedDay == null)
                    db.Entry(customer).Property(m => m.CreatedDay).IsModified = false;
                if (customer.UserName == null)
                    db.Entry(customer).Property(m => m.UserName).IsModified = false;
                if (customer.FirstName == null)
                    db.Entry(customer).Property(m => m.FirstName).IsModified = false;
                if (customer.LastName == null)
                    db.Entry(customer).Property(m => m.LastName).IsModified = false; 
                if (customer.Email_Cus == null)
                    db.Entry(customer).Property(m => m.Email_Cus).IsModified = false;
                if (customer.Address_Cus == null)
                    db.Entry(customer).Property(m => m.Address_Cus).IsModified = false;
                if (customer.Phone_Cus == null)
                    db.Entry(customer).Property(m => m.Phone_Cus).IsModified = false;
                if (customer.Score == null)
                {
                    db.Entry(customer).Property(m => m.Score).IsModified = false;
                }
                if (customer.CustomerRank == null)
                {
                    db.Entry(customer).Property(m => m.CustomerRank).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("/Details/" + customer.IdCus);
            }
            return View(customer);
        }
        //Lịch sử đặt hàng
        public ActionResult OrderHistory(int id)
        {
            return PartialView(db.OrderDetails.Where(s => s.Order.IdCus == id).ToList());
        }
        //Chi tiết đơn hàng
        [HttpGet]
        public ActionResult OrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _Order order = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.OrderDetails.Where(n => n.IDOrder == id);
            ViewBag.ListOrderDetail = lstChiTietDH;
            return View(order);
        }

        /// <param name="ctdh"></param>
        [HttpPost]
        public ActionResult OrderDetail(_Order ctdh)
        {
            //var session = (UserLogin)Session[Common.USER_SESSION];
            _Order ddhUpdate = db.Orders.Where(n => n.IDOrder.Equals(ctdh.IDOrder)).FirstOrDefault();
            if (ddhUpdate != null)
            {
                var lstChiTietDH = db.OrderDetails.Where(n => n.IDOrder == ctdh.IDOrder);
                ViewBag.ListOrderDetail = lstChiTietDH;
                return View(ddhUpdate);
            }
            else
            {
                ViewBag.Message = "Choose receipt";
            }
            return View(ddhUpdate);
        }

        // GET: CustomerAccount/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: CustomerAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Quản lý đơn hàng
        public ActionResult OrderManagement(long? id)
        {
            var result = from or in db.Orders
                         where or.IdCus == id
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,
                             Address_cus = or.Address_cus,
                             Phone_Cus = or.Phone_Cus,
                             Status = or.Status,
                             OrderDate = or.OrderDate
                         };
            var ls = result.OrderBy(n => n.OrderDate);
            return View(ls);
        }
        //đợi duyệt
        public ActionResult WaitingApproval(long? id)
        {
            var result = from or in db.Orders
                         where or.IdCus == id && or.Status == false && or.Cancelled == false && or.Delivered == false
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,
                             Address_cus = or.Address_cus,
                             Phone_Cus = or.Phone_Cus,
                             Status = or.Status,
                             OrderDate = or.OrderDate
                         };
            var ls = result.OrderBy(n => n.OrderDate);
            return View(ls);
        }
        //đã duyệt & đang giao
        public ActionResult Approved(long? id)
        {
            var result = from or in db.Orders
                         where or.IdCus == id && or.Status == true && or.Delivered == false && or.Cancelled == false
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,
                             Address_cus = or.Address_cus,
                             Phone_Cus = or.Phone_Cus,
                             Status = or.Status,
                             OrderDate = or.OrderDate,
                             ComfirmDate = or.ComfirmDate
                         };          
            var lst = result.OrderBy(n => n.OrderDate);
            return View(lst);
        }
        //đã giao, hoàn tất đơn hàng
        public ActionResult Success(long? id)
        {
            var result = from or in db.Orders
                         where or.Status == true && or.Paid == true && or.Delivered == true && or.Cancelled == false
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,
                             Address_cus = or.Address_cus,
                             Phone_Cus = or.Phone_Cus,
                             Status = or.Status,
                             OrderDate = or.OrderDate
                         };
            var ls = result.OrderBy(n => n.OrderDate);
            return View(ls);
        }
        //hủy đơn hàng
        [HttpGet]
        public ActionResult OrderCancelled(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var orderDetails = db.OrderDetails.Where(n => n.IDOrder == id);
            ViewBag.orderDetailsList = orderDetails;
            return View(model);
        }

        /// <param name="ddh"></param>
        [HttpPost]
        public ActionResult OrderCancelled(_Order order)
        {
            _Order orderUpdate = db.Orders.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            if (orderUpdate != null)
            {
                orderUpdate.ComfirmDate = DateTime.Now;
                orderUpdate.Cancelled = true;
                orderUpdate.Status = false;
                db.SaveChanges();
                var orderDetails = db.OrderDetails.Where(n => n.IDOrder == order.IDOrder);
                ViewBag.orderDetailsList = orderDetails;
                ViewBag.Message = "Hủy đơn hàng thành công";
                foreach (var item in orderDetails)
                {
                    Product _pro = db.Products.Find(item.IDProduct);
                    _pro.Quantity = _pro.Quantity + item.QuantitySale;
                }
                db.SaveChanges();

            }
            else
            {
                ViewBag.Message = "Hủy đơn hàng không thành công";
            }
            return View(orderUpdate);
        }
        //đã hủy
        public ActionResult Cancelled(long? id)
        {
            var result = from or in db.Orders
                         where or.Cancelled == true
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,                             
                             Address_cus = or.Address_cus,
                             Phone_Cus = or.Phone_Cus,
                             Status = or.Status,
                             OrderDate = or.OrderDate,
                         };
            var cancelled = result.OrderBy(n => n.OrderDate);
            return View(cancelled);
        }
    }
}
