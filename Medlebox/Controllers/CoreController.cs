using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medlebox.Controllers
{
    public class CoreController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LoggedIn = false;
            
            //filterContext.Result = new RedirectResult(Url.Action("Login", "Auth"));

            base.OnActionExecuting(filterContext);
        }
    }
}
