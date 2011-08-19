namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;

	/// <summary>
	/// Sets a URL parameter as optional for an MVC action method.
	/// </summary>
	/// <remarks>
	/// A <see cref="OptionalUrlParamAttribute"/> declaration must be used on a method that also has a
	/// <see cref="RouteAttribute"/> declaration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OptionalUrlParamAttribute : UrlParamAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OptionalUrlParamAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		public OptionalUrlParamAttribute(String name) : base(name, UrlParameter.Optional) { }
	}
}
