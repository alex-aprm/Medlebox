using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;

namespace Medlebox.Controllers
{
    public class UserController : CoreController
    {
        //
        // GET: /Auth/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            return View(user);
        }

    }
}
