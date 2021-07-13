using ApiDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IG_API.Controllers
{
	/// <summary>
	/// CRUD operations for Monuments.
	/// </summary>
	public class MonumentsController : ApiController
	{
		/// <summary>
		/// Returns list of monuments.
		/// </summary>
		[HttpGet]
		public IEnumerable<Monument> Get()
		{

			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				return entities.Monuments.ToList();
			}
		}

		/// <summary>
		/// Creates a new Monument.
		/// </summary>
		public HttpResponseMessage Post([FromBody] Monument monument)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					entities.Monuments.Add(monument);
					entities.SaveChanges();
					var message = Request.CreateResponse(HttpStatusCode.Created, monument);
					message.Headers.Location = new Uri(Request.RequestUri + monument.IDMonument.ToString());
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