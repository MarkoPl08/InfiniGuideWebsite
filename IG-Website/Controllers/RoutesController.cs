using ApiDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace IG_Website.Controllers
{
	public class RoutesController : Controller
	{
		// GET: Routes
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult AllRoutes()
		{
			return View();
		}

		public ActionResult CreateRoute()
		{
			return View();
		}
	}
}