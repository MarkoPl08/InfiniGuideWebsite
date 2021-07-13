using ApiDataLayer;
using ApiDataLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IG_API.Controllers
{
    /// <summary>
    /// CRUD operations for KotlinRouteMonuments.
    /// </summary>
    public class KotlinRouteMonumentsController : ApiController
    {
        /// <summary>
        /// Returns list of KotlinRouteMonuments.
        /// </summary>        
        [HttpGet]
        public List<KotlinMonument> Get()
        {
            using (IG_DB_Entities entities = new IG_DB_Entities())
            {
                var monuments = new List<KotlinMonument>();
                foreach (var monument in entities.Monuments)
                {
                    monuments.Add(new KotlinMonument
                    {
                        IDMonument = monument.IDMonument,
                        MonumentID = monument.MonumentID,
                        About = monument.About,
                        Name = monument.Name,
                        Sound = monument.Sound,
                        Lat = monument.Lat,
                        Lng = monument.Lng
                    });
                }

                return monuments;
            }
        }
        /// <summary>
        /// Returns list of KotlinRouteMonuments.
        /// </summary>
        [HttpGet]
        public List<KotlinMonument> Get(int id)
        {
            using (IG_DB_Entities entities = new IG_DB_Entities())
            {
                var monuments = new List<KotlinMonument>();
                foreach (var routeMonument in entities.RouteMonuments.ToList())
                {
                    if (routeMonument.RouteID == id)
                    {
                        var monument = entities.Monuments.FirstOrDefault(m => m.IDMonument == routeMonument.MonumentID);
                        monuments.Add(new KotlinMonument
                        {
                            IDMonument = monument.IDMonument,
                            MonumentID = monument.MonumentID,
                            About = monument.About,
                            Name = monument.Name,
                            Sound = monument.Sound,
                            Lat = monument.Lat,
                            Lng = monument.Lng
                        });
                    }
                }

                return monuments;
            }
        }
    }
}