namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;

	/// <summary>
	/// Extension methods for <see cref="RouteCollection"/>.
	/// </summary>
	public static class RouteCollectionExtensions
	{
		/// <summary>
		/// Maps a collection of <see cref="RouteAttribute"/> attributes as routes in a specified route collection.
		/// </summary>
		/// <param name="routeCollection">The route collection.</param>
		/// <param name="routeAttributes">The route attributes to map.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="routeAttributes"/> is null.</exception>
		/// <returns>A list of <see cref="Route"/> objects added to <paramref name="routeCollection"/>.</returns>
		public static IList<Route> MapRoutesFromAttributes(this RouteCollection routeCollection, IEnumerable<RouteAttribute> routeAttributes)
		{
			if (routeAttributes == null)
				throw new ArgumentNullException("routeAttributes");

			return routeAttributes
				.Select(MvcRoute.FromAttribute)
				.Select(r => routeCollection.MapRoute(r))
				.ToList();
		}

		private static Route MapRoute(this RouteCollection routeCollection, MvcRoute mvcRoute)
		{
			Route route = routeCollection.MapRoute(mvcRoute.Name, mvcRoute.Url);

			route.Defaults = mvcRoute.Defaults;
			route.Constraints = mvcRoute.Constraints;

			return route;
		}
	}
}
