using System;

namespace Renko.Extensions
{
	public static class ExtensionDateTime {

		/// <summary>
		/// Returns the number of seconds since 1970/01/01 00:00:00.
		/// </summary>
		public static long ToUnixTimestamp(this DateTime context) {
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan difference = context.ToUniversalTime() - origin;
			return (long)difference.TotalSeconds;
		}
	}
}

