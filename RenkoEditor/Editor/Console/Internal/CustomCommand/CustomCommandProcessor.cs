using System;
using RenkoEditor.Console.Internal.CustomCommand;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// A class for handling DevConsole's custom commands.
	/// </summary>
	public static class CustomCommandProcessor {

		/// <summary>
		/// Array of custom commands available for DevConsole.
		/// </summary>
		public static ICustomCommander[] Commands = new ICustomCommander[] {
			new HelpCommand(),
			new NamespaceCommand()
		};


		/// <summary>
		/// Tries processing a DevConsole custom command using specified command.
		/// Returns true if custom command was successfully processed.
		/// </summary>
		public static bool Process(CommandInfo command) {
			var inputs = command.OriginalString.Split(' ');
			if(inputs == null || inputs.Length < 1)
				return false;

			return DispatchCommand(inputs);
		}

		/// <summary>
		/// Checks for validity of the command and executes it.
		/// Returns true if successful.
		/// </summary>
		public static bool DispatchCommand(string[] inputs) {
			var commandHandler = FindCommandHandler(inputs[0]);
			if(commandHandler == null)
				return false;
			
			CustomCommandInfo info = new CustomCommandInfo(inputs);
			commandHandler.Process(info);
			return true;

		}

		/// <summary>
		/// Finds the handler for specified command.
		/// </summary>
		private static ICustomCommander FindCommandHandler(string command) {
			for(int i=0; i<Commands.Length; i++) {
				if(Commands[i].CommandName.Equals(command))
					return Commands[i];
			}
			return null;
		}
	}
}

