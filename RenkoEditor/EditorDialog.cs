using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Renko.IO;

namespace RenkoEditor
{
	/// <summary>
	/// A utility script that wraps UnityEditor's OpenFilePanel and OpenFolderPanel features.
	/// </summary>
	public static class EditorDialog {

		/// <summary>
		/// The path of file that was last opened.
		/// </summary>
		private static string LastFilePath {
			get {
				string path = PlayerPrefs.GetString("Renko_EditorDialog_LastFilePath", "");
				if(!File.Exists(path))
					return Application.dataPath;
				return path;
			}
			set { PlayerPrefs.SetString("Renko_EditorDialog_LastFilePath", value); }
		}

		/// <summary>
		/// The path of the folder that was last opened.
		/// </summary>
		private static string LastFolderPath {
			get {
				string path = PlayerPrefs.GetString("Renko_EditorDialog_LastFolderPath", Application.dataPath);
				if(!Directory.Exists(path))
					return Application.dataPath;
				return path;
			}
			set { PlayerPrefs.SetString("Renko_EditorDialog_LastFolderPath", value); }
		}


		/// <summary>
		/// Opens a file selector window and returns the selected file's path.
		/// If cancelled, null is returned.
		/// </summary>
		public static string OpenFile(string extensions = "", bool saveLastPath = false, bool getRelativePath = false) {
			string result = EditorUtility.OpenFilePanel("Select a file", LastFilePath, extensions);
			if(string.IsNullOrEmpty(result))
				return null;
			
			if(saveLastPath)
				LastFilePath = result;
			
			if(getRelativePath)
				return NyanPath.EditorRelativePath(result);
			return result;
		}

		/// <summary>
		/// Opens a folder selector window and returns the selected file's path.
		/// If cancelled, null is returned.
		/// </summary>
		public static string OpenDirectory(bool saveLastPath = false, bool getRelativePath = false) {
			string result = EditorUtility.OpenFolderPanel("Select a folder", LastFolderPath, null);
			if(string.IsNullOrEmpty(result))
				return null;

			if(saveLastPath)
				LastFolderPath = result;

			if(getRelativePath)
				return NyanPath.EditorRelativePath(result);
			return result;
		}

		/// <summary>
		/// Opens an alert dialog with an Ok button only.
		/// </summary>
		public static bool OpenAlert(string title, string message) {
			return OpenAlert(title, message, "Ok");
		}

		/// <summary>
		/// Opens an alert dialog.
		/// Returns whether it's a confirm button or not.
		/// </summary>
		public static bool OpenAlert(string title, string message, string ok, string cancel = "") {
			return EditorUtility.DisplayDialog(title, message, ok, cancel);
		}
	}
}

