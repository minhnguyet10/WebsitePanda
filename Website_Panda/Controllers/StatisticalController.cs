using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_Panda.Models;
using Website_Panda.Models.DAL;

namespace Website_Panda.Controllers
{
    public class StatisticalController : ManagementController
    {
        DB_Optical db = new DB_Optical() ;
        public IEnumerable<BestSeller> GetBestSeller() 
        {          
            var bestSeller = from bs in db.OrderDetails.AsEnumerable()
                           group bs by bs.IDProduct into g
                           orderby g.Sum(x => x.IDProduct) descending
                           select new BestSeller
                           {
                               IDPro = g.Key,
                               Image = g.First().Product.Image,
                               NamePro = g.First().Product.ProductName,
                               Price = g.First().Product.Price,
                               Quantity = g.Sum(x => x.QuantitySale)
                           };
            return bestSeller.Take(10).ToList();
        }
        public ActionResult GetBestSellerChart()
        {
            var bestSeller = from bs in db.OrderDetails.AsEnumerable()
                             group bs by bs.IDProduct into g
                             orderby g.Sum(x => x.IDProduct) descending
                             select new
                             {
                                 NamePro = g.First().Product.ProductName,
                                 Quantity = g.Sum(x => x.QuantitySale)
                             };
            var temp = bestSeller.Take(10).ToList();
            var query = temp.OrderByDescending(m => m.Quantity).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        // GET: Statistical
        public ActionResult BestSeller()
        {           
            return View(GetBestSeller().OrderByDescending(m => m.Quantity));
        }
        //Thống kê doanh thu
        public ActionResult Income(DateTime? fromDate, DateTime? toDate)
        {
            var result = from or in db.Orders
                         join odt in db.OrderDetails on or.IDOrder equals odt.IDOrder into t
                         from odt in t.DefaultIfEmpty()
                         select new IncomeMD
                         {
                             OrderDate = or.OrderDate,
                             IDOrder = odt.IDOrder,
                             IdCus = or.IdCus,
                             QuantitySale = t.Sum(s => s.QuantitySale),
                             Price = t.Sum(s => s.Price)
                         };
            var order = result.OrderByDescending(m => m.OrderDate);
            if (!fromDate.HasValue) fromDate = DateTime.Now.Date;
            if (!toDate.HasValue) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            if (toDate <= fromDate) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var search = order.Where(c => c.OrderDate >= fromDate && c.OrderDate <= toDate).ToList();
            return View(search);
        }
        //public ActionResult GetIncomeChart()
        //{           
            
        //}
        public ActionResult Index()
        {
            return View();
        }
    }
}