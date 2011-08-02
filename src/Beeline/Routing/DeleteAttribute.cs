namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;

	/// <summary>
	/// Adds an MVC route definition using the HTTP DELETE method to a controller action method.
	/// </summary>
	/// <remarks>
	/// This class provides a clearer and more convenient way to define routes that use the DELETE method.
	/// See <see cref="RouteAttribute"/> for full documentation.
	/// </remarks>
	/// <seealso cref="RouteAttribute"/>
	public class DeleteAttribute : RouteAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteAttribute"/> class.
		/// </summary>
		/// <param name="urlPattern">The URL pattern for this route.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="urlPattern"/> is null.</exception>
		public DeleteAttribute(String urlPattern) : base(HttpVerbs.Delete, urlPattern) { }
	}
}
