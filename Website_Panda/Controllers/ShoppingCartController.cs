
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;
using Website_Panda.Models.Login;
using Website_Panda.Models.QueryModel;
using System.Configuration;
using Website_Panda.Models.Account;
using PayPal.Api;
using Website_Panda.Class;

namespace Website_Panda.Controllers
{
    public class ShoppingCartController : Controller
    {
        DB_Optical db = new DB_Optical();
        // GET: ShoppingCart
        public CartSession GetCart()
        {
            CartSession cart = Session["CartSession"] as CartSession;
            if (cart == null || Session["CartSession"] == null)
            {
                cart = new CartSession();
                Session["CartSession"] = cart;
            }
            return cart;
        }
        public ActionResult AddtoCart(long id)
        {
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (pro != null)
            {
                GetCart().Add(pro);
            }
            return RedirectToAction("ShowToCart", "ShoppingCart", new { r = Request.Url.ToString() });
        }
        public ActionResult ModalCart()
        {
            CartSession cart = Session["CartSession"] as CartSession;
            return View(cart);
        }
        public ActionResult ShowToCart()
        {
            if (Session["CartSession"] == null)
            {
                ViewBag.Empty = "Empty Cart";
            }

            CartSession cart = Session["CartSession"] as CartSession;

            return View(cart);
        }
        //public ActionResult EmptyCart()
        //{
        //    return View();
        //}
        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            CartSession cart = Session["CartSession"] as CartSession;
            int id_pro = int.Parse(form["ID_Product"]);

            Product _pro = db.Products.Find(id_pro);

            int quantity = int.Parse(form["Quantity"]);
            if (quantity > _pro.Quantity)
            {
                return RedirectToAction("ErrorCart", "ShoppingCart");
            }
            else
            {
                cart.Update_Quantity_Shopping(id_pro, quantity);
            }
            if (quantity <= 0)
            {
                cart.Remove_Cart_Item(id_pro);
                return RedirectToAction("ShowToCart", "ShoppingCart");
            }
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public ActionResult ErrorCart()
        { return View(); }
        public ActionResult RemoveCart(int id)
        {
            CartSession cart = Session["CartSession"] as CartSession;
            cart.Remove_Cart_Item(id);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int _t_item = 0;
            CartSession cart = Session["CartSession"] as CartSession;
            if (cart != null)
            {
                _t_item = cart.Total_Quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }
        public ActionResult Shopping_Success()
        {
            return View();
        }
        public ActionResult LoginToPurchase()
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            if (session == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("PurchaseAccount", "ShoppingCart");
            }
        }
        public ActionResult CheckOut()
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            
            if (session != null) {                
                var result = from r in db.Customers
                         where r.IdCus == session.UserID
                         select r;
            var cus2 = result.ToList().First();
            Customer cus = db.Customers.Find(session.UserID);
            ViewBag.Score = cus2.Score;
            }
            
