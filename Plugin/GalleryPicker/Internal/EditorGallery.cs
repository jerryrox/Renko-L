using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Renko.Plugin.Internal
{
	public class EditorGallery : IGalleryPicker { 

		//TODO: Implement with editor's file selection dialog.

		#if UNITY_EDITOR

		#endif
		
		public void Initialize (string objectName) {
			
		}

		public void Destroy () {
			
		}

		public void PickImage (CropOption cropOption) {
			
		}

		public void PickVideo () {
			
		}

		public void FinalizeImagePick (string result) {
			
		}

		public void FinalizeVideoPick (string result) {
			
		}
	}
}

