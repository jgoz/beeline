namespace Beeline
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	internal static class EnumExtensions
	{
		public static IEnumerable<TEnum> GetFlagsValues<TEnum>(this Enum @enum)
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("Expected enumeration for type argument TEnum.");

			return Enum.GetValues(typeof(TEnum))
				.Cast<Enum>()
				.Where(@enum.HasFlag)
				.Cast<TEnum>();
		}
	}
}
