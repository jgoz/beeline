namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using Beeline.Routing;

	/// <summary>
	/// Provides methods for finding Route definitions in a given assembly.
	/// </summary>
	public static class DiscoverRoutes
	{
		/// <summary>
		/// Enumerates all of the route attributes in a given type's containing assembly.
		/// </summary>
		/// <typeparam name="T">A type in the assembly that will be searched.</typeparam>
		/// <returns>An <see cref="IEnumerable{RouteAttribute}"/> containing all of the route attributes in the assembly.</returns>
		public static IEnumerable<RouteAttribute> InAssemblyOf<T>()
		{
			return InAssembly(typeof(T).Assembly);
		}

		/// <summary>
		/// Enumerates all of the <see cref="RouteAttribute"/> attributes in an assembly.
		/// </summary>
		/// <param name="assembly">The assembly to search for route definitions.</param>
		/// <returns>An <see cref="IEnumerable{RouteAttribute}"/> containing all of the route attributes in the assembly.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="assembly"/> is null.</exception>
		public static IEnumerable<RouteAttribute> InAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			var actionMethods = assembly.GetTypes()
				.Where(t => typeof(Controller).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods());

			return actionMethods.SelectMany(GetActionRouteAttributes);
		}

		private static IEnumerable<RouteAttribute> GetActionRouteAttributes(MethodInfo method)
		{
			Object[] routeAttributes = method.GetCustomAttributes(typeof(RouteAttribute), false);

			if (!routeAttributes.Any())
				return new RouteAttribute[0];

			HttpVerbs httpMethod = GetHttpMethods(method)
				.DefaultIfEmpty(HttpVerbs.Get)
				.Aggregate((result, verb) => result | verb);

			return routeAttributes
				.Cast<RouteAttribute>()
				.Select(r => new RouteAttribute(r, httpMethod, method.Name, method.DeclaringType.Name.Replace("Controller", String.Empty)));
		}

		private static IEnumerable<HttpVerbs> GetHttpMethods(MethodInfo method)
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
