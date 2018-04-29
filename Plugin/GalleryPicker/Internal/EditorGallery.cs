using System;
using UnityEngine;
using Renko.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Renko.Plugin.Internal
{
	public class EditorGallery : IGalleryPicker { 

		private GameObject pickerObject;
		private bool isPickingImage;


		public void Initialize (string objectName) {
			#if UNITY_EDITOR
			try {
				pickerObject = GameObject.FindObjectOfType<GalleryPicker>().gameObject;
			}
			catch(Exception e) {
				RenLog.Log(
					LogLevel.Warning,
					"EditorGallery.Initialize - GalleryPicker instance is not found in scene!\n"+e.Message
				);
			}
			#endif
		}

		public void Destroy () {

		}

		public void PickImage (CropOption cropOption) {
			#if UNITY_EDITOR
			isPickingImage = true;
			if(!OpenFile("Pick a JPG file. (Close it for PNG)", Application.dataPath, "jpg")) {
				if(!OpenFile("Pick a PNG file.", Application.dataPath, "png")) {
					pickerObject.SendMessage("OnImagePicked", GalleryPicker.CancelCode);
				}
			}
			#endif
		}

		public void PickVideo () {
			#if UNITY_EDITOR
			isPickingImage = false;
			if(!OpenFile("Pick a mp4 file. (Close it for avi)", Application.dataPath, "mp4")) {
				if(!OpenFile("Pick a avi file.", Application.dataPath, "avi")) {
					pickerObject.SendMessage("OnVideoPicked", GalleryPicker.CancelCode);
				}
			}
			#endif
		}

		public string FinalizeImagePick (string result) {
			return result;
		}

		public string FinalizeVideoPick (string result) {
			return result;
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Opens the file selector dialog and returns whether a file is successfully picked.
		/// </summary>
		private bool OpenFile(string message, string directory, string extension) {
			string path = EditorUtility.OpenFilePanel( message, directory, extension );

			if(path.Length != 0) {
				pickerObject.SendMessage(isPickingImage ? "OnImagePicked" : "OnVideoPicked", path);
				return true;
			}

			return false;
		}
		#endif
	}
}

