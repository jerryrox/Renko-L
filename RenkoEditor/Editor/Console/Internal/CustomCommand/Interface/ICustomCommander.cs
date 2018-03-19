using System;

namespace RenkoEditor.Console.Internal
{
	public interface ICustomCommander {

		/// <summary>
		/// Returns the command name.
		/// </summary>
		string CommandName { get; }

		/// <summary>
		/// Returns the description of this command.
		/// </summary>
		string Description { get; }


		/// <summary>
		/// Process the specified info.
		/// </summary>
		void Process(CustomCommandInfo info);
	}
}

