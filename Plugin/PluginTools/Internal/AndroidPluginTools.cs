using System;
using UnityEngine;

namespace Renko.Plugin.Internal
{
	public class AndroidPluginTools : IPluginTools {

		#if UNITY_ANDROID
		private AndroidJavaClass pluginClass;
		#endif


		/// <summary>
		/// Initializes this tool.
		/// </summary>
		public void Initialize () {
			#if UNITY_ANDROID
			pluginClass = new AndroidJavaClass("com.reisenmoe.renkol.PluginTools");
			#endif
		}

		/// <summary>
		/// Releases the resources used by this tool.
		/// </summary>
		public void Destroy () {
			#if UNITY_ANDROID
			pluginClass.Dispose();
			#endif
		}

		/// <summary>
		/// Saves the image located at specified path to the system gallery.
		/// </summary>
		public void SaveImageToGallery (string imagePath) {
			#if UNITY_ANDROID
			pluginClass.CallStatic("SaveImageToGallery", imagePath);
			#endif
		}

		/// <summary>
		/// Copies the specified text to the clipboard.
		/// </summary>
		public void CopyToClipboard (string text) {
			#if UNITY_ANDROID
			pluginClass.CallStatic("CopyToClipboard", text);
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

