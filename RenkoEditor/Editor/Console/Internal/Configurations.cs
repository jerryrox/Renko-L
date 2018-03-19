using System;
using UnityEditor;
using UnityEngine;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// Stores DevConsole configurations.
	/// </summary>
	public static class Configurations {

		/// <summary>
		/// Max number of command histories that can be held at once.
		/// </summary>
		public static int MaxCommandHistory {
			get { return PlayerPrefs.GetInt(GetKey("maxCommandHistory"), 25); }
			set { PlayerPrefs.SetInt(GetKey("maxCommandHistory"), value); }
		}

		/// <summary>
		/// Max number of lines that will be displayed in output window.
		/// </summary>
		public static int MaxOutputLines {
			get { return PlayerPrefs.GetInt(GetKey("maxOutputLines"), 50); }
			set { PlayerPrefs.SetInt(GetKey("maxOutputLines"), value); }
		}

		/// <summary>
		/// String value that contains the namespaces to use during code compilation.
		/// </summary>
		public static string Namespaces {
			get {
				string namespaces = PlayerPrefs.GetString(GetKey("namespaces"), null);
				if(string.IsNullOrEmpty(namespaces))
					return "System.Collections\nSystem.Collections.Generic\nUnityEngine";
				return namespaces;
			}
			set { PlayerPrefs.SetString(GetKey("namespaces"), value); }
		}


		/// <summary>
		/// Returns the unique config key for Renko-L DevConsole.
		/// </summary>
		private static string GetKey(string key) {
			return "renko_devconsole_"+key;
		}
	}
}

