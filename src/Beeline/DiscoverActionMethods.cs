namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;

	internal static class DiscoverActionMethods
	{
		public static IEnumerable<MethodInfo> InAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			return assembly.GetTypes()
				.Where(t => typeof(Controller).IsAssignableFrom(t))
				.SelectMany(t => t.GetMethods())
				.Routable();
		}
	}
}
