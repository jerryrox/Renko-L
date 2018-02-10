using System;
using System.IO;
using UnityEngine;
using Renko.IO;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// A class that provides helper methods related to MVC workspace.
	/// </summary>
	public static class MvcWorkspace {
		
		/// <summary>
		/// The workspace directory path.
		/// Will be defined by developer.
		/// </summary>
		public static string WorkspacePath {
			get { return PlayerPrefs.GetString("mvc_workspace_path", ""); }
			private set { PlayerPrefs.SetString("mvc_workspace_path", value); }
		}

		/// <summary>
		/// Returns the path to auto-generated code directory.
		/// </summary>
		public static string AutogenCodePath {
			get { return Path.Combine(WorkspacePath, "_AutoGen"); }
		}


		/// <summary>
		/// Sets the specified path as workspace.
		/// </summary>
		public static void SetWorkspace(string path) {
			WorkspacePath = path;

			MvcResources.Setup();
		}

		/// <summary>
		/// Auto generates MVC partial script.
		/// </summary>
		public static void AutogenMVC(MvcConfig config) {
			CreateAutogenDirectory();
			MvcAutoGen.Create(AutogenCodePath, config);
		}

		/// <summary>
		/// Auto generates MVC View, Model, and Controller files for specified config view object.
		/// </summary>
		public static void AutogenView(MvcConfig.View configView) {
			CreateAutogenDirectory();
			MvcViewAutoGen.Create(AutogenCodePath, WorkspacePath, configView);
		}

		/// <summary>
		/// Returns whether there is a valid workspace setup.
		/// </summary>
		public static bool HasValidWorkspace() {
			string workspace = WorkspacePath;
			if(string.IsNullOrEmpty(workspace) || !NyanPath.IsProjectPath(workspace))
				return false;
			
			return Directory.Exists(WorkspacePath);
		}

		/// <summary>
		/// Returns the path of workspace or inside it.
		/// </summary>
		public static string GetWorkspacePath(string innerPath = "") {
			return Path.Combine(WorkspacePath, innerPath);
		}

		/// <summary>
		/// Creates autogen directory if doesn't exist.
		/// </summary>
		private static void CreateAutogenDirectory() {
			if(!Directory.Exists(AutogenCodePath))
				Directory.CreateDirectory(AutogenCodePath);
		}
	}
}

