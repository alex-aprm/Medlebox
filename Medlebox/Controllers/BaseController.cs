using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.DAL;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Medlebox.Controllers
{
#if DEBUG
    public abstract class BaseController : CoreController
    {
        public BaseController()
            : base(God.GetConnectionString("MedleboxConnectionTEST"))
        {}
    }

    public abstract class BaseAPIController : CoreAPIController
    {
        public BaseAPIController()
            : base(God.GetConnectionString("MedleboxConnectionTEST"))
        { }
    }
#else
    public abstract class BaseController : CoreController
    {
        public BaseController()
            : base(God.GetConnectionString("MedleboxConnection"))
        {}
    }

    public abstract class BaseAPIController : CoreAPIController
    {
        public BaseAPIController()
            : base(God.GetConnectionString("MedleboxConnection"))
        { }
    }

#endif
}
