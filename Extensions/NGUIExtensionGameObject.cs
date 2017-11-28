#if NGUI
using UnityEngine;

namespace Renko.Extensions
{
	public static class NGUIExtensionGameObject
	{
		/// <summary>
		/// Sets the UIPanel component's rect.
		/// </summary>
		public static void SetPanelRect(this GameObject context, float x, float y, float width, float height) {
			context.GetComponent<UIPanel>().SetRect(x, y, width, height);
		}

		/// <summary>
		/// Sets the UIPanel component's rect.
		/// </summary>
		public static void SetPanelRect(this GameObject context, Vector4 bounds) {
			context.GetComponent<UIPanel>().SetRect(bounds.x, bounds.y, bounds.z+2, bounds.w);
		}

		/// <summary>
		/// Returns the UIPanel component's rect.
		/// </summary>
		public static Vector4 GetPanelRect(this GameObject context) {
			return context.GetComponent<UIPanel>().finalClipRegion;
		}
	}
}
#endif