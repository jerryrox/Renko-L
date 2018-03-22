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
				MVCEditor.SelectWorkspaceDirectory();
			}
		}
	}
}
#endif