namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Adds an MVC route definition to a controller action method.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="RouteAttribute"/> attribute provides an alternate way of specifying custom route
	/// definitions in ASP.NET MVC applications. Instead of defining routes in a central location using
	/// <see cref="RouteCollectionExtensions.MapRoute(RouteCollection,string,string)"/> or its overloads,
	/// routes are defined as attributes of the action methods to which they apply.
	/// </para>
	/// <para>
	/// Default values and constraints cannot be set using a <see cref="RouteAttribute"/> due to limitations
	/// in the .NET Framework, in which attribute parameters must be constant expressions.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class RouteAttribute : Attribute
	{
		private readonly String _urlPattern;

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteAttribute"/> class.
		/// </summary>
		/// <param name="urlPattern">The URL pattern for this route.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="urlPattern"/> is null.</exception>
		/// <remarks>
		///	<paramref name="urlPattern"/> follows the pattern conventions from <see cref="RouteCollectionExtensions.MapRoute(RouteCollection,string,string)"/>.
		///	For example, <c>"/Widgets/{id}"</c> adds a parameter named 'id' to the current route.
		/// </remarks>
		public RouteAttribute(String urlPattern)
		{
			if (urlPattern == null)
				throw new ArgumentNullException("urlPattern");

			_urlPattern = urlPattern;
		}

		/// <summary>
		/// Gets the URL pattern defined by this route.
		/// </summary>
		public String UrlPattern { get { return _urlPattern; } }

		/// <summary>
		/// Gets or sets the name of this route.
		/// </summary>
		/// <remarks>
		/// If unset, <see cref="Name"/> will default to <c>Verbs.ControllerName.ActionName</c>.
		/// </remarks>
		public String Name { get; set; }
	}
}
