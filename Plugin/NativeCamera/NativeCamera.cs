using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Debug;
using Renko.Plugin.Internal;

namespace Renko.Plugin
{
	/// <summary>
	/// A simple camera plugin that opens iOS/Android's native camera application to take a picture or video.
	/// </summary>
	public partial class NativeCamera : MonoBehaviour {

		public const string CancelCode = "cancel";
		public const string ErrorCode = "error";
		public const string EditorCode = "editor";

		private static NativeCamera I;

		/// <summary>
		/// The interface for platform-specific camera plugin object.
		/// </summary>
		private INativeCamera camPlugin;

		/// <summary>
		/// Callback for photo result
		/// </summary>
		private PhotoResultHandler onPhotoResult;

		/// <summary>
		/// Callback for video result
		/// </summary>
		private VideoResultHandler onVideoResult;


		/// <summary>
		/// Delegate for handling photo results.
		/// The result may contain any of the followings:
		/// 1. Path of the saved image
		/// 2. Return codes (Refer to NativeCamera.CancelCode/ErrorCode/EditorCode)
		/// </summary>
		public delegate void PhotoResultHandler(string result);

		/// <summary>
		/// Delegate for handling video results.
		/// The result may contain any of the followings:
		/// 1. Path of the saved image
		/// 2. Return codes (Refer to NativeCamera.CancelCode/ErrorCode/EditorCode)
		/// </summary>
		public delegate void VideoResultHandler(string result);


		/// <summary>
		/// Initializes the camera plugin.
		/// Will not do anything if already initialized.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;

			I = RenkoLibrary.CreateModule<NativeCamera>();
			SetupPlugin();
		}

		/// <summary>
		/// Releases the resources used by plugin.
		/// </summary>
		public static void Destroy() {
			if(I == null)
				return;
			I.camPlugin.Destroy();
			I = null;
		}

		/// <summary>
		/// Takes a new photo and saves at specified file path.
		/// On iOS, the photo will be saved to the library along with documents directory,
		/// as you can't directly access files in the photo library.
		/// </summary>
		public static void TakePhoto(SaveOption saveOptions, CropOption cropOptions, PhotoResultHandler callback) {
			if(I == null) {
				RenLog.Log(LogLevel.Warning, "NativeCamera.TakePhoto - You must initialize this module first!");
				return;
			}
			if(saveOptions == null) saveOptions = new SaveOption();
			if(cropOptions == null) cropOptions = new CropOption();

			I.onPhotoResult = callback;
			I.camPlugin.TakePhoto(saveOptions, cropOptions);
		}

		/// <summary>
		/// Takes a new video and saves at specified file path.
		/// On iOS, the photo will be saved to the library along with documents directory,
		/// as you can't directly access files in the photo library.
		/// </summary>
		public static void TakeVideo(SaveOption saveOptions, VideoOption videoOptions, VideoResultHandler callback) {
			if(I == null) {
				RenLog.Log(LogLevel.Warning, "NativeCamera.TakePhoto - You must initialize this module first!");
				return;
			}
			if(saveOptions == null) saveOptions = new SaveOption();
			if(videoOptions == null) videoOptions = new VideoOption();

			I.onVideoResult = callback;
			I.camPlugin.TakeVideo(saveOptions, videoOptions);
		}

		/// <summary>
		/// Setups the platform-specific plugin.
		/// </summary>
		private static void SetupPlugin() {
			switch(Application.platform) {
			case RuntimePlatform.Android:
				I.camPlugin = new AndroidCamera();
				break;
			case RuntimePlatform.IPhonePlayer:
				I.camPlugin = new IOSCamera();
				break;
			default:
				I.camPlugin = new EditorCamera();
				break;
			}

			I.camPlugin.Initialize(I.name);
		}

		/// <summary>
		/// Photo saved callback from camera plugin.
		/// </summary>
		void OnPhotoCallback(string path) {
			path = I.camPlugin.FinalizePhoto(path);
			if(onPhotoResult != null)
				onPhotoResult(path);
		}

		/// <summary>
		/// Video saved callback from camera plugin.
		/// </summary>
		void OnVideoCallback(string path) {
			path = I.camPlugin.FinalizeVideo(path);
			if(onVideoResult != null)
				onVideoResult(path);
		}
	}
}