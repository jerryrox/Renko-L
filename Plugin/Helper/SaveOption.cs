using System;
using Renko.Extensions;
using Renko.Utility;

namespace Renko.Plugin
{
	/// <summary>
	/// Object that contains saving information for the plugin.
	/// </summary>
	public class SaveOption {
		
		private string savePath;


		/// <summary>
		/// Whether the camera output should be saved to the library or Application.persistentDataPath.
		/// </summary>
		public bool SaveToLibrary {
			get; set;
		}

		/// <summary>
		/// The desired name of the new photo to be taken.
		/// </summary>
		public string FileName {
			get { return savePath; }
			set {
				if(string.IsNullOrEmpty(value))
					value = DateTime.Now.ToUnixTimestamp().ToString();
				savePath = value;
			}
		}


		public SaveOption() {
			SaveToLibrary = false;
			FileName = null;
		}
	}
}