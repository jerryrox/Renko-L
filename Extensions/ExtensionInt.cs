using System;

namespace Renko.Extensions
{
	public static class ExtensionInt {
		
		/// <summary>
		/// Returns whether this int is an even number.
		/// </summary>
		public static bool IsEven(this int context) {
			return (context % 2) == 0;
		}

		/// <summary>
		/// Returns whether (this value & specified value) is not 0.
		/// </summary>
		public static bool ContainsBit(this int context, int value) {
			return (context & value) != 0;
		}
	}
}

