namespace Beeline
{
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Web.Routing;

	internal static class RouteInitializerMethod
	{
		public static void Invoke(MethodInfo initializer, RouteCollection routeCollection)
		{
			ValidateRouteInitializer(initializer);

			initializer.Invoke(null, new[] { routeCollection });
		}

		private static void ValidateRouteInitializer(MethodInfo initializer)
		{
			var parameters = initializer.GetParameters();

			if (parameters.Count() != 1 || !typeof(RouteCollection).IsAssignableFrom(parameters.Single().ParameterType))
				throw new InvalidOperationException(GetExceptionMessage(initializer, "must accept a RouteCollection as its only parameter."));

			if (!initializer.IsStatic)
				throw new InvalidOperationException(GetExceptionMessage(initializer, "must be a static method."));
		}

		private static String GetExceptionMessage(MethodInfo initializer, String errorMessage)
		{
			return "Route initializer '" + initializer.Name + "' in controller '" + initializer.DeclaringType.Name + "' " + errorMessage;
		}
	}
}
