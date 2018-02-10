using System;
using UnityEngine;
using UnityEditor;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Resource error view for MVC editor.
	/// </summary>
	public static class ResourceErrorView {

		/// <summary>
		/// Renders editor gui.
		/// </summary>
		public static void Render(MVCEditor editor) {
			GUILayout.Label("Something went wrong in the Resources folder.");
			GUILayout.Space(15);
			GUILayout.Label("Please do NOT delete, move, or modify auto-generated " +
				"folders and files.\nI will not be responsible for any losses in your project!");
			GUILayout.Space(15);
			if(GUILayout.Button("Fix")) {
				OnFix();
			}
		}

		private static void OnFix() {
			MvcResources.Setup();
		}
	}
}

