using System;

namespace Renko.Extensions
{
	public static class ExtensionByteArray {
		
		/// <summary>
		/// Returns a base64 representation of this byte array.
		/// </summary>
		public static string ToBase64String(this byte[] context) {
			return Convert.ToBase64String(context);
		}
	}
}

