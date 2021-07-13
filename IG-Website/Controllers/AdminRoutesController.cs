using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace IG_Website.Controllers
{
    public class AdminRoutesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
