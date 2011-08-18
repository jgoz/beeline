namespace Beeline.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Routing;
	using Machine.Specifications;

	public static class RouteExtensions
	{
		public static void ShouldContainConstraint<TConstraint>(this Route route) where TConstraint : IRouteConstraint
		{
			route.Constraints.Values.ShouldContain(c => c.GetType() == typeof(TConstraint));
		}

		public static IEnumerable<TConstraint> ConstraintsOf<TConstraint>(this Route route) where TConstraint : IRouteConstraint
		{
			return route.Constraints.Values.Where(c => c.GetType() == typeof(TConstraint)).Cast<TConstraint>();
		}

		public static TConstraint SingleConstraintOf<TConstraint>(this Route route) where TConstraint : IRouteConstraint
		{
			return route.ConstraintsOf<TConstraint>().Single();
		}
	}
}
