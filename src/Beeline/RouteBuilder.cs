namespace Beeline
{
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;

	internal class RouteBuilder
	{
		public static RouteBuilder FromActionMethod(MethodInfo methodInfo)
		{
			return new RouteBuilder(methodInfo);
		}

		private readonly MethodInfo _actionMethod;

		private RouteBuilder(MethodInfo actionMethod)
		{
			if (actionMethod == null)
				throw new ArgumentNullException("actionMethod");

			_actionMethod = actionMethod;

			InitializeMetaData();
			InitializeRouteData(actionMethod.GetRouteAttributes().Single());
		}

		public String ActionName { get; private set; }
		public String ControllerName { get; private set; }

		public String Name { get; private set; }
		public HttpVerbs Verbs { get; private set; }
		public String Url { get; private set; }
		public RouteValueDictionary Defaults { get; private set; }
		public RouteValueDictionary Constraints { get; private set; }

		private void InitializeMetaData()
		{
			ActionName = _actionMethod.Name;
			ControllerName = _actionMethod.DeclaringType.Name.Replace("Controller", String.Empty);
			Verbs = _actionMethod.GetHttpVerbs();
		}

		private void InitializeRouteData(RouteAttribute routeAttribute)
		{
			Name = routeAttribute.Name ?? Verbs + "." + ControllerName + "." + ActionName;
			Url = routeAttribute.UrlPattern;
			Defaults = new RouteValueDictionary(routeAttribute.Defaults ?? new Object())
			{
				{ "controller", ControllerName },
				{ "action", ActionName }
			};
			Constraints = new RouteValueDictionary(routeAttribute.Constraints ?? new Object())
			{
				{ "isValidMethod", new HttpMethodConstraint(ExpandVerbs()) }
			};
		}

		private String[] ExpandVerbs()
		{
			return Verbs.GetFlagsValues<HttpVerbs>()
				.Select(v => v.ToString().ToUpper())
				.ToArray();
		}
	}
}
