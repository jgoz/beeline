namespace Beeline.Routing
{
	using System;

	/// <summary>
	/// Sets a URL parameter as optional for an MVC action method.
	/// </summary>
	/// <remarks>
	/// A <see cref="OptionalAttribute"/> declaration must be used on a method that also has a
	/// <see cref="RouteAttribute"/> declaration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OptionalAttribute : Attribute
	{
		private readonly String _name;

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionalAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		public OptionalAttribute(String name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("A valid parameter name was expected.", "name");

			_name = name;
		}

		/// <summary>
		/// Gets the parameter name for this optional value.
		/// </summary>
		public String Name { get { return _name; } }
	}
}
