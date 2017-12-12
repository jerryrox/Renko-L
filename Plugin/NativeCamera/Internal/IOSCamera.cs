using System;
using System.Runtime.InteropServices;

namespace Renko.Plugin.Internal
{
	public class IOSCamera : INativeCamera {
		
		public void Initialize (string objectName) {
			throw new NotImplementedException ();
		}

		public void Destroy () {
			throw new NotImplementedException ();
		}

		public void TakePhoto (SaveOption saveOptions, CropOption cropOptions) {
			throw new NotImplementedException ();
		}

		public void TakeVideo (SaveOption saveOptions, VideoOption videoOptions) {
			throw new NotImplementedException ();
		}

		public void FinalizePhoto(string filePath) {

		}

		public void FinalizeVideo(string filePath) {

		}
	}
}

