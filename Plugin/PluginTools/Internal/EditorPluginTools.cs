using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Renko.Plugin.Internal
{
	public class EditorPluginTools : IPluginTools {

		/// <summary>
		/// Initializes this tool.
		/// </summary>
		public void Initialize () {
		}

		/// <summary>
		/// Releases the resources used by this tool.
		/// </summary>
		public void Destroy () {
		}

		/// <summary>
		/// Saves the image located at specified path to the system gallery.
		/// </summary>
		public void SaveImageToGallery (string imagePath) {
		}

		/// <summary>
		/// Copies the specified text to the clipboard.
		/// </summary>
		public void CopyToClipboard (string text) {
			#if UNITY_EDITOR
			EditorGUIUtility.systemCopyBuffer = text;
			#endif
		}

		/// <summary>
		/// Removes network caches.
		/// </summary>
		public void RemoveNetworkCache() {
			// Nothing to do!
		}
	}
}

