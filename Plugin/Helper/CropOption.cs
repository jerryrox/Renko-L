using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Data;

namespace Renko.Plugin
{
	/// <summary>
	/// Object that contains cropping information for the plugin.
	/// </summary>
	public class CropOption {

		/// <summary>
		/// The target aspect ratio of your image.
		/// </summary>
		[JsonAllowSerialize]
		public Vector2 Ratio {
			get; set;
		}

		/// <summary>
		/// The target resolution of your image.
		/// </summary>
		[JsonAllowSerialize]
		public Vector2 Size {
			get; set;
		}

		/// <summary>
		/// Returns whether the cropping process will be done.
		/// </summary>
		[JsonAllowSerialize]
		public bool IsCropping {
			get {
				Vector2 ratio = Ratio;
				Vector2 size = Size;
				return ratio.x > 0f && ratio.y > 0f && size.x > 0f && size.y > 0f;
			}
		}


		public CropOption() {
			Ratio = new Vector2();
			Size = new Vector2();
		}
	}
}