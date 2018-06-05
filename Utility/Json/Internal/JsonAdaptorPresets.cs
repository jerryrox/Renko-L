using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Info = Renko.Utility.JsonAdaptor.Info;

namespace Renko.Utility.Internal
{
	/// <summary>
	/// A static class that contains presets for JsonAdaptor.
	/// </summary>
	public static class JsonAdaptorPresets {

		/// <summary>
		/// Returns a dictionary of adaptor presets.
		/// </summary>
		public static IDictionary<Type, Info> Fetch() {
			Dictionary<Type, Info> dict = new Dictionary<Type, Info>();

			return dict;
		}
	}
}

