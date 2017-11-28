using UnityEngine;

namespace Renko.Extensions
{
	public static class ExtensionComponent {
		
		/// <summary>
		/// Returns the component from child at given index.
		/// </summary>
		public static T GetComponent<T>(this Component context, int childInx) where T : Component {
			return context.transform.GetChild(childInx).GetComponent<T>();
		}
	}
}

