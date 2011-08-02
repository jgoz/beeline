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
				.SelectMany(t => t.GetMethods(BindingFlags.Public));

			return actionMethods.SelectMany(GetActionRouteAttributes);
		}

		private static IEnumerable<RouteAttribute> GetActionRouteAttributes(MethodInfo method)
		{
			return method.GetCustomAttributes(typeof(RouteAttribute), false)
				.Cast<RouteAttribute>()
				.Select(r => new RouteAttribute(r, method.Name, method.DeclaringType.Name.Replace("Controller", String.Empty)));
		}

		/// <summary>
		/// Enumerates all of the route attributes in a given type's containing assembly.
		/// </summary>
		/// <typeparam name="T">A type in the assembly that will be searched.</typeparam>
		/// <returns>An <see cref="IEnumerable{RouteAttribute}"/> containing all of the route attributes in the assembly.</returns>
		public static IEnumerable<RouteAttribute> InAssemblyOf<T>()
		{
			return InAssembly(typeof(T).Assembly);
		}
	}
}
