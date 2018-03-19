using System;
using System.Collections.Generic;
using Renko.Extensions;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// Helper class that holds console output string data.
	/// </summary>
	public static class OutputHistory {

		/// <summary>
		/// Format used for displaying command string.
		/// </summary>
		private const string CommandLineFormat = "> {0}";

		/// <summary>
		/// Format used for displaying command result(output) string.
		/// </summary>
		private const string OutputLineFormat = ": {0}";

		/// <summary>
		/// List of output info stored.
		/// </summary>
		private static List<OutputInfo> Outputs;

		private static bool IsInitialized = false;


		/// <summary>
		/// Returns the current number of outputs stored.
		/// </summary>
		public static int Count {
			get {
				Initialize();
				return Outputs.Count;
			}
		}


		public static void Initialize() {
			if(IsInitialized)
				return;

			IsInitialized = true;

			Outputs = new List<OutputInfo>(Configurations.MaxOutputLines);

		}

		/// <summary>
		/// Returns the output at specified index.
		/// </summary>
		public static OutputInfo GetOutput(int index) {
			Initialize();

			return Outputs[index];
		}

		/// <summary>
		/// Clears output list.
		/// </summary>
		public static void Clear() {
			Initialize();

			Outputs.Clear();
		}

		/// <summary>
		/// Removes first item from list.
		/// </summary>
		public static void RemoveFirstLine() {
			Initialize();

			Outputs.RemoveAt(0);
		}

		/// <summary>
		/// Adds the specified output info to history.
		/// </summary>
		public static void AddOutput(OutputInfo info) {
			Initialize();

			Outputs.Add(info);

			int lineCount = Outputs.Count;
			int maxLines = Configurations.MaxOutputLines;

			if(lineCount > maxLines) {
				Outputs.RemoveRange(0, lineCount-maxLines);
			}
		}

		/// <summary>
		/// Adds the specified string as a command output form.
		/// </summary>
		public static void AddCommandOutput(string text) {
			AddOutput(new OutputInfo(text, true));
		}

		/// <summary>
		/// Adds the specified string as a result output form.
		/// </summary>
		public static void AddResultOutput(string text) {
			AddOutput(new OutputInfo(text, false));
		}
	}
}

