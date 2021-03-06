﻿#if NGUI
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using RenkoEditor;
using Renko.IO;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Initialized view for MVC editor.
	/// </summary>
	public static class InitializedView {

		private static MvcConfig cachedConfig;

		private static string advancedViewBaseClassName;
		private static bool isAdvancedViewSettings;


		private static GUIStyle CategoryLabelStyle = new GUIStyle() {
			fontSize = 16
		};

		private static MvcConfig Config {
			get {
				if(cachedConfig == null)
					cachedConfig = MvcConfig.LoadFromResources();
				return cachedConfig;
			}
		}


		/// <summary>
		/// Renders editor gui.
		/// </summary>
		public static void Render(MVCEditor editor) {
			RenderGeneral(editor);
			RenderSpace();
			RenderScreen(editor);
			RenderSpace();
			RenderView(editor);
		}

		private static int ClampRescaleModeValue(int index)
		{
			// If default
			if(index == 3)
				return 0;
			return index;
		}

		private static string GetRescaleModeHelMessage(int index) {
			switch(index) {
			case 0:
				return "Your views' width will stay constant while the height rescales.";
			case 1:
				return "Your views' height will stay constant while the width rescales.";
			case 2:
				return "Your views' width or height will scale based on screen ratio.";
			}
			return "Unknown";
		}

		private static void RenderSpace() {
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		private static void RenderGeneral(MVCEditor editor) {
			EditorGUILayout.LabelField(
				"General Settings", CategoryLabelStyle
			);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Select a scene object to use as a parent for MVC view instances.");
			editor.I.ViewParent = (GameObject)RenkoEditorLayout.SceneObjectField(
				"ViewParent", editor.I.ViewParent, typeof(GameObject)
			);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Whether the initial view should be shown on awake.\n" +
				"If false, you'll have to show it manually via code.");
			EditorGUILayout.PropertyField(
				editor.InitializeViewOnAwake, new GUIContent("InitializeViewOnAwake")
			);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Determines how disposed views will be handled by default.");
			EditorGUILayout.PropertyField(
				editor.UiLifecycleMethod, new GUIContent("UiLifecycleMethod")
			);
			if(editor.UiLifecycleMethod.enumValueIndex == 0)
				editor.UiLifecycleMethod.enumValueIndex = 1;
		}

		private static void RenderScreen(MVCEditor editor) {
			EditorGUILayout.LabelField(
				"Screen Settings", CategoryLabelStyle
			);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Set base (or desired) resolution of your UI.");
			EditorGUILayout.PropertyField(
				editor.BaseResolution, new GUIContent("BaseResolution")
			);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Set default scaling mode for all views.");
			EditorGUILayout.PropertyField(
				editor.RescaleMode, new GUIContent("RescaleMode")
			);
			editor.RescaleMode.enumValueIndex = ClampRescaleModeValue(editor.RescaleMode.enumValueIndex);
			EditorGUILayout.HelpBox(
				GetRescaleModeHelMessage(editor.RescaleMode.enumValueIndex),
				MessageType.Info
			);

			EditorGUILayout.Space();
		}

		private static void RenderView(MVCEditor editor) {
			EditorGUILayout.LabelField(
				"View Settings", CategoryLabelStyle
			);

			EditorGUILayout.Space();

			EditorColors.SetBackgroundColor(Color.green);
			{
				if(GUILayout.Button("Apply configuration")) {
					if(Config.IsConfigValid())
						ConfigSynchronizer.Sync(Config);
				}
			}
			EditorColors.ResetBackgroundColor();

			EditorGUILayout.HelpBox("Click the button above whenever you make changes in this area.", MessageType.Info);

			EditorGUILayout.Space();

			if(GUILayout.Button("Generate prefabs")) {
				if(Config.IsConfigValid() && !EditorApplication.isCompiling)
					MvcPrefabMaker.CreatePrefabs(editor.I, Config);
			}

			EditorGUILayout.Space();

			WarningView.Render(editor, Config);

			EditorGUILayout.Space();

			ConfigViewLayout.Render(editor, Config);

			EditorGUILayout.Space();

			if(GUILayout.Button(isAdvancedViewSettings ? "Hide advanced settings" : "Show advnaced settings"))
				isAdvancedViewSettings = !isAdvancedViewSettings;
			if(isAdvancedViewSettings)
				RenderAdvancedView(editor);
		}

		private static void RenderAdvancedView(MVCEditor editor) {
			EditorGUILayout.HelpBox(
				"Advanced view settings should NOT be touched in general cases.\nUnless you have full understanding" +
				"of this framework's process, it's highly recommended to just leave the settings as-is.",
				MessageType.Warning
			);

			RenderSpace();

			EditorGUILayout.LabelField(
				"Current workspace directory: " + NyanPath.EditorRelativePath(MvcWorkspace.WorkspacePath)
			);
			EditorGUILayout.HelpBox(
				"If you change the workspace directory, you must move/delete script files already created in " +
				"the previous directory.",
				MessageType.Warning
			);
			if(GUILayout.Button("Change workspace directory")) {
				MVCEditor.SelectWorkspaceDirectory();
			}

			RenderSpace();

			EditorGUILayout.LabelField("Default base class of MVC views.");
			Config.SetBaseClassName(EditorGUILayout.TextField(
				"BaseClassName",
				Config.BaseClassName
			));
		}
	}
}
#endif