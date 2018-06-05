using System;
using Renko.Utility;

namespace Renko.Plugin
{
	/// <summary>
	/// Object that contains video recording information for the plugin.
	/// </summary>
	public class VideoOption {

		/// <summary>
		/// The max duration in seconds for limiting video recording.
		/// </summary>
		[JsonAllowSerialize]
		public int MaxDuration {
			get; set;
		}


		public VideoOption () {
			MaxDuration = 0;
		}
	}
}

