using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Website_Panda.Models;
using Website_Panda.Models.DAL;
using Website_Panda.Models.OrderModel;
using System.Data.Entity;
using Website_Panda.Models.Login;

namespace Website_Panda.Controllers
{
    public class OrderManagementController : ManagementController
    {
        // GET: OrderManagement
        private DB_Optical db = new DB_Optical();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllOrder()
        {
            return View(db.Orders.ToList());
        }
        public ActionResult ManagementOrderDetail(int? id)
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
        //đợi duyệt
        public ActionResult WaitingApproval()
        {
            var result = from or in db.Orders
                         where or.Status == false && or.Cancelled == false && or.Delivered == false
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,                        
                             OrderDate = or.OrderDate,
                             Status = or.Status
                         };
            var ls = result.OrderBy(n => n.OrderDate);
            return View(ls);
        }
        //duyệt đơn hàng
        [HttpGet]
        public ActionResult OrderApproval(int? id)
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
        public ActionResult OrderApproval(_Order order)
        {
            _Order orderUpdate = db.Orders.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            if (orderUpdate != null)
            {               
                orderUpdate.ComfirmDate = DateTime.Now;
                orderUpdate.Status = true;                
                db.SaveChanges();
                var orderDetails = db.OrderDetails.Where(n => n.IDOrder == order.IDOrder);
                ViewBag.orderDetailsList = orderDetails;
                ViewBag.Message = "Duyệt đơn hàng thành công";
            }
            else
            {
                ViewBag.Message = "Duyệt đơn hàng không thành công";
            }
            return View(orderUpdate);
        }

        //đã duyệt & chưa thanh toán & chưa giao
        public ActionResult Approved_Unpaid()
        {
            var c = from or in db.Orders
                    where or.Status == true && or.Paid == false && or.Delivered == false && or.Cancelled == false
                    select new OrderViewModel()
                    {
                        IDOrder = or.IDOrder,
                        IdCus = or.IdCus,
                        OrderDate = or.OrderDate,
                        Status = or.Status,
                        ComfirmDate = or.ComfirmDate
                    };
            var lst = c.OrderBy(n => n.OrderDate);
            return View(lst); 
        }
        //duyệt đã thanh toán
        [HttpGet]
        public ActionResult OrderPaid(int? id)
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
        public ActionResult OrderPaid(_Order order)
        {
            _Order orderUpdate = db.Orders.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            if (orderUpdate != null)
            {
                orderUpdate.ComfirmDate = DateTime.Now;
                orderUpdate.Paid = true;
                db.SaveChanges();
                var orderDetails = db.OrderDetails.Where(n => n.IDOrder == order.IDOrder);
                ViewBag.orderDetailsList = orderDetails;
                ViewBag.Message = "Duyệt đơn hàng thành công";
            }
            else
            {
                ViewBag.Message = "Duyệt đơn hàng không thành công";
            }
            return View(orderUpdate);
        }
        //đã duyệt & dã thanh toán & chưa giao
        public ActionResult Approved_paid()
        {
            var c = from or in db.Orders
                    where or.Status == true && or.Paid == true && or.Delivered==false && or.Cancelled == false 
                    select new OrderViewModel()
                    {
                        IDOrder = or.IDOrder,
                        IdCus = or.IdCus,
                        OrderDate = or.OrderDate,
                        Status = or.Status,
                        ComfirmDate = or.ComfirmDate
                    };
            var lst = c.OrderBy(n => n.OrderDate);
            return View(lst);
        }
        //duyệt đã giao
        [HttpGet]
        public ActionResult OrderDelivered(int? id)
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
        public ActionResult OrderDelivered(_Order order)
        {
            _Order orderUpdate = db.Orders.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            OrderDetail detail =db.OrderDetails.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            if (orderUpdate != null)
            {
                orderUpdate.DeliveryDate = DateTime.Now;
                orderUpdate.Delivered = true;
                if(orderUpdate.Customer.Score == null) { 
                    orderUpdate.Customer.Score = Convert.ToDouble(detail.OrderTotalPrice) * 0.01; //mua 100$ cộng 1 đ
                    
                }
                else {
                    orderUpdate.Customer.Score += Convert.ToDouble(detail.OrderTotalPrice) * 0.01;
                }      
                if(orderUpdate.Customer.Score < 50) {
                    orderUpdate.Customer.CustomerRank = "Khách hàng bậc đồng";
                }         
                else if (orderUpdate.Customer.Score >= 50 && orderUpdate.Customer.Score <= 150)
                { orderUpdate.Customer.CustomerRank= "Khách hàng bậc vàng"; }
                else
                {
                    orderUpdate.Customer.CustomerRank = "Khách hàng bậc kim cương";
                }
                db.SaveChanges();
                var orderDetails = db.OrderDetails.Where(n => n.IDOrder == order.IDOrder);
                
                ViewBag.orderDetailsList = orderDetails;
                ViewBag.Message = "Duyệt đơn hàng thành công";
            }
            else
            {
                ViewBag.Message = "Duyệt đơn hàng không thành công";
            }
            return View(orderUpdate);
        }
        //đã giao, hoàn tất đơn hàng
        public ActionResult Success()
        {
            var result = from or in db.Orders
                         where or.Status == true && or.Paid == true && or.Delivered == true && or.Cancelled == false
                         select new OrderViewModel()
                         {
                             IDOrder = or.IDOrder,
                             IdCus = or.IdCus,
                             Status = or.Status,
                             OrderDate = or.OrderDate,
                         };
            var ls = result.OrderBy(n => n.OrderDate);
            return View(ls);
        }
        //huy don hang
 
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
                //orderUpdate.Status = false;
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
        public ActionResult Cancelled()
        {
            var result = from r in db.Orders
                         where r.Cancelled == true
                         select new OrderViewModel()
                         {
                             IDOrder = r.IDOrder,
                             IdCus = r.IdCus,
                             ComfirmDate = r.ComfirmDate,
                             Status = r.Status,
                             OrderDate = r.OrderDate,
                         };
            var cancelled = result.OrderBy(n => n.OrderDate);
            return View(cancelled);
        }
        //duyệt đã thanh toán
        [HttpGet]
        public ActionResult DetailCancel(int? id)
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
        public ActionResult DetailCancel(_Order order)
        {
            _Order orderUpdate = db.Orders.Where(n => n.IDOrder.Equals(order.IDOrder)).FirstOrDefault();
            if (orderUpdate != null)
            {
                orderUpdate.ComfirmDate = DateTime.Now;
                orderUpdate.Cancelled = true;
                db.SaveChanges();
                var orderDetails = db.OrderDetails.Where(n => n.IDOrder == order.IDOrder);
                ViewBag.orderDetailsList = orderDetails;
                ViewBag.Message = "Duyệt đơn hàng thành công";
            }
            else
            {
                ViewBag.Message = "Duyệt đơn hàng không thành công";
            }
            return View(orderUpdate);
        }
    }
}