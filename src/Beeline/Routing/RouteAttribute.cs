namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Adds an MVC route definition to a controller action method.
	/// </summary>
	/// <remarks>
	/// The <see cref="RouteAttribute"/> attribute provides an alternate way of specifying custom route
	/// definitions in ASP.NET MVC applications. Instead of defining routes in a central location using
	/// <see cref="RouteCollectionExtensions.MapRoute(RouteCollection,string,string)"/> or its overloads,
	/// routes are defined as attributes of the action methods to which they apply.
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
				throw new ArgumentNullException("urlPattern", "A URL pattern is required for a route definition.");

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

		/// <summary>
		/// Gets or sets the default values for any defined route parameters. May be null.
		/// </summary>
		public Object Defaults { get; set; }

		/// <summary>
		/// Gets or sets an object that defines the parameter constraints for this route. May be null.
		/// </summary>
		/// <remarks>
		/// <para>
		///	If set, the properties defined in the <see cref="Constraints"/> object will be evaluated during route matching.
		///	If a constraint property name matches a URL parameter defined in <see cref="UrlPattern"/>, the constraint will only apply
		///	to the matching parameter. If the property name does not match any URL parameters, the constraint will apply to the entire route.
		/// </para>
		/// <para>
		///	Constrant properties may be defined as strings, which will be evaluated as regular expressions, or they may be
		///	defined as instances of classes that implement <see cref="IRouteConstraint"/>.
		/// </para>
		/// </remarks>
		public Object Constraints { get; set; }
	}
}
