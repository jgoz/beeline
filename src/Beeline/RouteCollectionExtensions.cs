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
		private const BindingFlags RouteInitializerFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

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

			routeCollection.InvokeRouteInitializers(FindInitializerMethods(assembly));

			return routeCollection.MapActionMethodsToRoutes(FindActionMethods(assembly));
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
			routeCollection.InvokeRouteInitializers(FindInitializerMethods(typeof(TController)));

			return routeCollection.MapActionMethodsToRoutes(FindActionMethods(typeof(TController)));
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

		private static IEnumerable<MethodInfo> FindActionMethods(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(t => typeof(ControllerBase).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods())
				.Where(m => m.HasRouteAttributes());
		}

		private static IEnumerable<MethodInfo> FindActionMethods(Type controllerType)
		{
			return controllerType.GetMethods().Where(m => m.HasRouteAttributes());
		}

		private static IEnumerable<MethodInfo> FindInitializerMethods(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(t => typeof(ControllerBase).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods(RouteInitializerFlags))
				.Where(m => m.IsRouteInitializer());
		}

		private static IEnumerable<MethodInfo> FindInitializerMethods(Type controllerType)
		{
			return controllerType
				.GetMethods(RouteInitializerFlags)
				.Where(m => m.IsRouteInitializer());
		}
	}
}
