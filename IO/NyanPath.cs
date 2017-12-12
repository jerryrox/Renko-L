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
		/// Returns the directory for storing temporary data.
		/// </summary>
		public static string TempDirectory {
			get { return GetRootDataPath("Temp"); }
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
	}
}

