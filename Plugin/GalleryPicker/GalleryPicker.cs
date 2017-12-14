using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Debug;
using Renko.Plugin.Internal;

namespace Renko.Plugin
{
	/// <summary>
	/// A simple plugin that allows users to pick an image or video and retrieve its path.
	/// </summary>
	public class GalleryPicker : MonoBehaviour {

		public const string CancelCode = "cancel";
		public const string ErrorCode = "error";

		private static GalleryPicker I;

		private IGalleryPicker galleryPlugin;

		private ImageResultHandler onImageResult;
		private VideoResultHandler onVideoResult;

		/// <summary>
		/// Delegate for handling image results.
		/// The result may contain any of the followings:
		/// 1. Path of the saved image
		/// 2. Return codes (Refer to GalleryPicker.CancelCode/ErrorCode)
		/// </summary>
		public delegate void ImageResultHandler(string result);

		/// <summary>
		/// Delegate for handling video results.
		/// The result may contain any of the followings:
		/// 1. Path of the saved image
		/// 2. Return codes (Refer to GalleryPicker.CancelCode/ErrorCode)
		/// </summary>
		public delegate void VideoResultHandler(string result);


		/// <summary>
		/// Initializes the gallery picker plugin.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;

			I = RenkoLibrary.CreateModule<GalleryPicker>();
			SetupPlugin();
		}

		/// <summary>
		/// Destroys the resources used by this plugin.
		/// </summary>
		public static void Destroy() {
			if(I == null)
				return;
			Destroy(I.gameObject);
			I = null;
		}

		/// <summary>
		/// Opens the native gallery for an image and returns its path to the specified callback.
		/// </summary>
		public static void PickImage(CropOption cropOption, ImageResultHandler callback) {
			if(I == null) {
				RenLog.Log(LogLevel.Warning, "GalleryPicker.PickImage - You must initialize this module first!");
				return;
			}
			I.onImageResult = callback;
			I.galleryPlugin.PickImage(cropOption);
		}

		/// <summary>
		/// Opens the native gallery for a video and returns its path to the specified callback.
		/// </summary>
		public static void PickVideo(VideoResultHandler callback) {
			if(I == null) {
				RenLog.Log(LogLevel.Warning, "GalleryPicker.PickVideo - You must initialize this module first!");
				return;
			}
			I.onVideoResult = callback;
			I.galleryPlugin.PickVideo();
		}

		/// <summary>
		/// Setups the platform-specific plugin.
		/// </summary>
		private static void SetupPlugin() {
			switch(Application.platform) {
			case RuntimePlatform.Android:
				I.galleryPlugin = new AndroidGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				I.galleryPlugin = new IOSGallery();
				break;
			default:
				I.galleryPlugin = new EditorGallery();
				break;
			}

			I.galleryPlugin.Initialize(I.name);
		}

		void OnImagePicked(string result) {
			I.galleryPlugin.FinalizeImagePick(result);
			if(I.onImageResult != null)
				I.onImageResult(result);
		}

		void OnVideoPicked(string result) {
			I.galleryPlugin.FinalizeVideoPick(result);
			if(I.onVideoResult != null)
				I.onVideoResult(result);
		}
	}
}