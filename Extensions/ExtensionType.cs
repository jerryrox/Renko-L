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

		/// <summary>
		/// Returns whether specified type is an anonymous type.
		/// </summary>
		public static bool IsAnonymous(this Type context) {
			string name = context.Name;
			return name.StartsWith("<>") && name.Contains("Anon");
		}
	}
}

