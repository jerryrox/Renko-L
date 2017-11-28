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
		public static void AddCondition(string condition)
		{
			//Cleanse the param
			condition = condition.Trim().ToLower();

			//If doesn't exist in the conditions, add it
			if(!trueConditions.Contains(condition))
				trueConditions.Add(condition);
		}

		/// <summary>
		/// Removes a string value from conditions list.
		/// </summary>
		public static void RemoveCondition(string condition)
		{
			//Cleanse the param
			condition = condition.Trim().ToLower();

			//Remove condition
			trueConditions.Remove(condition);
		}

		/// <summary>
		/// Parses the string value and returns result
		/// </summary>
		public static bool Parse(string s)
		{
			//If string is null, return false
			if(s == null)
				return false;

			//For each condition
			for(int i=0; i<trueConditions.Count; i++)
			{
				//If there was any matching condition, return true
				if(trueConditions[i].Equals(s))
					return true;
			}

			//No condition matched. return false
			return false;
		}
	}
}