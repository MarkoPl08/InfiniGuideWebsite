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
	/// CRUD operations for Routes.
	/// </summary>
	public class RoutesController : ApiController
	{
		/// <summary>
		/// Returns list of routes.
		/// </summary>
		public IEnumerable<Route> Get()
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				return entities.Routes.ToList();
			}
		}
		/// <summary>
		/// Returns a route by ID.
		/// </summary>
		public HttpResponseMessage Get(int id)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var entity = entities.Routes.FirstOrDefault(route => route.IDRoute == id);

				var routeMonuments = new List<RouteMonument>();
				foreach (var routeMonument in entities.RouteMonuments.ToList())
				{
					if (routeMonument.RouteID == id)
					{
						var monument = entities.Monuments.FirstOrDefault(b => b.IDMonument == routeMonument.MonumentID);
						routeMonument.Monument = monument;
						routeMonuments.Add(routeMonument);
					}
				}
				entity.RouteMonuments = routeMonuments;

				var routeRestaurants = new List<RouteRestaurant>();
				foreach (var routeRestaurant in entities.RouteRestaurants.ToList())
				{
					if (routeRestaurant.RouteID == id)
					{
						var restaurant = entities.Restaurants.FirstOrDefault(b => b.IDRestaurant == routeRestaurant.RestaurantID);
						routeRestaurant.Restaurant = restaurant;
						routeRestaurants.Add(routeRestaurant);
					}
				}
				entity.RouteRestaurants = routeRestaurants;

				var routeBars = new List<RouteBar>();
				foreach (var routeBar in entities.RouteBars.ToList())
				{
					if (routeBar.RouteID == id)
					{
						var bar = entities.Bars.FirstOrDefault(b => b.IDBar == routeBar.BarID);
						routeBar.Bar = bar;
						routeBars.Add(routeBar);
					}
				}
				entity.RouteBars = routeBars;

				if (entity != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, entity);
				}
				else
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route with [ID = " + id.ToString() + "] not found.");
				}
			}
		}
		/// <summary>
		/// Creates a new Route.
		/// </summary>
		public HttpResponseMessage Post(FullRoute fullRoute)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					entities.Routes.Add(fullRoute.Route);
					if (fullRoute.RouteMonuments != null)
					{
						foreach (var monument in fullRoute.RouteMonuments)
						{
							entities.RouteMonuments.Add(monument);
						}
					}
					if (fullRoute.RouteRestaurants != null)
					{
						foreach (var restaurant in fullRoute.RouteRestaurants)
						{
							entities.RouteRestaurants.Add(restaurant);
						}
					}
					if (fullRoute.RouteBars != null)
					{
						foreach (var bar in fullRoute.RouteBars)
						{
							entities.RouteBars.Add(bar);
						}
					}
					if (fullRoute.AccountRoute != null)
					{
						entities.AccountRoutes.Add(fullRoute.AccountRoute);
					}
					entities.SaveChanges();
					var message = Request.CreateResponse(HttpStatusCode.Created, fullRoute);
					message.Headers.Location = new Uri(Request.RequestUri + fullRoute.Route.IDRoute.ToString());
					return message;
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		/// <summary>
		/// Deletes a route by ID.
		/// </summary>
		public HttpResponseMessage Delete(int id)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					var entity = entities.Routes.FirstOrDefault(acc => acc.IDRoute == id);
					if (entity == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route with [ID = " + id.ToString() + "] not found to delete.");
					}

					entities.Routes.Remove(entity);
					entities.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		/// <summary>
		/// Updates a route by ID.
		/// </summary>
		public HttpResponseMessage Put(int id, [FromBody] Route route)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					var entity = entities.Routes.FirstOrDefault(r => r.IDRoute == id);
					if (entity == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route with [ID = " + id.ToString() + "] not found to update.");
					}
					entity.Name = route.Name;
					entity.About = route.About;

					entities.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.OK, entity);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		/// <summary>
		/// Returns a route by ID.
		/// </summary>
		[HttpGet]
		public HttpResponseMessage SavedRoutes(string email)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var account = entities.Accounts.FirstOrDefault(acc => acc.Email == email);
				var routes = new List<Route>();
				foreach (var accountRoute in entities.AccountRoutes.ToList())
				{
					if (accountRoute.AccountID == account.IDAccount && accountRoute.RouteType == RouteType.SavedRoute.ToString())
					{
						var route = entities.Routes.FirstOrDefault(r => r.IDRoute == accountRoute.RouteID);
						routes.Add(route);
					}
				}
				if (routes != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, routes);
				}
				else
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Routes with account [ID = " + email.ToString() + "] not found.");
				}
			}
		}
		/// <summary>
		/// Returns a route by ID.
		/// </summary>
		[HttpGet]
		public HttpResponseMessage YourRoutes(string email)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var account = entities.Accounts.FirstOrDefault(acc => acc.Email == email);
				var routes = new List<Route>();
				foreach (var accountRoute in entities.AccountRoutes.ToList())
				{
					if (accountRoute.AccountID == account.IDAccount && accountRoute.RouteType == RouteType.MyRoute.ToString())
					{
						var route = entities.Routes.FirstOrDefault(r => r.IDRoute == accountRoute.RouteID);
						routes.Add(route);
					}
				}
				if (routes != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, routes);
				}
				else
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Routes with account [ID = " + email.ToString() + "] not found.");
				}
			}
		}
		/// <summary>
		/// Returns a route by ID.
		/// </summary>
		[HttpPost]
		public HttpResponseMessage SaveRoute(int id, string email)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var account = entities.Accounts.FirstOrDefault(acc => acc.Email == email);
				var accountRoute = new AccountRoute
				{
					AccountID = account.IDAccount,
					RouteID = id,
					RouteType = RouteType.SavedRoute.ToString()
				};
				//if (accountRoute.RouteID == id && accountRoute.AccountID == account.IDAccount)
				//{
				//	return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Already saved!");
				//}
				entities.AccountRoutes.Add(accountRoute);
				entities.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK, account);
			}
		}
	}
}
