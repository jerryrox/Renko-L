using System;
using UnityEngine;

namespace RenkoEditor
{
	/// <summary>
	/// Helper class for setting colors on Editor GUI elements.
	/// </summary>
	public static class EditorColors {

		private static Color? CachedBackgroundColor;
		private static Color? CachedColor;
		private static Color? CachedContentColor;


		/// <summary>
		/// Changes the current background color to specified value.
		/// </summary>
		public static void SetBackgroundColor(Color color) {
			if(!CachedBackgroundColor.HasValue)
				CachedBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
		}

		/// <summary>
		/// Resets the current background color to original value.
		/// </summary>
		public static void ResetBackgroundColor() {
			GUI.backgroundColor = CachedBackgroundColor.Value;
		}

		/// <summary>
		/// Changes the current global tint color to specified value.
		/// </summary>
		public static void SetColor(Color color) {
			if(!CachedColor.HasValue)
				CachedColor = GUI.color;
			GUI.color = color;
		}

		/// <summary>
		/// Resets the current global tint color to original value.
		/// </summary>
		public static void ResetColor() {
			GUI.color = CachedColor.Value;
		}

		/// <summary>
		/// Changes the current content color to specified value.
		/// </summary>
		public static void SetContentColor(Color color) {
			if(!CachedContentColor.HasValue)
				CachedContentColor = GUI.contentColor;
			GUI.contentColor = color;
		}

		/// <summary>
		/// Resets the current content color to original value.
		/// </summary>
		public static void ResetContentColor() {
			GUI.contentColor = CachedContentColor.Value;
		}
	}
}

