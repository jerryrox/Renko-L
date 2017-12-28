using System;

namespace Renko.Plugin.Internal
{
	public interface IGalleryPicker {

		/// <summary>
		/// Initializes the gallery picker.
		/// </summary>
		void Initialize(string objectName);

		/// <summary>
		/// Destroys the resources being used by this picker.
		/// </summary>
		void Destroy();

		/// <summary>
		/// Opens the native gallery for picking an image.
		/// </summary>
		void PickImage(CropOption cropOption);

		/// <summary>
		/// Opens the native gallery for picking a video.
		/// </summary>
		void PickVideo();

		/// <summary>
		/// Processes extra work before firing the image callback.
		/// Returns the final result.
		/// </summary>
		string FinalizeImagePick(string result);

		/// <summary>
		/// Processes extra work before firing the video callback.
		/// Returns the final result.
		/// </summary>
		string FinalizeVideoPick(string result);
	}
}

