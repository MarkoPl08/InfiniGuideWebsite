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
	/// CRUD operations for Restaurants.
	/// </summary>
	public class RestaurantsController : ApiController
	{
		/// <summary>
		/// Returns list of restaurants.
		/// </summary>
		[HttpGet]
		public IEnumerable<Restaurant> Get()
		{

			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				return entities.Restaurants.ToList();
			}
		}

		/// <summary>
		/// Creates a new Monument.
		/// </summary>
		public HttpResponseMessage Post([FromBody] Restaurant restaurant)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					entities.Restaurants.Add(restaurant);
					entities.SaveChanges();
					var message = Request.CreateResponse(HttpStatusCode.Created, restaurant);
					message.Headers.Location = new Uri(Request.RequestUri + restaurant.IDRestaurant.ToString());
					return message;
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}		
	}
}
