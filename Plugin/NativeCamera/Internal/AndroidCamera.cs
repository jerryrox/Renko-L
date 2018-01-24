using System.Collections;
using System.IO;
using System;
using UnityEngine;
using Renko.Extensions;
using Renko.Diagnostics;
using Renko.Utility;

namespace Renko.Plugin.Internal
{
	/// <summary>
	/// Native camera module for Android.
	/// </summary>
	public class AndroidCamera : INativeCamera {

		#if UNITY_ANDROID
		private AndroidJavaClass camClass;
		#endif

		private SaveOption lastSaveOption;
		private CropOption lastCropOption;


		public void Initialize (string objectName) {
			#if UNITY_ANDROID
			camClass = new AndroidJavaClass("com.reisenmoe.renkol.NativeCamera");
			camClass.CallStatic("Initialize", objectName);
			#endif
		}

		public void Destroy () {
			#if UNITY_ANDROID
			camClass.CallStatic("Destroy");
			camClass.Dispose();
			#endif
		}

		public void TakePhoto (SaveOption saveOption, CropOption cropOption) {
			lastSaveOption = saveOption;
			lastCropOption = cropOption;
			#if UNITY_ANDROID
			camClass.CallStatic(
				"TakePhoto",
				Json.ToString(new JsonData(saveOption)),
				Json.ToString(new JsonData(cropOption))
			);
			#endif
		}

		public void TakeVideo (SaveOption saveOption, VideoOption videoOption) {
			#if UNITY_ANDROID
			camClass.CallStatic(
				"TakeVideo",
				Json.ToString(new JsonData(saveOption)),
				Json.ToString(new JsonData(videoOption))
			);
			#endif
		}

		public string FinalizePhoto(string filePath) {
			// If the photo is saved in persistent datapath and image should be cropped,
			// We must get rid of the original image before cropping.
			if(lastSaveOption != null && lastCropOption != null &&
				!lastSaveOption.SaveToLibrary && lastCropOption.IsCropping) {
				string originalPath = string.Format(
					"{0}/{1}.jpg",
					Application.persistentDataPath,
					lastSaveOption.FileName
				);
				if(File.Exists(originalPath)) {
					RenLog.Log(LogLevel.Info, "AndroidCamera.FinalizePhoto - Removing original photo: " + originalPath);
					File.Delete(originalPath);
				}
			}
			return filePath;
		}

		public string FinalizeVideo(string filePath) {
			return filePath;
		}
	}
}

