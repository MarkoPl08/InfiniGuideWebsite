using ApiDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IG_API.Controllers
{
    public class RouteMonumentsController : ApiController
    {
        [HttpGet]
        public List<RouteMonument> Get(int id)
        {
            using (IG_DB_Entities entities = new IG_DB_Entities())
            {
                var routeMonuments = new List<RouteMonument>();
                foreach (var routeMonument in entities.RouteMonuments.ToList())
                {
                    if (routeMonument.RouteID == id)
                    {
                        //var monument = routeMonument.Monument;
                        var monument = entities.Monuments.FirstOrDefault(m => m.IDMonument == routeMonument.MonumentID);
                        routeMonument.Monument = monument;
                        routeMonuments.Add(routeMonument);
                    }
                }

                return routeMonuments;
            }
        }
    }
}
