#if NGUI
using System;
using UnityEditor;
using UnityEngine;
using Renko.IO;
using RenkoEditor;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Uninitialized view for MVC editor.
	/// </summary>
	public static class UninitializedView {
		

		/// <summary>
		/// Renders editor gui.
		/// </summary>
		public static void Render(MVCEditor editor) {
			GUILayout.Label("Select a new or existing MVC workspace.");
			if(GUILayout.Button("Select directory")) {
				OnSelectDirectory();
			}
		}

		private static void OnSelectDirectory() {
			// Get valid folder path
			string folderPath = EditorDialog.OpenDirectory();
			if(string.IsNullOrEmpty(folderPath))
				return;
			if(!NyanPath.IsStandardProjectPath(folderPath)) {
				EditorDialog.OpenAlert(
					"Error",
					"Workspace must be placed inside the project, but outside of StreamingAssets, Resources, " +
					"Plugins, and Editor folder."
				);
				return;
			}

			// Setup workspace
			MvcWorkspace.SetWorkspace(folderPath);
		}
	}
}
#endif