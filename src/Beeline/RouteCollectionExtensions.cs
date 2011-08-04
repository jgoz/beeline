namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;

	/// <summary>
	/// Extension methods for <see cref="RouteCollection"/>.
	/// </summary>
	public static class RouteCollectionExtensions
	{
		/// <summary>
		/// Searches the assembly containing <typeparamref name="T"/> for <see cref="RouteAttribute"/> declarations
		/// and maps the results in a specified route collection.
		/// </summary>
		/// <typeparam name="T">A type in the assembly that will be searched.</typeparam>
		/// <param name="routeCollection">The route collection.</param>
		/// <returns>A list of <see cref="Route"/> objects added to <paramref name="routeCollection"/>.</returns>
		public static IList<Route> MapRoutesInAssemblyOf<T>(this RouteCollection routeCollection)
		{
			return routeCollection.MapRoutesInAssembly(typeof(T).Assembly);
		}

		/// <summary>
		/// Searches an assembly for <see cref="RouteAttribute"/> declarations and maps the results in a
		/// specified route collection.
		/// </summary>
		/// <param name="routeCollection">The route collection.</param>
		/// <param name="assembly">The assembly to search.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="assembly"/> is null.</exception>
		/// <returns>A list of <see cref="Route"/> objects added to <paramref name="routeCollection"/>.</returns>
		public static IList<Route> MapRoutesInAssembly(this RouteCollection routeCollection, Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			return FindActionMethods(assembly)
				.Select(RouteBuilder.FromActionMethod)
				.Select(r => routeCollection.MapRoute(r))
				.ToList();
		}

		private static IEnumerable<MethodInfo> FindActionMethods(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(t => typeof(Controller).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods())
				.Where(m => m.HasRouteAttributes());
		}

		private static Route MapRoute(this RouteCollection routeCollection, RouteBuilder routeBuilder)
		{
			Route route = routeCollection.MapRoute(routeBuilder.Name, routeBuilder.Url);

			route.Defaults = routeBuilder.Defaults;
			route.Constraints = routeBuilder.Constraints;

			return route;
		}
	}
}
