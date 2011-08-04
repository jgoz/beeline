namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using Beeline.Routing;

	internal static class MethodInfoExtensions
	{
		public static Boolean HasRouteAttributes(this MethodInfo methodInfo)
		{
			return methodInfo.GetRouteAttributes().Any();
		}

		public static IEnumerable<RouteAttribute> GetRouteAttributes(this MethodInfo methodInfo)
		{
			return methodInfo.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>();
		}

		public static HttpVerbs GetHttpVerbs(this MethodInfo method)
		{
			return method
				.GetCustomAttributes(typeof(ActionMethodSelectorAttribute), false)
				.SelectMany(HttpMethodAttributeToVerb)
				.Distinct()
				.DefaultIfEmpty(HttpVerbs.Get)
				.Aggregate((result, verb) => result | verb);
		}

		private static IEnumerable<HttpVerbs> HttpMethodAttributeToVerb(Object attribute)
		{
			Type type = attribute.GetType();

			if (type == typeof(HttpGetAttribute))
				return new[] { HttpVerbs.Get };
			if (type == typeof(HttpPostAttribute))
				return new[] { HttpVerbs.Post };
			if (type == typeof(HttpPutAttribute))
				return new[] { HttpVerbs.Put };
			if (type == typeof(HttpDeleteAttribute))
				return new[] { HttpVerbs.Delete };

			if (type == typeof(AcceptVerbsAttribute))
				return MethodStringsToVerbs(((AcceptVerbsAttribute)attribute).Verbs);

			if (type == typeof(NonActionAttribute))
				throw new InvalidOperationException("RouteAttribute cannot be combined with NonActionAttribute");

			throw new ArgumentException("Unsupported action method selector attribute type: " + type.Name, "attribute");
		}

		private static IEnumerable<HttpVerbs> MethodStringsToVerbs(IEnumerable<String> methods)
		{
			return methods.Select(s => (HttpVerbs)Enum.Parse(typeof(HttpVerbs), s, true));
		}
	}
}
