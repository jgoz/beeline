namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;

	/// <summary>
	/// Adds an MVC route definition using the HTTP POST method to a controller action method.
	/// </summary>
	/// <remarks>
	/// This class provides a clearer and more convenient way to define routes that use the POST method.
	/// See <see cref="RouteAttribute"/> for full documentation.
	/// </remarks>
	/// <seealso cref="RouteAttribute"/>
	public class PostAttribute : RouteAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PostAttribute"/> class.
		/// </summary>
		/// <param name="urlPattern">The URL pattern for this route.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="urlPattern"/> is null.</exception>
		public PostAttribute(String urlPattern) : base(HttpVerbs.Get, urlPattern) { }
	}
}
