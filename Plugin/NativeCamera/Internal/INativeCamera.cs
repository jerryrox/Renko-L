using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Plugin
{
	public interface INativeCamera {

		/// <summary>
		/// Initializes the camera plugin.
		/// </summary>
		void Initialize(string objectName);

		/// <summary>
		/// Destroys the camera plugin.
		/// </summary>
		void Destroy();

		/// <summary>
		/// Takes a new photo and saves at specified file path.
		/// </summary>
		void TakePhoto(SaveOption saveOption, CropOption cropOption);

		/// <summary>
		/// Takes a new video and saves at specified file path.
		/// </summary>
		void TakeVideo(SaveOption saveOption, VideoOption videoOption);

		/// <summary>
		/// Processes extra work before firing a photo callback.
		/// </summary>
		void FinalizePhoto(string filePath);

		/// <summary>
		/// Processes extra work before firing a photo callback.
		/// </summary>
		void FinalizeVideo(string filePath);
	}
}