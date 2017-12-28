using System;
using System.IO;
using UnityEngine;

namespace Renko.IO
{
	//TODO: Add more useful stuffs

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
	}
}

