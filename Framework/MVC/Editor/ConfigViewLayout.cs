#if NGUI
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Renko.Extensions;
using Renko.MVCFramework.Internal;
using RenkoEditor;

namespace Renko.MVCFramework
{
	public static class ConfigViewLayout {

		private static List<bool> viewFlags;

		private static bool showingViews;

		private static string editingName;
		private static string editingBaseClassName;
		private static MvcLifeType editingLifeType;
		private static bool editingInitial;


		/// <summary>
		/// Renders list of config views on custom editor.
		/// </summary>
		public static void Render(MVCEditor editor, MvcConfig config) {
			ConfigViewEditorFlags.Setup(config);

			DrawControls(editor, config);
			DrawViews(editor, config);
		}

		private static void DrawControls(MVCEditor editor, MvcConfig config) {
			if(GUILayout.Button(showingViews ? "Hide views" : "Show views"))
				showingViews = !showingViews;
		}

		private static void DrawViews(MVCEditor editor, MvcConfig config) {
			if(!showingViews)
				return;

			GUILayout.BeginVertical("GroupBox");

			for(int i=0; i<config.Views.Count; i++) {
				if(ConfigViewEditorFlags.IsOpen[i])
					DrawOpenedView(editor, config, config.Views[i], i);
				else
					DrawClosedView(editor, config, config.Views[i], i);
			}

			if(GUILayout.Button("Create View")) {
				config.Views.Add(new MvcConfig.View(config) {
					Name = "View"+config.Views.Count
				});

				ConfigViewEditorFlags.Setup(config);
				ConfigViewEditorFlags.SetOpen(config.Views.Count-1);
				SetEdit(config.Views.GetLast());
			}

			GUILayout.EndVertical();
		}

		private static void DrawClosedView(MVCEditor editor, MvcConfig config, MvcConfig.View view, int index) {
			GUILayout.BeginHorizontal("GroupBox");

			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Label(view.Name + " : " + view.GetBaseClass());
			GUILayout.Label(view.LifeType.ToString() + " on dispose.");
			if(view.IsInitial)
				GUILayout.Label("Initial view");
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			if(GUILayout.Button("Edit")) {
				ConfigViewEditorFlags.SetOpen(index);
				SetEdit(view);
			}

			GUILayout.EndHorizontal();
		}

		private static void DrawOpenedView(MVCEditor editor, MvcConfig config, MvcConfig.View view, int index) {
			GUILayout.BeginVertical("LightmapEditorSelectedHighlight");

			editingName = ViewNameTrim(
				EditorGUILayout.TextField("View name", editingName), true
			);
			editingBaseClassName = EditorGUILayout.TextField("Custom base class", editingBaseClassName);
			editingLifeType = (MvcLifeType)EditorGUILayout.EnumPopup("Custom lifecycle", editingLifeType);
			editingInitial = EditorGUILayout.Toggle("Is initial view?", editingInitial);

			if(GUILayout.Button("Save")) {
				ApplyEdit(config, view);
			}
			if(GUILayout.Button("Save & Close")) {
				if(ApplyEdit(config, view)) {
					ConfigViewEditorFlags.ResetFlags();
				}
			}
			if(GUILayout.Button("Delete")) {
				if(EditorDialog.OpenAlert(
					"Delete view \""+view.Name+'\"',
					"Are you sure you want to delete this view? (This an irreversible action!)",
					"Yes", "No")) {

					config.Views.RemoveAt(index);
					ConfigViewEditorFlags.Setup(config);
					ConfigViewEditorFlags.ResetFlags();
				}
			}

			GUILayout.EndVertical();
		}

		private static string ViewNameTrim(string value, bool displayAlert) {
			if(!value.EndsWith("View"))
				return value;

			string targetString = value.Substring(0, value.Length-4);
			if(displayAlert) {
				EditorDialog.OpenAlert(
					"Invalid name ending",

					"All views are automatically added with suffix 'View'.\n" +
					"Your view name will be trimmed to " + targetString + " instead."
				);
			}
			return targetString;
		}

		private static void SetEdit(MvcConfig.View view) {
			editingName = view.Name;
			editingBaseClassName = view.GetBaseClass();
			editingLifeType = view.LifeType;
			editingInitial = view.IsInitial;
		}

		private static bool ApplyEdit(MvcConfig config, MvcConfig.View view) {
			// Silently replace view name that ends with suffix "View".
			editingName = ViewNameTrim(editingName, false);

			if(view.SetViewName(editingName) && view.SetBaseClassName(editingBaseClassName)) {
				view.LifeType = editingLifeType;
				if(editingInitial != view.IsInitial)
					config.ToggleInitial(editingInitial ? view : null);
				return true;
			}
			return false;
		}
	}
}
#endif