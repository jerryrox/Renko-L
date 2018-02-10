#if NGUI
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Renko.MVCFramework.Internal;
using RenkoEditor;

namespace Renko.MVCFramework
{
	public static class WarningView {

		private static bool showWarnings = true;


		public static void Render(MVCEditor editor, MvcConfig config) {
			if(GUILayout.Button(showWarnings ? "Hide warnings" : "Show warnings")) {
				showWarnings = !showWarnings;
			}

			if(showWarnings) {
				RenderMissingPrefab(editor, config);
			}
		}

		private static void RenderMissingPrefab(MVCEditor editor, MvcConfig config) {
			for(int i=0; i<config.Views.Count; i++) {
				var view = config.Views[i];
				if(!File.Exists(MvcResources.GetViewPrefabPath(view, true))) {
					EditorGUILayout.HelpBox(string.Format(
						"Missing prefab for view ({0}) at path: Resources/{1}",
						view.Name,
						MvcResources.GetViewPrefabPath(view)
					), MessageType.Warning);
				}
			}
		}

		private static void RenderAutogenCleanup(MVCEditor editor, MvcConfig config) {
			
		}
	}
}
#endif