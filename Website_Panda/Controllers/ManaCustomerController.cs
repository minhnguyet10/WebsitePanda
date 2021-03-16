using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Website_Panda.Models.Login;

namespace Website_Panda.Controllers
{
    public class ManaCustomerController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[Common.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "CustomerAccount", action = "LoginCus" }));
            }
            //if (session != null && session.GroupID == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new
            //       RouteValueDictionary(new { controller = "LoginAdmin", action = "Index" }));
            //}
            //if (session.GroupID == "MEMBER")
            //{
            //    filterContext.Result = new RedirectToRouteResult(new
            //       RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}