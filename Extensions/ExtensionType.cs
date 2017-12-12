using System;

namespace Renko.Extensions
{
	public static class ExtensionType {

		/// <summary>
		/// Returns the name of this type without base namespaces or parent class.
		/// </summary>
		public static string GetName(this Type context) {
			string name = context.Name;
			return name.Substring(name.LastIndexOf('.') + 1);
		}
	}
}

