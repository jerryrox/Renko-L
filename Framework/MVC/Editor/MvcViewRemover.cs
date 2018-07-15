using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using Renko.Diagnostics;
using RenkoEditor;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// A class that handle removal of MVC views.
	/// </summary>
	public static class MvcViewRemover {

		/// <summary>
		/// Removes the view config from specified list at index.
		/// </summary>
		public static void RemoveFromConfig(List<MvcConfig.View> views, int removeIndex)
		{
			views.RemoveAt(removeIndex);
		}

		/// <summary>
		/// Removes all scripts related to specifeid MVC view.
		/// </summary>
		public static void RemoveScripts(MvcConfig.View view)
		{
			// Delete base autogen file
			string autoGenFile = MvcViewAutoGen.GetBaseScriptPath(
				MvcWorkspace.AutogenCodePath,
				view.Name
			);
			if(File.Exists(autoGenFile))
				File.Delete(autoGenFile);
			else
				RenLog.Log("MvcViewRemover.RemoveScripts - Base file missing: " + autoGenFile);

			// Delete the folder
			string directory = MvcViewAutoGen.GetDirectoryPath(
				MvcWorkspace.WorkspacePath,
				view.Name
			);
			if(Directory.Exists(directory))
				Directory.Delete(directory, true);
			else
				RenLog.Log("MvcViewRemover.RemoveScripts - Scripts directory missing: " + directory);

			// Refresh the project view
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Removes the view prefab linked to specified view config from resources.
		/// </summary>
		public static void RemovePrefab(MvcConfig.View view)
		{
			string prefabPath = view.GetResourcePath(true, true);
			if(File.Exists(prefabPath))
				File.Delete(prefabPath);
			else
				RenLog.Log("MvcViewRemover.RemoveScripts - Prefab file missing: " + prefabPath);

			// Refresh the project view
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Handles finalization after performing Delete All action.
		/// </summary>
		public static void Finalize(MvcConfig config)
		{
			ConfigSynchronizer.Sync(config);
		}
	}
}
