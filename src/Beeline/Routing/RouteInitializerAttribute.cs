namespace Beeline.Routing
{
	using System;
	using System.Web.Routing;

	/// <summary>
	/// Indicates that a method will be used during route initialization.
	/// </summary>
	/// <remarks>
	/// A route initializer method must accept a <see cref="RouteCollection"/> as its only parameter.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class RouteInitializerAttribute : Attribute
	{
	}
}
