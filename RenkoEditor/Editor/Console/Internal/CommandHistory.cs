using System;
using System.Collections.Generic;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// A helper class for implementing command history.
	/// </summary>
	public static class CommandHistory {

		/// <summary>
		/// List of commands recorded.
		/// </summary>
		private static List<CommandInfo> Commands;

		private static bool IsInitialized = false;


		/// <summary>
		/// Returns the number of commands currently recorded.
		/// </summary>
		public static int CommandCount {
			get { return Commands.Count; }
		}


		public static void Initialize() {
			if(IsInitialized)
				return;

			IsInitialized = true;

			Commands = new List<CommandInfo>(Configurations.MaxCommandHistory);
		}

		/// <summary>
		/// Adds the specified command to history list.
		/// </summary>
		public static void AddCommand(CommandInfo info) {
			Initialize();

			Commands.Add(info);
			if(Commands.Count > Configurations.MaxCommandHistory)
				Commands.RemoveAt(0);
		}

		/// <summary>
		/// Returns the command info at specified index.
		/// </summary>
		public static CommandInfo GetCommand(int index) {
			Initialize();

			return Commands[index];
		}

		/// <summary>
		/// Clears all recorded commands.
		/// </summary>
		public static void ClearCommands() {
			Initialize();

			Commands.Clear();
		}
	}
}

