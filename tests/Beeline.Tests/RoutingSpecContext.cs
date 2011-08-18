namespace Beeline.Tests
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Machine.Specifications;

	public abstract class RoutingSpecContext
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
	}
}
