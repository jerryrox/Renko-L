using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Plugin.Internal
{
	public interface IPluginTools {

		/// <summary>
		/// Initializes this tool.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Releases the resources used by this tool.
		/// </summary>
		void Destroy();

		/// <summary>
		/// Saves the image located at specified path to the system gallery.
		/// </summary>
		void SaveImageToGallery(string imagePath);

		/// <summary>
		/// Copies the specified text to the clipboard.
		/// </summary>
		void CopyToClipboard(string text);

		/// <summary>
		/// Removes network caches.
		/// </summary>
		void RemoveNetworkCache();
	}
}