namespace Beeline.Routing
{
	using System;

	/// <summary>
	/// Adds a default parameter value specification to an MVC action method.
	/// </summary>
	/// <remarks>
	/// A <see cref="DefaultAttribute"/> declaration must be used on a method that also has a
	/// <see cref="RouteAttribute"/> declaration.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class DefaultAttribute : Attribute
	{
		private readonly String _name;
		private readonly Object _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultAttribute"/> class.
		/// </summary>
		/// <param name="name">The URL parameter name.</param>
		/// <param name="value">The default value for the URL parameter</param>
		public DefaultAttribute(String name, Object value)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("A valid parameter name was expected.", "name");

			_name = name;
			_value = value;
		}

		/// <summary>
		/// Gets the parameter name for this default value.
		/// </summary>
		public String Name { get { return _name; } }

		/// <summary>
		/// Gets the default value for this parameter.
		/// </summary>
		public Object Value { get { return _value; } }
	}
}
