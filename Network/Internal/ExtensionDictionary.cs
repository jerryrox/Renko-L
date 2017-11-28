using System.Collections.Generic;
using System;

namespace Renko.Network.Internal
{
	public static class ExtensionDictionary {

		/// <summary>
		/// Parses response code from this dictionary.
		/// The dictionary must represent a collection of response headers.
		/// Returns 0 if any invalid condition is met.
		/// </summary>
		public static long GetResponseCode(this Dictionary<string,string> context) {
			if(context == null)
				return 0;
			if(!context.ContainsKey("STATUS"))
				return 0;
			
			string[] components = context["STATUS"].Split(' ');
			if (components.Length < 3)
				return 0;

			long ret = 0;
			if (!long.TryParse(components[1], out ret))
				return 0;
			return ret;
		}
	}
}

