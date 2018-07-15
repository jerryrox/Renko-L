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


		private static bool IsShowingWarning {
			get { return EditorPrefs.GetBool("MVCFramework.WarningView.IsShowingWarning", true); }
			set { EditorPrefs.SetBool("MVCFramework.WarningView.IsShowingWarning", value); }
		}


		public static void Render(MVCEditor editor, MvcConfig config) {
			if(GUILayout.Button(IsShowingWarning ? "Hide warnings" : "Show warnings")) {
				IsShowingWarning = !IsShowingWarning;
			}

			if(IsShowingWarning) {
				RenderMissingPrefab(editor, config);
			}
		}

		private static void RenderMissingPrefab(MVCEditor editor, MvcConfig config) {
			for(int i=0; i<config.Views.Count; i++) {
				var view = config.Views[i];
				if(!File.Exists(MvcResources.GetViewPrefabPath(view, view.GetViewName(), true))) {
					EditorGUILayout.HelpBox(string.Format(
						"Missing prefab for view ({0}) at path: Resources/{1}",
						view.Name,
						MvcResources.GetViewPrefabPath(view, view.GetViewName())
					), MessageType.Warning);
				}
			}
		}
	}
}
#endif