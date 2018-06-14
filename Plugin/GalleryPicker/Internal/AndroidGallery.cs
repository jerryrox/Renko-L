using System;
using UnityEngine;
using Renko.Data;

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
				new JsonData(cropOption).ToString()
			);
			#endif
		}

		public void PickVideo () {
			#if UNITY_ANDROID
			pluginClass.CallStatic("PickVideo");
			#endif
		}

		public string FinalizeImagePick (string result) {
			return result;
		}

		public string FinalizeVideoPick (string result) {
			return result;
		}
	}
}

