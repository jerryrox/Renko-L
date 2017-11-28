using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A helper class that parses a string to boolean value based on its representation.
	/// By default, "true", "y", "yes", "1" will return true.
	/// You may add to or remove these conditions using the methods provided.
	/// </summary>
	public static class BoolParser {

		/// <summary>
		/// The list of conditions that will make the Parse method return true.
		/// </summary>
		private static List<string> trueConditions = new List<string>() {
			"true",
			"y",
			"yes",
			"1"
		};

		/// <summary>
		/// Adds a string value that will make the Parse method return true.
		/// </summary>
		public static void AddCondition(string condition) {
			condition = condition.Trim().ToLower();
			if(!trueConditions.Contains(condition))
				trueConditions.Add(condition);
		}

		/// <summary>
		/// Removes a string value from conditions list.
		/// </summary>
		public static void RemoveCondition(string condition) {
			condition = condition.Trim().ToLower();
			trueConditions.Remove(condition);
		}

		/// <summary>
		/// Parses the string value and returns result
		/// </summary>
		public static bool Parse(string s) {
			if(s == null)
				return false;
			for(int i=0; i<trueConditions.Count; i++) {
				if(trueConditions[i].Equals(s))
					return true;
			}
			return false;
		}
	}
}