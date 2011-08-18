namespace Beeline.Tests
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Beeline.Routing;
	using Beeline.Tests.Helpers;
	using Machine.Specifications;

	[Subject("Route attributes")]
	public class when_only_a_url_pattern_is_specified : RoutingSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_get_requests_to_the_specified_url = () =>
			Get("test/basic").ShouldNotBeNull();

		It should_not_match_on_post_requests_to_the_specified_url = () =>
			Post("test/basic").ShouldBeNull();

		It should_not_match_on_put_requests_to_the_specified_url = () =>
			Put("test/basic").ShouldBeNull();

		It should_not_match_on_delete_requests_to_the_specified_url = () =>
			Delete("test/basic").ShouldBeNull();

		It should_add_a_route_to_the_collection_with_a_default_name = () =>
			ByName("Get.Test.Basic").ShouldNotBeNull();

		It should_set_the_correct_url_for_the_matching_named_route = () =>
			ByName("Get.Test.Basic").Url.ShouldEqual("test/basic");

		It should_use_the_same_route_for_a_url_as_the_matching_named_route = () =>
			Get("test/basic").Route.ShouldBeTheSameAs(ByName("Get.Test.Basic"));

		It should_route_to_the_correct_controller = () =>
			Get("test/basic").Values["controller"].ShouldEqual("Test");

		It should_route_to_the_correct_action = () =>
			Get("test/basic").Values["action"].ShouldEqual("Basic");

		It should_set_an_http_method_constraint = () =>
			ByName("Get.Test.Basic").ShouldContainConstraint<HttpMethodConstraint>();

		It should_constrain_the_http_method_to_get = () =>
			ByName("Get.Test.Basic").SingleConstraintOf<HttpMethodConstraint>().AllowedMethods.ShouldContainOnly("GET");
	}

	[Subject("Route attributes")]
	public class when_a_name_is_specified : RoutingSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_add_a_route_to_the_collection_with_the_specified_name = () =>
			ByName("Named").ShouldNotBeNull();

		It should_match_on_requests_to_the_specified_url = () =>
			Post("test/named").ShouldNotBeNull();

		It should_set_the_correct_url_for_the_matching_named_route = () =>
			ByName("Named").Url.ShouldEqual("test/named");

		It should_use_the_same_route_for_a_url_as_the_matching_named_route = () =>
			ByName("Named").ShouldBeTheSameAs(Post("test/named").Route);
	}

	[Subject("Route attributes")]
	public class when_the_action_method_responds_to_http_post_requests : RoutingSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_post_requests_to_the_specified_url = () =>
			Post("test/named").ShouldNotBeNull();

		It should_not_match_on_get_requests_to_the_specified_url = () =>
			Get("test/named").ShouldBeNull();

		It should_not_match_on_put_requests_to_the_specified_url = () =>
			Put("test/named").ShouldBeNull();

		It should_not_match_on_delete_requests_to_the_specified_url = () =>
			Delete("test/named").ShouldBeNull();

		It should_set_an_http_method_constraint = () =>
			ByName("Named").ShouldContainConstraint<HttpMethodConstraint>();

		It should_constrain_the_http_method_to_post = () =>
			ByName("Named").SingleConstraintOf<HttpMethodConstraint>().AllowedMethods.ShouldContainOnly("POST");
	}

	[Subject("Route attributes")]
	public class when_the_action_method_responds_to_http_get_and_post_requests : RoutingSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_get_requests_to_the_specified_url = () =>
			Get("test/getandpost").ShouldNotBeNull();

		It should_match_on_post_requests_to_the_specified_url = () =>
			Post("test/getandpost").ShouldNotBeNull();

		It should_not_match_on_put_requests_to_the_specified_url = () =>
			Put("test/getandpost").ShouldBeNull();

		It should_not_match_on_delete_requests_to_the_specified_url = () =>
			Delete("test/getandpost").ShouldBeNull();

		It should_set_an_http_method_constraint = () =>
			ByName("GetAndPost").ShouldContainConstraint<HttpMethodConstraint>();

		It should_constrain_the_http_method_to_get_and_post = () =>
			ByName("GetAndPost").SingleConstraintOf<HttpMethodConstraint>().AllowedMethods.ShouldContain("GET", "POST");
	}

	[Subject("Route attributes")]
	public class when_the_action_method_has_an_accept_verbs_attribute_with_put_and_delete : RoutingSpecs
	{
		Because of = () =>
			routes.MapRoutesInController<TestController>();

		It should_match_on_put_requests_to_the_specified_url = () =>
			Put("test/acceptverbs").ShouldNotBeNull();

		It should_match_on_delete_requests_to_the_specified_url = () =>
			Delete("test/acceptverbs").ShouldNotBeNull();

		It should_not_match_on_get_requests_to_the_specified_url = () =>
			Get("test/acceptverbs").ShouldBeNull();

		It should_not_match_on_post_requests_to_the_specified_url = () =>
			Post("test/acceptverbs").ShouldBeNull();

		It should_set_an_http_method_constraint = () =>
			ByName("AcceptVerbs").ShouldContainConstraint<HttpMethodConstraint>();

		It should_constrain_the_http_method_to_get_and_post = () =>
			ByName("AcceptVerbs").SingleConstraintOf<HttpMethodConstraint>().AllowedMethods.ShouldContain("PUT", "DELETE");
	}

	[Subject("Route attributes")]
	public class when_a_controller_method_has_a_non_action_attribute : RoutingSpecs
	{
		static Exception exception;

		Because of = () =>
			exception = Catch.Exception(() => routes.MapRoutesInAssemblyOf<TestNonActionController>());

		It should_not_allow_a_route_to_be_set = () =>
			exception.ShouldBeOfType<InvalidOperationException>();
	}

	public abstract class RoutingSpecs
	{
		protected static RouteCollection routes;

		Establish context = () =>
			routes = new RouteCollection();

		protected static RouteData ByUrl(String url, HttpVerbs method = HttpVerbs.Get)
		{
			return routes.GetRouteData(new HttpContextStub(requestUrl: "~/" + url, method: method));
		}

		protected static RouteData Get(String url)
		{
			return ByUrl(url);
		}

		protected static RouteData Post(String url)
		{
			return ByUrl(url, HttpVerbs.Post);
		}

		protected static RouteData Put(String url)
		{
			return ByUrl(url, HttpVerbs.Put);
		}

		protected static RouteData Delete(String url)
		{
			return ByUrl(url, HttpVerbs.Delete);
		}

		protected static Route ByName(String name)
		{
			return routes[name] as Route;
		}

		internal sealed class TestController : ControllerBase
		{
			[Route("test/basic")]
			public ActionResult Basic()
			{
				return new ViewResult();
			}

			[HttpPost, Route("test/named", Name = "Named")]
			public ActionResult Named()
			{
				return new ViewResult();
			}

			[HttpGet, HttpPost, Route("test/getandpost", Name = "GetAndPost")]
			public ActionResult GetAndPost()
			{
				return new ViewResult();
			}

			[AcceptVerbs(HttpVerbs.Put | HttpVerbs.Delete)]
			[Route("test/acceptverbs", Name = "AcceptVerbs")]
			public ActionResult AcceptVerbs()
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}

		internal sealed class TestNonActionController : ControllerBase
		{
			[NonAction, Route("test/nonaction")]
			public ActionResult NonAction()
			{
				return new ViewResult();
			}

			protected override void ExecuteCore()
			{
			}
		}
	}
}
