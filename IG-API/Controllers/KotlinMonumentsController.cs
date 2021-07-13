using ApiDataLayer;
using ApiDataLayer.Models;
using System.Linq;
using System.Web.Http;

namespace IG_API.Controllers
{
    /// <summary>
    /// CRUD operations for KotlinMonuments.
    /// </summary>
    public class KotlinMonumentsController : ApiController
    {
        /// <summary>
        /// Returns KotlinMonument.
        /// </summary>
        [HttpGet]
        public KotlinMonument GetKotlinMonument(int id)
        {
            using (IG_DB_Entities entities = new IG_DB_Entities())
            {
                var monument = entities.Monuments.FirstOrDefault(m => m.IDMonument == id);
                var kotlinMonument = new KotlinMonument
                {
                    IDMonument = monument.IDMonument,
                    MonumentID = monument.MonumentID,
                    About = monument.About,
                    Name = monument.Name,
                    Sound = monument.Sound,
                    Lat = monument.Lat,
                    Lng = monument.Lng
                };
                return kotlinMonument;
            }
        }
    }
}
