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

			routeCollection.InvokeRouteInitializers(FindControllerMethods(assembly, m => m.IsRouteInitializer()));

			return routeCollection.MapActionMethodsToRoutes(FindControllerMethods(assembly, m => m.HasRouteAttributes()));
		}

		/// <summary>
		/// Finds action methods with <see cref="RouteAttribute"/> declarations in a specified controller type
		/// and maps the results in a specified route collection.
		/// </summary>
		/// <typeparam name="TController">The controller type. Must inherit from <see cref="ControllerBase"/></typeparam>
		/// <param name="routeCollection">The route collection.</param>
		/// <returns>A list of <see cref="Route"/> objects added to <paramref name="routeCollection"/>.</returns>
		public static IList<Route> MapRoutesInController<TController>(this RouteCollection routeCollection) where TController : ControllerBase
		{
			routeCollection.InvokeRouteInitializers(FindControllerMethods(typeof(TController), m => m.IsRouteInitializer()));

			return routeCollection.MapActionMethodsToRoutes(FindControllerMethods(typeof(TController), m => m.HasRouteAttributes()));
		}

		private static void InvokeRouteInitializers(this RouteCollection routeCollection, IEnumerable<MethodInfo> routeInitializers)
		{
			foreach (MethodInfo initializer in routeInitializers)
				RouteInitializerMethod.Invoke(initializer, routeCollection);
		}

		private static IList<Route> MapActionMethodsToRoutes(this RouteCollection routeCollection, IEnumerable<MethodInfo> actionMethods)
		{
			return actionMethods
				.Select(RouteBuilder.FromActionMethod)
				.Select(r => routeCollection.MapRouteBuilder(r))
				.ToList();
		}

		private static Route MapRouteBuilder(this RouteCollection routeCollection, RouteBuilder routeBuilder)
		{
			Route route = routeCollection.MapRoute(routeBuilder.Name, routeBuilder.Url);

			route.Defaults = routeBuilder.Defaults;
			route.Constraints = routeBuilder.Constraints;

			return route;
		}

		private static IEnumerable<MethodInfo> FindControllerMethods(Assembly assembly, Func<MethodInfo, Boolean> predicate)
		{
			return assembly.GetTypes()
				.Where(t => typeof(ControllerBase).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods())
				.Where(predicate);
		}

		private static IEnumerable<MethodInfo> FindControllerMethods(Type controllerType, Func<MethodInfo, Boolean> predicate)
		{
			return controllerType.GetMethods().Where(predicate);
		}
	}
}
