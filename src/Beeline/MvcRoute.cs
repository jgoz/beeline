namespace Beeline
{
	using System;
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;

	internal class MvcRoute
	{
		public static MvcRoute FromAttribute(RouteAttribute routeAttribute)
		{
			return new MvcRoute(routeAttribute);
		}

		private MvcRoute(RouteAttribute routeAttribute)
		{
			if (routeAttribute == null)
				throw new ArgumentNullException("routeAttribute");

			Name = routeAttribute.Name;
			Url = routeAttribute.UrlPattern;
			Defaults = new RouteValueDictionary(routeAttribute.Defaults)
			{
				{ "controller", routeAttribute.ControllerName },
				{ "action", routeAttribute.ActionName }
			};
			Constraints = new RouteValueDictionary(routeAttribute.Constraints)
			{
				{ "isValidMethod", new HttpMethodConstraint(GetHttpMethods(routeAttribute.Method)) }
			};
		}

		public String Name { get; private set; }
		public String Url { get; private set; }
		public RouteValueDictionary Defaults { get; private set; }
		public RouteValueDictionary Constraints { get; private set; }

		private static String[] GetHttpMethods(HttpVerbs methodFlags)
		{
			return methodFlags.GetFlagsValues<HttpVerbs>()
				.Select(v => v.ToString().ToUpper())
				.ToArray();
		}
	}
}
