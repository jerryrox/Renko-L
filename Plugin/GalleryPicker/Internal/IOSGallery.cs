using System;
using System.Runtime.InteropServices;
using Renko.Data;

namespace Renko.Plugin.Internal
{
	public class IOSGallery : IGalleryPicker { 

		#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void _GalleryPicker_Initialize(string objectName);
		[DllImport("__Internal")]
		private static extern void _GalleryPicker_Destroy();
		[DllImport("__Internal")]
		private static extern void _GalleryPicker_PickImage(string targetPath, string cropOption);
		[DllImport("__Internal")]
		private static extern void _GalleryPicker_PickVideo(string targetPath);
		#endif


		public void Initialize (string objectName) {
			#if UNITY_IPHONE
			_GalleryPicker_Initialize(objectName);
			#endif
		}

		public void Destroy () {
			#if UNITY_IPHONE
			_GalleryPicker_Destroy();
			#endif
		}

		public void PickImage (CropOption cropOption) {
			#if UNITY_IPHONE
			_GalleryPicker_PickImage(
				new SaveOption().FileName,
				new JsonData(cropOption).ToString()
			);
			#endif
		}

		public void PickVideo () {
			#if UNITY_IPHONE
			_GalleryPicker_PickVideo(
				new SaveOption().FileName
			);
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

