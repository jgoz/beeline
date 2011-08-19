namespace Beeline.Tests.Specs
{
	using System;
	using System.Web.Mvc;
	using Beeline.Routing;
	using Machine.Specifications;

	[Subject("URL parameters")]
	public class when_all_parameters_have_default_values : UrlParamSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_requests_to_the_full_url = () =>
			Get("test/all-defaults/blah/1").ShouldNotBeNull();

		It should_match_on_requests_to_a_partial_url = () =>
			Get("test/all-defaults/blah").ShouldNotBeNull();

		It should_match_on_requests_with_no_parameters = () =>
			Get("test/all-defaults").ShouldNotBeNull();

		It should_use_the_specified_default_values_for_the_first_parameter = () =>
			Get("test/all-defaults").Values["p1"].ShouldEqual("blank");

		It should_use_the_specified_default_values_for_the_second_parameter = () =>
			Get("test/all-defaults").Values["p2"].ShouldEqual(42);
	}

	[Subject("URL parameters")]
	public class when_only_some_parameters_have_default_values : UrlParamSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_requests_to_the_full_url = () =>
			Get("test/some-defaults/blah/1").ShouldNotBeNull();

		It should_match_on_requests_to_a_partial_url = () =>
			Get("test/some-defaults/blah").ShouldNotBeNull();

		It should_use_the_url_values_for_the_first_parameter = () =>
			Get("test/some-defaults/blah").Values["p1"].ShouldEqual("blah");

		It should_not_match_on_requests_to_the_root_url = () =>
			Get("test/some-defaults").ShouldBeNull();

		It should_use_the_specified_default_values_for_the_second_parameter = () =>
			Get("test/some-defaults/blah").Values["p2"].ShouldEqual(42);
	}

	[Subject("URL parameters")]
	public class when_a_parameter_is_optional : UrlParamSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_requests_to_the_full_url = () =>
			Get("test/with-optional/blah/1").ShouldNotBeNull();

		It should_match_on_requests_to_a_partial_url = () =>
			Get("test/with-optional/blah").ShouldNotBeNull();

		It should_not_match_on_requests_to_the_root_url = () =>
			Get("test/with-optional").ShouldBeNull();

		It should_use_mvc_optional_url_parameter_for_the_second_parameter = () =>
			Get("test/with-optional/blah").Values["p2"].ShouldBeTheSameAs(UrlParameter.Optional);
	}

	[Subject("URL parameters")]
	public class when_a_parameter_is_constrained : UrlParamSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_requests_to_a_valid_url = () =>
			Get("test/with-constraint/blah/11").ShouldNotBeNull();

		It should_not_match_on_requests_to_an_invalid_url = () =>
			Get("test/with-constraint/blah/1a").ShouldBeNull();
	}

	public abstract class UrlParamSpecs : RoutingSpecContext
	{
		protected class TestController : ControllerBase
		{
			[Route("test/all-defaults/{p1}/{p2}"), UrlParam("p1", "blank"), UrlParam("p2", 42)]
			public ActionResult AllDefaults(String p1, Int32 p2)
			{
				return new ViewResult();
			}

			[Route("test/some-defaults/{p1}/{p2}"), UrlParam("p2", 42)]
			public ActionResult SomeDefaults(String p1, Int32 p2)
			{
				return new ViewResult();
			}

			[Route("test/with-optional/{p1}/{p2}"), OptionalUrlParam("p2")]
			public ActionResult WithOptional(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			[Route("test/with-constraint/{p1}/{p2}"), UrlParam("p2", Constraint = @"\d+")]
			public ActionResult WithConstraint(String p1, Int32 p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}
	}
}