            if (Session["CartSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                CartSession cart = Session["CartSession"] as CartSession;
                ViewBag.IDMethod = new SelectList(db.PaymentMethods, "IDMethod", "MethodName");
                return View(cart);
            }

            //CartSession cart = Session["CartSession"] as CartSession;

            //}
        }
        public PartialViewResult CheckOut_Total()
        {
            int _t_item = 0;
            CartSession cart = Session["CartSession"] as CartSession;
            if (cart != null)
            {
                _t_item = cart.Total_Quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("CheckOut_Total");
        }
        public ActionResult PurchaseAccount()
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            Customer cus = db.Customers.Find(session.UserID);
            return View(cus);
        }        
        public ActionResult VoucherAccount()
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            Customer cus = db.Customers.Find(session.UserID);
            //return View(cus);
            return PartialView(cus);
        }
        //public ActionResult _Payment() // Bi trung ten voi Payment cua paypal nen t sua lai
        //{
        //    return View();
        //}
        public ActionResult PurchaseSuccess(FormCollection form)
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            CartSession cart = Session["CartSession"] as CartSession;
            OrderDetail _order_Detail = new OrderDetail();
            _Order _order = new _Order();
            _order.IDMethod = Convert.ToInt32(form["IDMethod"]);
            if (_order.IDMethod == 1)
            {
                _order.PaymentMethodName = "Ship COD";
            }
            else
            {
                _order.PaymentMethodName = "Thanh toán bằng Paypal";
            }
            var result = from r in db.Customers
                         where r.IdCus == session.UserID
                         select r;
            if (session == null)
            {
                if (cart != null)
                {
                    _order.OrderDate = DateTime.Now;
                    _order.IdCus = null;
                    _order.LastName = form["LastName"];
                    _order.FirstName = form["FirstName"];
                    _order.Email_Cus = form["Email"];
                    _order.Phone_Cus = form["Phone"];
                    _order.Address_cus = form["Address"];

                    _order.Status = false;
                    _order.Paid = false;
                    _order.Cancelled = false;
                    _order.Delivered = false;

                    db.Orders.Add(_order);
                    if (_order.IDMethod == 2)
                    {
                        _order.Paid = true;
                        TempData["OrderTemp"] = _order;
                        return RedirectToAction("PayMent", "ShoppingCart");
                    }
                    foreach (var item in cart.Items)
                    {
                        _order_Detail.IDOrder = _order.IDOrder;
                        _order_Detail.IDProduct = item._shopping_product.IDProduct;
                        if (item._shopping_product.PromotionPrice <= 0 || item._shopping_product.PromotionPrice == null)
                        {
                            _order_Detail.Price = item._shopping_product.Price;
                        }
                        else
                        {
                            _order_Detail.Price = item._shopping_product.PromotionPrice;
                        }
                        _order_Detail.QuantitySale = item._shopping_quantity;
                        _order_Detail.CreateDay = DateTime.Now;
                        Product _pro = db.Products.Find(_order_Detail.IDProduct);
                        if (_pro == null)
                        {
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }
                        else
                        {
                            _pro.Quantity = _pro.Quantity - _order_Detail.QuantitySale;
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }
                        db.SaveChanges();
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMailAdmin1.html"));
                    content = content.Replace("{{IDOrder}}", _order.IDOrder.ToString());
                    content = content.Replace("{{Name}}", _order.LastName.ToString());
                    content = content.Replace("{{Quantity}}", _order_Detail.QuantitySale.ToString());
                    content = content.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new Mail().SendMail(toMail, "Đơn hàng mới từ Panda.", content);

                    string content1 = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMail1.html"));
                    content1 = content1.Replace("{{IDOrder}}", _order_Detail.IDOrder.ToString());
                    content1 = content1.Replace("{{LastName}}", _order.LastName.ToString());
                    content = content.Replace("{{Quantity}}", _order_Detail.QuantitySale.ToString());
                    content1 = content1.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMails = _order.Email_Cus;
                    new Mail().SendMail(toMails, "Đơn hàng của bạn.", content1);

                    db.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("Shopping_Success", "ShoppingCart");
                }
                return Content("empty cart");
            }

            else
            {
                var cus2 = result.ToList().First();
                Customer cus = db.Customers.Find(session.UserID);
                if (result != null)
                {
                    if (_order.IDMethod == 2)
                    {
                        //    _order.Paid = true;
                        TempData["OrderTemp"] = _order;
                        return RedirectToAction("PayMent", "ShoppingCart");
                    }

                    _order.OrderDate = DateTime.Now;
                    _order.IdCus = cus2.IdCus;
                    _order.FirstName = cus2.FirstName;
                    _order.LastName = cus2.LastName;                    
                    _order.Email_Cus = cus2.Email_Cus;
                    _order.Phone_Cus = cus2.Phone_Cus;
                    _order.Address_cus = cus2.Address_Cus;
                    _order.Status = false;
                    _order.Paid = false;
                    _order.Cancelled = false;
                    _order.Delivered = false;

                    db.Orders.Add(_order);
                    
                    foreach (var item in cart.Items)
                    {
                        _order_Detail.IDOrder = _order.IDOrder;
                        _order_Detail.IDProduct = item._shopping_product.IDProduct;
                        if (item._shopping_product.PromotionPrice <= 0 || item._shopping_product.PromotionPrice == null)
                        {
                            _order_Detail.Price = item._shopping_product.Price;
                        }
                        else
                        {
                            _order_Detail.Price = item._shopping_product.PromotionPrice;
                        }
                        _order_Detail.QuantitySale = item._shopping_quantity;
                        _order_Detail.CreateDay = DateTime.Now;
                        Product _pro = db.Products.Find(_order_Detail.IDProduct);
                        if (_pro == null)
                        {
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }
                        else
                        {
                            _pro.Quantity = _pro.Quantity - _order_Detail.QuantitySale;
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }
                        db.SaveChanges();
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMailAdmin.html"));
                    content = content.Replace("{{IdCus}}", _order.Customer.IdCus.ToString());
                    content = content.Replace("{{IDOrder}}", _order.IDOrder.ToString());
                    content = content.Replace("{{Name}}", _order.Customer.UserName.ToString());
                    content = content.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new Mail().SendMail(toMail, "Đơn hàng mới từ Panda.", content);

                    string content1 = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMail.html"));
                    content1 = content1.Replace("{{IdCus}}", _order.Customer.IdCus.ToString());
                    content1 = content1.Replace("{{IDOrder}}", _order_Detail.IDOrder.ToString());
                    content1 = content1.Replace("{{LastName}}", _order.Customer.LastName.ToString());
                    content1 = content1.Replace("{{Name}}", _order.Customer.UserName.ToString());
                    content1 = content1.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMails = _order.Customer.Email_Cus;
                    new Mail().SendMail(toMails, "Đơn hàng của bạn.", content1);

                    db.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("Shopping_Success", "ShoppingCart");
                }

                return RedirectToAction("Shopping_Success", "ShoppingCart");
            }
        }
        //-----------------------------------PayMent-------------------------
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var paypaloder = DateTime.Now.Ticks;//  ma giao dich
            double tongtien = 0;
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            CartSession cart = Session["CartSession"] as CartSession;
            foreach (var item in cart.Items)
            {
                if (item._shopping_product.PromotionPrice <= 0 || item._shopping_product.PromotionPrice == null)
                {
                    itemList.items.Add(new Item()
                    {
                        name = item._shopping_product.ProductName,
                        currency = "USD",
                        price = item._shopping_product.Price.ToString(), // gia nay la chua khuyen mai
                        quantity = item._shopping_quantity.ToString(),
                        sku = "sku"
                    });
                    tongtien += double.Parse((item._shopping_product.Price * item._shopping_quantity).ToString());
                }
                else
                {
                    itemList.items.Add(new Item()
                    {
                        name = item._shopping_product.ProductName,
                        currency = "USD",
                        price = item._shopping_product.PromotionPrice.ToString(), // gia khuyen mai
                        quantity = item._shopping_quantity.ToString(),
                        sku = "sku"
                    });
                    tongtien += double.Parse((item._shopping_product.PromotionPrice * item._shopping_quantity).ToString());
                }
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            var details = new Details()
            {
                tax = "0",
                shipping = "10", /// tien ship mac dinh la 10
                subtotal = tongtien.ToString() /// cho nay lay tong tien don hang (chua co ship)
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = (tongtien + double.Parse(details.shipping)).ToString(), //cho nay tinh tong don hang = tien tong don + ship
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypaloder}",
                invoice_number = paypaloder.ToString(),
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
        public ActionResult PayMent()
        {
            //_Order order = new _Order();
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PayMent?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("CheckoutFail");// cho nay neu thanh toan that bai thi chuyen toi trang CheckoutFail, m sua trang do lai di
                    }

                    return RedirectToAction("PaymentSuccess", "ShoppingCart");
                    //------------cho nay may viet them sau khi thanh toan xong thi lam gi nua(luu don hang vo database, gui mail, ....)
                }
            }

