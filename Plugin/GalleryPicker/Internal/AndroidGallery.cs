using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Plugin.Internal
{
	public class AndroidGallery : IGalleryPicker {

		#if UNITY_ANDROID
		private AndroidJavaClass pluginClass;
		#endif


		public void Initialize (string objectName) {
			#if UNITY_ANDROID
			pluginClass = new AndroidJavaClass("com.reisenmoe.renkol.GalleryPicker");
			pluginClass.CallStatic("Initialize", objectName);
			#endif
		}

		public void Destroy () {
			#if UNITY_ANDROID
			pluginClass.CallStatic("Destroy");
			pluginClass.Dispose();
			#endif
		}

		public void PickImage (CropOption cropOption) {
			#if UNITY_ANDROID
			pluginClass.CallStatic(
				"PickImage",
				Json.ToString(new JsonData(cropOption))
			);
			#endif
		}

		public void PickVideo () {
			#if UNITY_ANDROID
			pluginClass.CallStatic("PickVideo");
			#endif
		}

		public void FinalizeImagePick (string result) {
			#if UNITY_ANDROID
			#endif
		}

		public void FinalizeVideoPick (string result) {
			#if UNITY_ANDROID
			#endif
		}
	}
}

