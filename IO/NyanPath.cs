using System;
using System.IO;
using UnityEngine;

namespace Renko.IO
{
	/// <summary>
	/// A class with additional features for System.IO.Path.
	/// </summary>
	public static class NyanPath {

		/// <summary>
		/// Returns the directory for storing cache data.
		/// </summary>
		public static string CachesDirectory {
			get { return GetRootDataPath("Caches"); }
		}

		/// <summary>
		/// Returns the relative caches directory inside the app's persistent datapath.
		/// </summary>
		public static string RelativeCachesDirectory {
			get { return CachesDirectory.Substring(Application.persistentDataPath.Length+1); }
		}

		/// <summary>
		/// Returns the directory for storing temporary data.
		/// </summary>
		public static string TempDirectory {
			get { return GetRootDataPath("Temp"); }
		}

		/// <summary>
		/// Returns the relative temp directory inside the app's persistent datapath.
		/// </summary>
		public static string RelativeTempDirectory {
			get { return TempDirectory.Substring(Application.persistentDataPath.Length+1); }
		}


		/// <summary>
		/// Creates all missing directories for Renko-L.
		/// </summary>
		public static void CreateRenkoDirectories() {
			if(!Directory.Exists(CachesDirectory))
				Directory.CreateDirectory(CachesDirectory);
			if(!Directory.Exists(TempDirectory))
				Directory.CreateDirectory(TempDirectory);
		}

		/// <summary>
		/// Returns the persistentDataPath exclusively for Renko-L.
		/// </summary>
		public static string GetRootDataPath(string innerPath = "") {
			return Path.Combine(
				Application.persistentDataPath+"/RenkoL",
				innerPath
			);
		}

		/// <summary>
		/// Deletes all files and directories in the Temp folder.
		/// </summary>
		public static void ClearTempFiles() {
			DirectoryInfo tempDir = new DirectoryInfo(TempDirectory);
			foreach(FileInfo file in tempDir.GetFiles())
				file.Delete();
			foreach(DirectoryInfo subDirectory in tempDir.GetDirectories())
				subDirectory.Delete(true);
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Returns the RenkoLibrary path.
		/// </summary>
		public static string GetLibraryPath(string innerPath = "") {
			return Path.Combine(
				Application.dataPath+"/Renko-L",
				innerPath
			);
		}

		/// <summary>
		/// Returns the relative path inside the project's asset folder.
		/// </summary>
		public static string EditorRelativePath(string path) {
			return path.Remove(0, Application.dataPath.Length+1);
		}

		/// <summary>
		/// Returns whether specified path is inside the current project' asset folder and
		/// is not located in Resources, Plugins, Editor, or StreamingAssets directory.
		/// </summary>
		public static bool IsStandardProjectPath(string path) {
			return IsProjectPath(path) && !IsStreamingAssetsPath(path) && !IsResourcesPath(path) &&
				!IsPluginsPath(path) && !IsEditorPath(path);
		}

		/// <summary>
		/// Returns whether specified path is located inside the current project's asset folder.
		/// </summary>
		public static bool IsProjectPath(string path) {
			return path.Contains(Application.dataPath);
		}

		/// <summary>
		/// Returns whether specified path is located inside the current project's streaming assets folder.
		/// </summary>
		public static bool IsStreamingAssetsPath(string path) {
			return path.Contains(Application.streamingAssetsPath);
		}

		/// <summary>
		/// Returns whether specified path is located inside the project's resources folder.
		/// </summary>
		public static bool IsResourcesPath(string path) {
			return path.Contains("/Resources/") || path.EndsWith("/Resources");
		}

		/// <summary>
		/// Returns whether specified path is located inside the project's plugins folder.
		/// </summary>
		public static bool IsPluginsPath(string path) {
			return path.Contains("/Plugins/") || path.EndsWith("/Plugins");
		}

		/// <summary>
		/// Returns whether specified path is located inside the project's editor folder.
		/// </summary>
		public static bool IsEditorPath(string path) {
			return path.Contains("/Editor/") || path.EndsWith("/Editor");
		}
		#endif
	}
}

