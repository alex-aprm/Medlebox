using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Medlebox.Models;
using Medlebox.DAL;
using System.Net;
using System.Net.Http;
using System.Web.Http;


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
            User CurrentUser = dal.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            //if (CurrentUser == null)
            //{
            //    CurrentUser = new User();
            //    CurrentUser.Email = System.Web.HttpContext.Current.User.Identity.Name;
            //    dal.SaveUser(CurrentUser);
            //}
                dal.CurrentUser = CurrentUser;
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

            string controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RouteData.Values["action"].ToString().ToLower();
            string url = filterContext.HttpContext.Request.RawUrl.ToString();
            bool reset = filterContext.HttpContext.Request.Params["reset"] == "True";
            bool modal = filterContext.HttpContext.Request.Params["modal"] == "True";

            ViewBag.LoggedIn=HttpContext.User.Identity.IsAuthenticated;
            ViewBag.Controller = controller;

            if (!modal)
            {
                if (action == "index")
                {
                    if (Session["IndexUrl"] != null && Session["NonIndexUrl"] != null && !reset)
                    {
                        url = Session["IndexUrl"].ToString();
                        Session.Remove("NonIndexUrl");
                        Session.Remove("IndexUrl");
                        filterContext.Result = new RedirectResult(url);
                        return;

                    }
                    Session["IndexUrl"] = url;
                }
                else
                {
                    if (Session["IndexUrl"] != null)
                    {
                        Session["NonIndexUrl"] = url;
                    }
                }
            }

            // TempData.Remove("success");
            base.OnActionExecuting(filterContext);
        }
    }


    public class CoreAPIController : ApiController
    {
        public CoreAPIController(string MedleboxConnection)
            : base()
        {
            //Инитилизация всего
            this.MedleboxConnection = MedleboxConnection;
            dal = new MedleboxDAL(MedleboxConnection);
            User CurrentUser = dal.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            //if (CurrentUser == null)
            //{
            //    CurrentUser = new User();
            //    CurrentUser.Email = System.Web.HttpContext.Current.User.Identity.Name;
            //    dal.SaveUser(CurrentUser);
            //}
            dal.CurrentUser = CurrentUser;
        }
        private string MedleboxConnection;
        // protected static NLog.Logger logger;
        protected MedleboxDAL dal;

    }
}
