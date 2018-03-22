#if NGUI
using System;
using System.IO;
using UnityEngine;
using Renko.MVCFramework;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// A helper class for working with MVC resources.
	/// </summary>
	public static class MvcResources {

		/// <summary>
		/// The relative path of MVC resource directory inside Assets/Resources folder;
		/// </summary>
		public const string ResourcePath = "MVC";

		/// <summary>
		/// The name of configuration file.
		/// </summary>
		public const string ConfigFileName = "config";


		/// <summary>
		/// Creates necessary folders and files for MVC.
		/// </summary>
		public static void Setup() {
			string res = GetResourcePath(true);
			string conf = GetConfigFilePath(true);
			if(!Directory.Exists(res))
				Directory.CreateDirectory(res);
			if(!File.Exists(conf))
				SaveConfig(MvcConfig.DefaultConfig);
		}

		/// <summary>
		/// Saves the specified config instance to resources.
		/// </summary>
		public static void SaveConfig(MvcConfig config) {
			File.WriteAllText(GetConfigFilePath(true), config.ToString());
		}

		/// <summary>
		/// Returns whether MVC resources path is valid.
		/// </summary>
		public static bool IsValid() {
			return Directory.Exists(GetResourcePath(true)) && File.Exists(GetConfigFilePath(true));
		}

		/// <summary>
		/// Returns the resource path for specified config view.
		/// </summary>
		public static string GetViewPrefabPath(MvcConfig.View view, bool fullPath = false) {
			string path = Path.Combine(GetResourcePath(fullPath), view.ViewName);
			if(fullPath)
				path += ".prefab";
			return path;
		}

		/// <summary>
		/// Returns the MVC resource directory.
		/// </summary>
		public static string GetResourcePath(bool fullPath) {
			if(fullPath)
				return Path.Combine(Application.dataPath, "Resources/"+ResourcePath);
			return ResourcePath;
		}

		/// <summary>
		/// Returns the MVC configuration file inside workspace.
		/// </summary>
		public static string GetConfigFilePath(bool fullPath) {
			if(fullPath)
				return Path.Combine(GetResourcePath(true), ConfigFileName+".txt");
			return ResourcePath + '/' + ConfigFileName;
		}
	}
}
#endif