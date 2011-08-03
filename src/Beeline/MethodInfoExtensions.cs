namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Beeline.Routing;

	internal static class MethodInfoExtensions
	{
		public static IEnumerable<MethodInfo> Routable(this IEnumerable<MethodInfo> collection)
		{
			return collection.Where(IsRoutable);
		}

		private static Boolean IsRoutable(MethodInfo methodInfo)
		{
			return methodInfo.GetRouteAttributes().Any();
		}

		public static IEnumerable<RouteAttribute> GetRouteAttributes(this MethodInfo methodInfo)
		{
			return methodInfo.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>();
		}
	}
}