            catch (Exception)
            {
                return RedirectToAction("CheckoutFail", "ShoppingCart");
            }
            //on successful payment, show success page to user.

            //return RedirectToAction("PaymentSuccess", "ShoppingCart");// thanh toan thanh cong thi chuyen toi trang CheckoutSuccess, sua lai luon di :)))
        }
        public ActionResult CheckoutFail() // trang thanh toan that bai
        {
            return View();
        }
        public ActionResult CheckoutSuccess() // trang thanh toan thanh cong
        {
            return View();
        }

        public ActionResult PaymentSuccess(FormCollection form)
        {
            var session = (UserLogin)Session[Common.USER_SESSION];

            CartSession cart = Session["CartSession"] as CartSession;
            

            if (session == null)
            {
                _Order _order = TempData["OrderTemp"] as _Order;
                db.Orders.Add(_order);

                decimal total = 0;
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_Detail = new OrderDetail();
                    _order_Detail.IDOrder = _order.IDOrder;
                    _order_Detail.IDProduct = item._shopping_product.IDProduct;
                    if (item._shopping_product.PromotionPrice <= 0 || item._shopping_product.PromotionPrice == null)
                    {
                        _order_Detail.Price = item._shopping_product.Price;
                    }
                    else
                    {
                        _order_Detail.Price = item._shopping_product.PromotionPrice;
                    }
                    _order_Detail.QuantitySale = item._shopping_quantity;
                    _order_Detail.CreateDay = DateTime.Now;
                    Product _pro = db.Products.Find(_order_Detail.IDProduct);
                    if (_pro == null)
                    {
                        db.OrderDetails.Add(_order_Detail);
                        _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        total = (decimal)_order_Detail.OrderTotalPrice;
                    }
                    else
                    {
                        _pro.Quantity = _pro.Quantity - _order_Detail.QuantitySale;
                        db.OrderDetails.Add(_order_Detail);
                        _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        total = (decimal)_order_Detail.OrderTotalPrice;
                    }
                    db.SaveChanges();
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMailAdmin.html"));
                //content = content.Replace("{{IdCus}}", _order.Customer.IdCus.ToString());
                content = content.Replace("{{IDOrder}}", _order.IDOrder.ToString());
                content = content.Replace("{{Name}}", _order.LastName.ToString());
                content = content.Replace("{{Total}}", total.ToString());
                var toMail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new Mail().SendMail(toMail, "Đơn hàng mới từ Panda.", content);

                string content1 = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMail.html"));
                //content1 = content1.Replace("{{IdCus}}", _order.Customer.IdCus.ToString());
                content1 = content1.Replace("{{IDOrder}}", _order.IDOrder.ToString());
                content1 = content1.Replace("{{LastName}}", _order.LastName.ToString());
                //content1 = content1.Replace("{{Name}}", _order.Customer.UserName.ToString());
                content1 = content1.Replace("{{Total}}", total.ToString());
                var toMails = _order.Email_Cus;
                new Mail().SendMail(toMails, "Đơn hàng của bạn.", content1);

                db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("Shopping_Success", "ShoppingCart");

            }
            else
            {
                var result = from r in db.Customers
                             where r.IdCus == session.UserID
                             select r;
                var cus2 = result.ToList().First();
                Customer cus = db.Customers.Find(session.UserID);
                OrderDetail _order_Detail = new OrderDetail();
                _Order order = new _Order();
                order = TempData["OrderTemp"] as _Order;
                if (result != null)
                {
                    order.OrderDate = DateTime.Now;
                    order.IdCus = cus2.IdCus;
                    order.FirstName = cus2.FirstName;
                    order.LastName = cus2.LastName;
                    order.Email_Cus = cus2.Email_Cus;
                    order.Phone_Cus = cus2.Phone_Cus;
                    order.Address_cus = cus2.Address_Cus;
                    order.Status = false;
                    order.Paid = true;
                    order.Cancelled = false;
                    order.Delivered = false;

                    db.Orders.Add(order);

                    foreach (var item in cart.Items)
                    {
                        _order_Detail.IDOrder = order.IDOrder;
                        _order_Detail.IDProduct = item._shopping_product.IDProduct;
                        if (item._shopping_product.PromotionPrice <= 0 || item._shopping_product.PromotionPrice == null)
                        {
                            _order_Detail.Price = item._shopping_product.Price;
                        }
                        else
                        {
                            _order_Detail.Price = item._shopping_product.PromotionPrice;
                        }
                        _order_Detail.QuantitySale = item._shopping_quantity;
                        _order_Detail.CreateDay = DateTime.Now;
                        Product _pro = db.Products.Find(_order_Detail.IDProduct);
                        if (_pro == null)
                        {
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }
                        else
                        {
                            _pro.Quantity = _pro.Quantity - _order_Detail.QuantitySale;
                            db.OrderDetails.Add(_order_Detail);
                            _order_Detail.OrderTotalPrice = (_order_Detail.Price.Value * _order_Detail.QuantitySale.Value);
                        }

                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMailAdmin.html"));
                    content = content.Replace("{{IdCus}}", order.Customer.IdCus.ToString());
                    content = content.Replace("{{IDOrder}}", order.IDOrder.ToString());
                    content = content.Replace("{{Name}}", order.Customer.UserName.ToString());
                    content = content.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new Mail().SendMail(toMail, "Đơn hàng mới từ Panda.", content);

                    string content1 = System.IO.File.ReadAllText(Server.MapPath("~/Content/Assets/Client/SendMail.html"));
                    content1 = content1.Replace("{{IdCus}}", order.Customer.IdCus.ToString());
                    content1 = content1.Replace("{{IDOrder}}", _order_Detail.IDOrder.ToString());
                    content1 = content1.Replace("{{LastName}}", order.Customer.LastName.ToString());
                    content1 = content1.Replace("{{Name}}", order.Customer.UserName.ToString());
                    content1 = content1.Replace("{{Total}}", _order_Detail.OrderTotalPrice.ToString());
                    var toMails = order.Customer.Email_Cus;
                    new Mail().SendMail(toMails, "Đơn hàng của bạn đã thanh toán thành công.", content1);

                    db.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("Shopping_Success", "ShoppingCart");
                }

                return RedirectToAction("Shopping_Success", "ShoppingCart");

            }
        }
    }
}