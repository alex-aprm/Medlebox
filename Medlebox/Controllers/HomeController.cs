using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CsQuery;
using Medlebox.Models;
using System.Threading.Tasks;
using System.IO;

namespace Medlebox.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Test()
        {
            Response.ContentType = "audio/mpeg";
            Response.Write("hi");
            HttpContext.ApplicationInstance.CompleteRequest();
            return new EmptyResult();
        }
        //
        // GET: /Home/
        public ActionResult Index(Guid? Gid)
        {
            return View();
        }


     }
}