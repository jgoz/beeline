namespace Beeline.Tests.Specs
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;
	using Machine.Specifications;

	[Subject("Route initializers")]
	public class when_a_valid_initializer_method_exists : RouteInitializerSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_map_the_contained_route = () =>
			ByUrl("test/index/blah/42").ShouldNotBeNull();

		It should_have_the_specified_name = () =>
			ByName("index").ShouldNotBeNull();

		It should_handle_default_url_parameter_values = () =>
			ByUrl("test/index").Values["p1"].ShouldEqual("blah");

		It should_handle_optional_url_parameter_values = () =>
			ByUrl("test/index").Values["p2"].ShouldBeTheSameAs(UrlParameter.Optional);
	}

	[Subject("Route initializers")]
	public class when_an_initializer_method_has_zero_parameters : RouteInitializerSpecs
	{
		static Exception exception;

		Because of = () =>
			exception = Catch.Exception(() => routes.MapRoutesInController<ZeroParameterInitializer>());

		It should_abort_initialization = () =>
			exception.ShouldBeOfType<InvalidOperationException>();
	}

	[Subject("Route initializers")]
	public class when_an_initializer_method_has_two_or_more_parameters : RouteInitializerSpecs
	{
		static Exception exception;

		Because of = () =>
			exception = Catch.Exception(() => routes.MapRoutesInController<MultiParameterInitializer>());

		It should_abort_initialization = () =>
			exception.ShouldBeOfType<InvalidOperationException>();
	}

	[Subject("Route initializers")]
	public class when_an_initializer_method_is_not_static : RouteInitializerSpecs
	{
		static Exception exception;

		Because of = () =>
			exception = Catch.Exception(() => routes.MapRoutesInController<NonStaticInitializer>());

		It should_abort_initialization = () =>
			exception.ShouldBeOfType<InvalidOperationException>();
	}

	public class RouteInitializerSpecs : RoutingSpecContext
	{
		protected sealed class TestController : ControllerBase
		{
			[RouteInitializer]
			internal static void InitRoutes(RouteCollection routes)
			{
				routes.MapRoute(
					"index",
					"test/index/{p1}/{p2}",
					new { controller = "Test", action = "Index", p1 = "blah", p2 = UrlParameter.Optional });
			}

			public ActionResult Index(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}

		protected sealed class ZeroParameterInitializer : ControllerBase
		{
			[RouteInitializer]
			internal static void InitRoutes()
			{
			}

			public ActionResult Index(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}

		protected sealed class MultiParameterInitializer : ControllerBase
		{
			[RouteInitializer]
			internal static void InitRoutes(RouteCollection routes, Int32 thisWillMakeItBreak)
			{
			}

			public ActionResult Index(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}

		protected sealed class NonStaticInitializer : ControllerBase
		{
			[RouteInitializer]
			internal void InitRoutes(RouteCollection routes, Int32 thisWillMakeItBreak)
			{
			}

			public ActionResult Index(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}
	}
}
