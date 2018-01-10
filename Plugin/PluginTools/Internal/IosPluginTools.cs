using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Renko.Plugin.Internal
{
	public class IosPluginTools : IPluginTools {

		#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void _PluginTools_SaveImageToGallery(string imagePath);

		[DllImport("__Internal")]
		private static extern void _PluginTools_CopyToClipboard(string text);

		[DllImport("__Internal")]
		private static extern void _PluginTools_RemoveNetworkCache();
		#endif



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
			#if UNITY_IPHONE
			_PluginTools_SaveImageToGallery(imagePath);
			#endif
		}

		/// <summary>
		/// Copies the specified text to the clipboard.
		/// </summary>
		public void CopyToClipboard (string text) {
			#if UNITY_IPHONE
			_PluginTools_CopyToClipboard(text);
			#endif
		}

		/// <summary>
		/// Removes network caches.
		/// </summary>
		public void RemoveNetworkCache() {
			#if UNITY_IPHONE
			_PluginTools_RemoveNetworkCache();
			#endif
		}
	}
}

