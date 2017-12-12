using System;
using System.Runtime.InteropServices;

namespace Renko.Plugin.Internal
{
	public class IOSGallery : IGalleryPicker { 

		#if UNITY_IPHONE

		#endif


		public void Initialize (string objectName) {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}

		public void Destroy () {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}

		public void PickImage (CropOption cropOption) {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}

		public void PickVideo () {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}

		public void FinalizeImagePick (string result) {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}

		public void FinalizeVideoPick (string result) {
			#if UNITY_IPHONE
			throw new NotImplementedException ();
			#endif
		}
	}
}

