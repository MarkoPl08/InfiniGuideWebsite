using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDataLayer.Models
{
	public class FullRoute
	{
		public Route Route { get; set; }
		public RouteMonument[] RouteMonuments { get; set; }
		public RouteRestaurant[] RouteRestaurants { get; set; }
		public RouteBar[] RouteBars { get; set; }
		public AccountRoute AccountRoute { get; set; }

	}
}
