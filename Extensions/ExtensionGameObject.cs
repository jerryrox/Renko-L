using UnityEngine;

namespace Renko.Extensions
{
	public static class ExtensionGameObject {
		
		/// <summary>
		/// Returns the full name (path) of this object.
		/// </summary>
		public static string GetFullName(this GameObject obj){
			string fullName = obj.name;
			Transform tm = obj.transform;
			while(tm.parent != null) {
				tm = tm.parent;
				fullName = tm.name + '/' + fullName;
			}
			return fullName;
		}
	}
}

