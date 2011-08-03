namespace Beeline
{
	using System;
	using System.Collections.Generic;
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

			var routeAttributes = actionMethod.GetRouteAttributes();

			if (!routeAttributes.Any() || routeAttributes.Count() > 1)
				throw new ArgumentException("Expected exactly 1 RouteAttribute on action method " + actionMethod.Name, "actionMethod");

			_actionMethod = actionMethod;

			InitializeMetaData();
			InitializeRouteData(routeAttributes.Single());
		}

		public String ActionName { get; private set; }
		public String ControllerName { get; private set; }

		public String Name { get; private set; }
		public HttpVerbs Method { get; private set; }
		public String Url { get; private set; }
		public RouteValueDictionary Defaults { get; private set; }
		public RouteValueDictionary Constraints { get; private set; }

		private void InitializeMetaData()
		{
			ActionName = _actionMethod.Name;
			ControllerName = _actionMethod.DeclaringType.Name.Replace("Controller", String.Empty);
			Method = GetHttpMethod(_actionMethod);
		}

		private void InitializeRouteData(RouteAttribute routeAttribute)
		{
			Name = routeAttribute.Name ?? Method + "." + ControllerName + "." + ActionName;
			Url = routeAttribute.UrlPattern;
			Defaults = new RouteValueDictionary(routeAttribute.Defaults ?? new Object())
			{
				{ "controller", ControllerName },
				{ "action", ActionName }
			};
			Constraints = new RouteValueDictionary(routeAttribute.Constraints ?? new Object())
			{
				{ "isValidMethod", new HttpMethodConstraint(ExpandMethod()) }
			};
		}

		private String[] ExpandMethod()
		{
			return Method.GetFlagsValues<HttpVerbs>()
				.Select(v => v.ToString().ToUpper())
				.ToArray();
		}

		private static HttpVerbs GetHttpMethod(MethodInfo method)
		{
			return GetHttpMethodAttributes(method)
				.DefaultIfEmpty(HttpVerbs.Get)
				.Aggregate((result, verb) => result | verb);
		}

		private static IEnumerable<HttpVerbs> GetHttpMethodAttributes(MethodInfo method)
		{
			return method.GetCustomAttributes(typeof(HttpGetAttribute), false)
				.Union(method.GetCustomAttributes(typeof(HttpPostAttribute), false))
				.Union(method.GetCustomAttributes(typeof(HttpPutAttribute), false))
				.Union(method.GetCustomAttributes(typeof(HttpDeleteAttribute), false))
				.Select(m => m.GetType())
				.Select(HttpMethodAttributeTypeToVerb);
		}

		private static HttpVerbs HttpMethodAttributeTypeToVerb(Type httpMethodAttributeType)
		{
			if (httpMethodAttributeType == typeof(HttpGetAttribute))
				return HttpVerbs.Get;
			if (httpMethodAttributeType == typeof(HttpPostAttribute))
				return HttpVerbs.Post;
			if (httpMethodAttributeType == typeof(HttpPutAttribute))
				return HttpVerbs.Put;
			if (httpMethodAttributeType == typeof(HttpDeleteAttribute))
				return HttpVerbs.Delete;

			throw new ArgumentException("Expected one of HttpGet, HttpPost, HttpPut or HttpDelete attribute types", "httpMethodAttributeType");
		}
	}
}
