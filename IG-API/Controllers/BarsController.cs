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
	/// CRUD operations for Bars.
	/// </summary>
	public class BarsController : ApiController
    {
		/// <summary>
		/// Returns list of Bars.
		/// </summary>
		[HttpGet]
		public IEnumerable<Bar> Get()
		{

			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				return entities.Bars.ToList();
			}
		}

		/// <summary>
		/// Creates a new Bar.
		/// </summary>
		public HttpResponseMessage Post([FromBody] Bar bar)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					entities.Bars.Add(bar);
					entities.SaveChanges();
					var message = Request.CreateResponse(HttpStatusCode.Created, bar);
					message.Headers.Location = new Uri(Request.RequestUri + bar.IDBar.ToString());
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
