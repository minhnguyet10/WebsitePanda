using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Website_Panda.Models.Login;

namespace Website_Panda.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "LoginAdmin", action = "Index" }));
            }
            if (session != null && session.GroupID == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { controller = "LoginAdmin", action = "Index" }));
            }
            if (session != null && session.GroupID == "SHIPPER")
            {               
                    filterContext.Result = new RedirectToRouteResult(new
                       RouteValueDictionary(new { controller = "OrderManagement", action = "Approved_Unpaid" }));              
            }
            base.OnActionExecuting(filterContext);
        }
    }
}