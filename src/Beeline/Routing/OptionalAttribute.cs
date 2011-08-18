namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;

	/// <summary>
	/// Sets a URL parameter as optional for an MVC action method.
	/// </summary>
	/// <remarks>
	/// A <see cref="OptionalAttribute"/> declaration must be used on a method that also has a
	/// <see cref="RouteAttribute"/> declaration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OptionalAttribute : DefaultAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OptionalAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		public OptionalAttribute(String name) : base(name, UrlParameter.Optional) { }
	}
}
