using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// A helper class for managing temporary variables in DevConsole.
	/// </summary>
	public static class VariableProvider {

		/// <summary>
		/// Named property provider.
		/// </summary>
		public static VariablePropertyHelper Variables;

		/// <summary>
		/// Dictionary of currently stored variables.
		/// </summary>
		private static Dictionary<string, VariableInfo> StoredVariables;

		/// <summary>
		/// List of variable names currently registered.
		/// </summary>
		private static List<string> VariableNames;

		private static bool IsInitialized = false;


		public static void Initialize() {
			if(IsInitialized)
				return;

			IsInitialized = true;

			Variables = new VariablePropertyHelper();
			StoredVariables = new Dictionary<string, VariableInfo>(20);
			VariableNames = new List<string>(20);
		}

		/// <summary>
		/// Assigns the specified value to the key.
		/// </summary>
		public static void AssignValue(string key, object value) {
			Initialize();

			if(StoredVariables.ContainsKey(key)) {
				if(value == null) {
					RemoveVariable(key);
					return;
				}

				StoredVariables[key].SetValue(value);
			}
			else {
				if(value == null)
					return;

				AddVariable(key, value);
			}
		}

		/// <summary>
		/// Returns the variable info for specified key.
		/// </summary>
		public static VariableInfo GetVariable(string key) {
			Initialize();

			if(StoredVariables.ContainsKey(key))
				return StoredVariables[key];
			return null;
		}

		/// <summary>
		/// Creates a new variable with specified key and value.
		/// </summary>
		private static void AddVariable(string key, object value) {
			StoredVariables.Add(key, new VariableInfo(key, value));
			VariableNames.Add(key);
		}

		/// <summary>
		/// Removes the variable with specified key.
		/// </summary>
		private static void RemoveVariable(string key) {
			StoredVariables.Remove(key);
			VariableNames.Remove(key);
		}
	}
}

