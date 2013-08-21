using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Medlebox.Models;
using Medlebox.DAL;

namespace Medlebox.Controllers
{
    public class CoreController : Controller
    {
        public CoreController(string MedleboxConnection)
            : base()
        {
            //Инитилизация всего
            this.MedleboxConnection = MedleboxConnection;
            dal = new MedleboxDAL(MedleboxConnection);
            //User CurrentUser = dal.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            //if (CurrentUser == null)
            //{
            //    CurrentUser = new User();
            //    CurrentUser.Email = System.Web.HttpContext.Current.User.Identity.Name;
            //    dal.SaveUser(CurrentUser);
            //}
            //    dal.CurrentUser = CurrentUser;
        }
       private string MedleboxConnection;
      // protected static NLog.Logger logger;
       protected MedleboxDAL dal;

       protected MedleboxDAL GetDALasync()
       {
           MedleboxDAL d = new MedleboxDAL(this.MedleboxConnection);
           //d.UserName = dal.UserName;
           return d;
       }
       
       protected override void OnException(ExceptionContext filterContext)
        {
           // logger.ErrorException("", filterContext.Exception);
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                ViewBag.Message = filterContext.Exception.Message;
                View("Error");
            }
        }
       protected override void Dispose(bool disposing)
       {
          dal.Dispose();
           base.Dispose(disposing);
       }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LoggedIn=HttpContext.User.Identity.IsAuthenticated;
            
            //filterContext.Result = new RedirectResult(Url.Action("Login", "Auth"));

            base.OnActionExecuting(filterContext);
        }
    }
}
