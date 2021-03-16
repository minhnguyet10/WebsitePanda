using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models.DAL;
using Website_Panda.Models.QueryModel;

namespace Website_Panda.Controllers
{
    public class WishListController : Controller
    {
        DB_Optical db = new DB_Optical();
        // GET: WishList
        public WishList GetWishList()
        {
            WishList list = Session["WishList"] as WishList;
            if (list == null || Session["WishList"] == null)
            {
                list = new WishList();
                Session["WishList"] = list;
            }
            return list;
        }
        public ActionResult AddToWishList(long id)
        {
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (pro != null)
            {
                GetWishList().Add(pro);
            }
            return RedirectToAction("ShowToWishList", "WishList", new { r = Request.Url.ToString() });
        }
        public ActionResult ShowToWishList()
        { 
            if (Session["WishList"] == null)
            {
                ViewBag.Empty = "Empty wish list";
            }
            WishList wl = Session["WishList"] as WishList;
            return View(wl);
        }
        public PartialViewResult BagWishList()
        {
            int _t_item = 0;
            WishList wl = Session["WishList"] as WishList;
            if (wl != null)
            {
                _t_item = wl.Total_Quantity();
            }
            ViewBag.infoWishList = _t_item;
            return PartialView("BagWishList");
        }
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
            WishList wl = Session["WishList"] as WishList;
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (pro != null)
            {
                GetCart().Add(pro);
                wl.ClearWishlist();
            }
            return RedirectToAction("ShowToCart", "ShoppingCart", new { r = Request.Url.ToString() });
        }
        public ActionResult RemoveWishList(int id)
        {
            WishList wl = Session["WishList"] as WishList;
            wl.Remove_WishList_Item(id);
            return RedirectToAction("ShowToWishList", "WishList");
        }
    }
}