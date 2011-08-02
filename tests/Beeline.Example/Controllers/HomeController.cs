namespace Beeline.Example.Controllers
{
	using System.Web.Mvc;
	using Beeline.Routing;

	public class HomeController : Controller
	{
		[Route("")]
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		[Route("About")]
		public ActionResult About()
		{
			return View();
		}
	}
}
