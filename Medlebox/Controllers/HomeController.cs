using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medlebox.Controllers
{
    public class HomeController : CoreController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
