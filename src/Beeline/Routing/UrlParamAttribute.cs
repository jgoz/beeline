namespace Beeline.Routing
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Adds a URL parameter specification to an MVC action method.
	/// </summary>
	/// <remarks>
	/// A <see cref="UrlParamAttribute"/> declaration must be used on a method that also has a
	/// <see cref="RouteAttribute"/> declaration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class UrlParamAttribute : Attribute
	{
		private readonly String _name;
		private readonly Object _default;

		/// <summary>
		/// Initializes a new instance of the <see cref="UrlParamAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		public UrlParamAttribute(String name) : this(name, null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="UrlParamAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		/// <param name="default">The default value.</param>
		public UrlParamAttribute(String name, Object @default)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("A valid parameter name was expected.", "name");

			_name = name;
			_default = @default;
		}

		/// <summary>
		/// Gets the URL parameter name.
		/// </summary>
		public String Name { get { return _name; } }

		/// <summary>
		/// Gets the default value for this parameter.
		/// </summary>
		public Object Default { get { return _default; } }

		/// <summary>
		/// Gets or sets the regular expression constraint for this parameter.
		/// </summary>
		/// <remarks>
		/// Only regular expression constraints are supported by <see cref="UrlParamAttribute"/>. To add
		/// <see cref="IRouteConstraint"/>-derived constraints, use
		/// <see cref="RouteCollectionExtensions.MapRoute(RouteCollection,string,string,object,object)"/>.
		/// </remarks>
		public String Constraint { get; set; }
	}
}
