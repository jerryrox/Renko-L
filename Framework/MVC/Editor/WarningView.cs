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


		private static bool IsShowingWarning
		{
			get { return EditorPrefs.GetBool("MVCFramework.WarningView.IsShowingWarning", true); }
			set { EditorPrefs.SetBool("MVCFramework.WarningView.IsShowingWarning", value); }
		}


		public static void Render(MVCEditor editor, MvcConfig config)
		{
			if(GUILayout.Button(IsShowingWarning ? "Hide warnings" : "Show warnings")) {
				IsShowingWarning = !IsShowingWarning;
			}

			if(IsShowingWarning) {
				RenderNoInitialView(config);
				RenderMissingPrefab(editor, config);
			}
		}

		private static void RenderNoInitialView(MvcConfig config)
		{
			if(config.InitialView == null)
			{
				DisplayWarning("The initial view is not assigned. " +
					"If this was intended, call MVC.ShowView() manually when you need to start displaying the views.");
			}
		}

		private static void RenderMissingPrefab(MVCEditor editor, MvcConfig config)
		{
			for(int i=0; i<config.Views.Count; i++)
			{
				var view = config.Views[i];
				if(!File.Exists(MvcResources.GetViewPrefabPath(view, view.GetViewName(), true)))
				{
					DisplayWarning(string.Format(
						"Missing prefab for view ({0}) at path: Resources/{1}",
						view.Name,
						MvcResources.GetViewPrefabPath(view, view.GetViewName())
					));
				}
			}
		}

		private static void DisplayWarning(string message)
		{
			EditorGUILayout.HelpBox(message, MessageType.Warning);
		}
	}
}
#endif