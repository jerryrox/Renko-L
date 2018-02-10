using System.IO;
using UnityEngine;
using UnityEditor;
using Renko.IO;

namespace RenkoEditor
{
	/// <summary>
	/// A helper class for checking abnormalities in RenkoLibrary.
	/// </summary>
	public static class Abnormalities {

		/// <summary>
		/// Returns whether RenkoLibrary is placed in the project window correctly.
		/// </summary>
		public static bool CheckRootPath() {
			if(!Directory.Exists(NyanPath.GetLibraryPath())) {
				Log("You should place RenkoLibrary in project root directory as 'Renko-L'!");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Logs debug message to console.
		/// </summary>
		private static void Log(string message) {
			Debug.LogWarning("RenkoL Abnormality Detected - " + message);
		}
	}
}
