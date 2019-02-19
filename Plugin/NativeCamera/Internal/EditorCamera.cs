using System;
using UnityEngine;
using Renko.Utility;
using Renko.LapseFramework;

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
			Timer.CreateDelay(delegate() {
				messageTarget.SendMessage("OnPhotoCallback", NativeCamera.EditorCode);
			}, 0.5f).Start();
		}

		public void TakeVideo (SaveOption saveOptions, VideoOption videoOptions) {
			Timer.CreateDelay(delegate() {
				messageTarget.SendMessage("OnVideoCallback", NativeCamera.EditorCode);
			}, 0.5f);
		}

		public string FinalizePhoto(string filePath) {
			return filePath;
		}

		public string FinalizeVideo(string filePath) {
			return filePath;
		}
	}
}

