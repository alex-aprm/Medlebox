using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.DAL;

namespace Medlebox.Controllers
{
    public class BaseController : CoreController
    {
        public BaseController()
            : base(God.GetConnectionString("MedleboxConnection"))
        {}
       
 
    }
}
