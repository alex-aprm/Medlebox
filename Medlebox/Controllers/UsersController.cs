using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;
using System.Web.Security;

namespace Medlebox.Controllers
{
    public class UsersController : BaseController
    {
        //
        // GET: /Auth/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string ReturnUrl)
        {
            User found = dal.GetUser(user.Email);
            bool UserValid =false;
            if (found != null)
            {
                user.PwdHash = found.PwdHash;
                UserValid = user.TryLogin();
            }
            if (UserValid)
            {
                dal.CurrentUser = found;
                FormsAuthentication.SetAuthCookie(user.Email, false);
                if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Ошибка входа");
            }

            return View(user);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
