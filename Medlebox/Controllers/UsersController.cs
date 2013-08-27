using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;
using System.Web.Security;
using Medlebox.DAL;

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

        public ActionResult Signin()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Signin(User user)
        {
            ModelState.Remove("MusicSource");
            if (ModelState.IsValid)
            {
                try
                {
                    dal.SaveUser(user);
                    dal.CurrentUser = user;
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    foreach (ValidationError err in ex.FieldErrors)
                    {
                        ModelState.AddModelError(err.Field, err.Error);
                    }
                }
            }
            return View(user);
        }


        [HttpPost]
        public ActionResult Login(User user, string ReturnUrl)
        {
            User found = dal.GetUser(user.Email);
            bool UserValid = false;
            if (found != null)
            {
                user.PwdHash = found.PwdHash;
                UserValid = user.TryLogin();
            }
            if (UserValid)
            {
                dal.CurrentUser = found;
                FormsAuthentication.SetAuthCookie(user.Email, true);
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
                ModelState.AddModelError("", "Неправильный пароль или почтовый адрес.");
            }

            return View(user);
        }
        [Authorize]

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]

        public ActionResult Profile(Guid? id)
        {
            if (id == null)
            { return View(dal.CurrentUser); }
            else
            {
                User u = dal.GetUser((Guid)id);
                return View(u);
            }

        }
        [Authorize]

        [HttpPost]
        public ActionResult Profile(User user, string SubAction, string RoleGid)
        {
            if (SubAction != "") //Если промежуточное действие с моделью
            {
                //user = ProcessSubAction(user, SubAction, RoleGid);
            }
            else //Если сохраняем модель
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        dal.SaveUser(user);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (ValidationException ex)
                    {
                        foreach (ValidationError err in ex.FieldErrors)
                        {
                            ModelState.AddModelError(err.Field, err.Error);
                        }
                    }
                }

            }
            return View(user);
        }


    }
}
