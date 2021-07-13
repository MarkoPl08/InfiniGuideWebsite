using ApiDataLayer;
using ApiDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IG_API.Controllers
{
	/// <summary>
	/// CRUD operations for KotlinRoutes.
	/// </summary>
	public class KotlinRoutesController : ApiController
	{
		/// <summary>
		/// Returns list of KotlinRoutes.
		/// </summary>		
		[HttpGet]
		public List<KotlinRoute> GetKotlinRoutes()
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var routes = new List<KotlinRoute>();
				foreach (var route in entities.Routes)
				{
					routes.Add(new KotlinRoute
					{
						IDRoute = route.IDRoute,
						About = route.About,
						Name = route.Name,
						PicturePath = route.PicturePath
					});
				}

				return routes;
			}
		}

		/// <summary>
		/// Returns list of KotlinRoutes.
		/// </summary>
		[HttpGet]
		public List<KotlinRoute> GetKotlinRoutes(int id)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var routes = new List<KotlinRoute>();
				foreach (var accountRoute in entities.AccountRoutes.ToList())
				{
					if (accountRoute.AccountID == id)
					{
						var route = entities.Routes.FirstOrDefault(r => r.IDRoute == accountRoute.RouteID);
						routes.Add(new KotlinRoute
						{
							IDRoute = route.IDRoute,
							About = route.About,
							Name = route.Name,
							PicturePath = route.PicturePath
						});
					}
				}

				return routes;
			}
		}
	}
}
