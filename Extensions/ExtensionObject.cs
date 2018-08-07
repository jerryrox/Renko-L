using System;

namespace Renko.Extensions
{
	public static class ExtensionObject {
		
		/// <summary>
		/// Returns whether this object is a numeric type.
		/// </summary>
		public static bool IsNumeric(this object context) {
			return context is sbyte
				|| context is byte
				|| context is short
				|| context is ushort
				|| context is int
				|| context is uint
				|| context is long
				|| context is ulong
				|| context is float
				|| context is double
				|| context is decimal;
		}

		/// <summary>
		/// Returns whether specified object is a type of anonymous type.
		/// </summary>
		public static bool IsAnonymous(this object context) {
			return context.GetType().IsAnonymous();
		}
	}
}

