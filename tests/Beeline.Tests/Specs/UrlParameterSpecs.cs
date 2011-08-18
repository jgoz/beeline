namespace Beeline.Tests.Specs
{
	using System;
	using System.Web.Mvc;
	using Beeline.Routing;
	using Machine.Specifications;

	[Subject("Default parameter values")]
	public class when_all_parameters_have_default_values : UrlParameterSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_get_requests_to_the_full_url = () =>
			Get("test/all-defaults/blah/1").ShouldNotBeNull();

		It should_match_on_get_requests_to_a_partial_url = () =>
			Get("test/all-defaults/blah").ShouldNotBeNull();

		It should_match_on_get_requests_with_no_parameters = () =>
			Get("test/all-defaults").ShouldNotBeNull();

		It should_use_the_specified_default_values_for_the_first_parameter = () =>
			Get("test/all-defaults").Values["p1"].ShouldEqual("blank");

		It should_use_the_specified_default_values_for_the_second_parameter = () =>
			Get("test/all-defaults").Values["p2"].ShouldEqual(42);
	}

	[Subject("Default parameter values")]
	public class when_only_some_parameters_have_default_values : UrlParameterSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_get_requests_to_the_full_url = () =>
			Get("test/some-defaults/blah/1").ShouldNotBeNull();

		It should_match_on_get_requests_to_a_partial_url = () =>
			Get("test/some-defaults/blah").ShouldNotBeNull();

		It should_use_the_url_values_for_the_first_parameter = () =>
			Get("test/some-defaults/blah").Values["p1"].ShouldEqual("blah");

		It should_not_match_on_get_requests_to_the_root_url = () =>
			Get("test/some-defaults").ShouldBeNull();

		It should_use_the_specified_default_values_for_the_second_parameter = () =>
			Get("test/some-defaults/blah").Values["p2"].ShouldEqual(42);
	}

	[Subject("Default parameter values")]
	public class when_a_parameter_is_optional : UrlParameterSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_get_requests_to_the_full_url = () =>
			Get("test/with-optional/blah/1").ShouldNotBeNull();

		It should_match_on_get_requests_to_a_partial_url = () =>
			Get("test/with-optional/blah").ShouldNotBeNull();

		It should_not_match_on_get_requests_to_the_root_url = () =>
			Get("test/with-optional").ShouldBeNull();

		It should_use_mvc_optional_url_parameter_for_the_second_parameter = () =>
			Get("test/with-optional/blah").Values["p2"].ShouldBeTheSameAs(UrlParameter.Optional);
	}

	public abstract class UrlParameterSpecs : RoutingSpecContext
	{
		protected class TestController : ControllerBase
		{
			[Route("test/all-defaults/{p1}/{p2}"), Default("p1", "blank"), Default("p2", 42)]
			public ActionResult AllDefaults(String p1, Int32 p2)
			{
				return new ViewResult();
			}

			[Route("test/some-defaults/{p1}/{p2}"), Default("p2", 42)]
			public ActionResult SomeDefaults(String p1, Int32 p2)
			{
				return new ViewResult();
			}

			[Route("test/with-optional/{p1}/{p2}"), Optional("p2")]
			public ActionResult WithOptional(String p1, Int32? p2)
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}
	}
}
