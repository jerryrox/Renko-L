using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Plugin.Internal
{
	public class EditorCamera : INativeCamera {
		
		/// <summary>
		/// Dummy object for SendMessage.
		/// </summary>
		private GameObject messageTarget;


		public void Initialize (string objectName) {
			messageTarget = GameObject.Find(objectName);
		}

		public void Destroy () {
			GameObject.Destroy(messageTarget);
		}

		public void TakePhoto (SaveOption saveOptions, CropOption cropOptions) {
			Timer.CreateDelay(0.5f, 0, delegate(Timer.Item item) {
				messageTarget.SendMessage("OnPhotoCallback", NativeCamera.EditorCode);
			});
		}

		public void TakeVideo (SaveOption saveOptions, VideoOption videoOptions) {
			Timer.CreateDelay(0.5f, 0, delegate(Timer.Item item) {
				messageTarget.SendMessage("OnVideoCallback", NativeCamera.EditorCode);
			});
		}

		public string FinalizePhoto(string filePath) {
			return filePath;
		}

		public string FinalizeVideo(string filePath) {
			return filePath;
		}
	}
}

