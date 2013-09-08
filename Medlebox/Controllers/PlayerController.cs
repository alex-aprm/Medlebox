using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;

namespace Medlebox.Controllers
{
    public class PlayerController : BaseController
    {
        //
        // GET: /Player/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Play(Guid? Gid)
        {
            Song song = dal.GetPlayable(Gid ?? Guid.Empty);


          
            return View("Index");
        }


    }
}
