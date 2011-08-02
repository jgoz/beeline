namespace Beeline.Example.Controllers
{
	using System.Web.Mvc;
	using Beeline.Routing;

	public class HomeController : Controller
	{
		[Get("")]
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		[Get("About")]
		public ActionResult About()
		{
			return View();
		}
	}
}
