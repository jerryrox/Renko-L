using System;

namespace Renko.Extensions
{
	public static class ExtensionObject {
		
		/// <summary>
		/// Returns whether this object is a numeric type.
		/// </summary>
		public static bool IsNumeric(this object value) {
			return value is sbyte
				|| value is byte
				|| value is short
				|| value is ushort
				|| value is int
				|| value is uint
				|| value is long
				|| value is ulong
				|| value is float
				|| value is double
				|| value is decimal;
		}
	}
}

