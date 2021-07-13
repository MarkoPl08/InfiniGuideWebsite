using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDataLayer.Models
{
    public class KotlinMonument
    {
        public int IDMonument { get; set; }
        public string MonumentID { get; set; }
        public string Sound { get; set; }
        public string About { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
