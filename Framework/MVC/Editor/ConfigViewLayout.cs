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

		private const string DeleteWarning = "(This is an irreversible action!)";
		private static List<bool> viewFlags;

		private static string editingName;
		private static string editingBaseClassName;
		private static MvcLifeType editingLifeType;
		private static MvcRescaleType editingRescaleMode;
		private static bool editingInitial;


		private static bool IsShowingView {
			get { return EditorPrefs.GetBool("MVCFramework.ConfigViewLayout.IsShowingView", false); }
			set { EditorPrefs.SetBool("MVCFramework.ConfigViewLayout.IsShowingView", value); }
		}


		/// <summary>
		/// Renders list of config views on custom editor.
		/// </summary>
		public static void Render(MVCEditor editor, MvcConfig config) {
			ConfigViewEditorFlags.Setup(config);

			DrawControls(editor, config);
			DrawViews(editor, config);
		}

		private static void DrawControls(MVCEditor editor, MvcConfig config) {
			if(GUILayout.Button(IsShowingView ? "Hide views" : "Show views"))
				IsShowingView = !IsShowingView;
		}

		private static void DrawViews(MVCEditor editor, MvcConfig config) {
			if(!IsShowingView)
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
				EditorGUILayout.TextField("View name", editingName)
			);
			editingBaseClassName = EditorGUILayout.TextField("Custom base class", editingBaseClassName);
			editingLifeType = (MvcLifeType)EditorGUILayout.EnumPopup("Custom lifecycle", editingLifeType);
			editingRescaleMode = (MvcRescaleType)EditorGUILayout.EnumPopup("View rescale mode", editingRescaleMode);
			editingInitial = EditorGUILayout.Toggle("Is initial view?", editingInitial);

			if(GUILayout.Button("Save & Close")) {
				if(ApplyEdit(config, view)) {
					ConfigViewEditorFlags.ResetFlags();
				}
			}

			if(GUILayout.Button("Danger Zone!")) {
				ConfigViewEditorFlags.IsDeleteOpen ^= true;
			}

			if(ConfigViewEditorFlags.IsDeleteOpen) {
				GUILayout.BeginVertical("GroupBox");

				EditorGUILayout.HelpBox(
					"It is highly recommended to apply configurations first before performing any actions here.",
					MessageType.Warning
				);

				if(GUILayout.Button("Delete config")) {
					if(EditorDialog.OpenAlert(
						"Delete view configuration.",
						"Are you sure you want to delete this view's configuration? " + DeleteWarning,
						"Yes", "No")) {

						// Remove view from views list.
						MvcViewRemover.RemoveFromConfig(config.Views, index);
						ConfigViewEditorFlags.Setup(config);
						ConfigViewEditorFlags.ResetFlags();
					}
				}

				// If loaded from resources, the user must've already created a prefab or at least scripts.
				if(view.IsFromResources) {
					EditorGUILayout.Space();

					if(GUILayout.Button("Delete prefab")) {
						// Confirm prefab deletion
						if(EditorDialog.OpenAlert(
							"Delete view prefab",
							"Are you sure you want to delete this view's prefab? " + DeleteWarning,
							"Yes", "No")) {

							MvcViewRemover.RemovePrefab(view);
						}
					}

					EditorGUILayout.Space();

					if(GUILayout.Button("Delete all")) {
						// Confirm deletion of all view-related things
						if(EditorDialog.OpenAlert(
							"Delete view config, scripts, prefab",
							"Are you sure you want to delete this view's config, scripts, and prefab?" + DeleteWarning,
							"Yes", "No")) {

							MvcViewRemover.RemoveFromConfig(config.Views, index);
							MvcViewRemover.RemoveScripts(view);
							MvcViewRemover.RemovePrefab(view);
							MvcViewRemover.Finalize(config);

							ConfigViewEditorFlags.Setup(config);
							ConfigViewEditorFlags.ResetFlags();
						}
					}
				}

				GUILayout.EndVertical();
			}

			GUILayout.EndVertical();
		}

		private static string ViewNameTrim(string value) {
			if(!value.EndsWith("View"))
				return value;
			
			return value.Substring(0, value.Length-4);
		}

		private static void SetEdit(MvcConfig.View view) {
			editingName = view.Name;
			editingBaseClassName = view.GetBaseClass();
			editingLifeType = view.LifeType;
			editingRescaleMode = view.ViewRescaleMode;
			editingInitial = view.IsInitial;
		}

		private static bool ApplyEdit(MvcConfig config, MvcConfig.View view) {
			// Silently replace view name that ends with suffix "View".
			editingName = ViewNameTrim(editingName);

			if(view.SetViewName(editingName) && view.SetBaseClassName(editingBaseClassName) &&
				config.IsConfigValid()) {

				view.LifeType = editingLifeType;
				view.ViewRescaleMode = editingRescaleMode;
				if(editingInitial != view.IsInitial)
					config.ToggleInitial(editingInitial ? view : null);
				return true;
			}
			return false;
		}
	}
}
#endif