using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A static helper class for this json module.
	/// </summary>
	public static class Json {
		
		/// <summary>
		/// Parses the specified json string and returns a JsonData object.
		/// </summary>
		public static JsonData Parse(string json) { return JsonParser.Parse(json); }

		/// <summary>
		/// Returns a string representation of specified JsonData.
		/// </summary>
		public static string ToString(JsonData data) { return JsonSerializer.Serialize(data); }
	}
}