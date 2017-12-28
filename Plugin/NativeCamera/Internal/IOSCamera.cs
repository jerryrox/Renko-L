using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Renko.Utility;
using Renko.Debug;

namespace Renko.Plugin.Internal
{
	/// <summary>
	/// Native camera module for iOS.
	/// </summary>
	public class IOSCamera : INativeCamera {

		#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void _NativeCamera_Initialize(string objectName);
		[DllImport("__Internal")]
		private static extern void _NativeCamera_Destroy();
		[DllImport("__Internal")]
		private static extern void _NativeCamera_TakePhoto(string saveOption, string cropOption);
		[DllImport("__Internal")]
		private static extern void _NativeCamera_TakeVideo(string saveOption, string videoOption);
		#endif

		private SaveOption lastSaveOption;
		private CropOption lastCropOption;


		/// <summary>
		/// Returns whether the photo save location should be altered to data path
		/// </summary>
		private bool ShoudAlterSaveLocation {
			get { return lastSaveOption.SaveToLibrary && lastCropOption.IsCropping; }
		}


		public void Initialize (string objectName) {
			#if UNITY_IPHONE
			_NativeCamera_Initialize(objectName);
			#endif
		}

		public void Destroy () {
			#if UNITY_IPHONE
			_NativeCamera_Destroy();
			#endif
		}

		public void TakePhoto (SaveOption saveOptions, CropOption cropOptions) {
			lastSaveOption = saveOptions;
			lastCropOption = cropOptions;

			if(ShoudAlterSaveLocation)
				AlterSaveLocation();
			
			#if UNITY_IPHONE
			_NativeCamera_TakePhoto(
				Json.ToString(new JsonData(saveOptions)),
				Json.ToString(new JsonData(cropOptions))
			);
			#endif
		}

		public void TakeVideo (SaveOption saveOptions, VideoOption videoOptions) {
			#if UNITY_IPHONE
			_NativeCamera_TakeVideo(
				Json.ToString(new JsonData(saveOptions)),
				Json.ToString(new JsonData(videoOptions))
			);
			#endif
		}

		public string FinalizePhoto(string filePath) {
			return filePath;
		}

		public string FinalizeVideo(string filePath) {
			return filePath;
		}

		void AlterSaveLocation() {
			lastSaveOption.SaveToLibrary = false;
		}
	}
}

