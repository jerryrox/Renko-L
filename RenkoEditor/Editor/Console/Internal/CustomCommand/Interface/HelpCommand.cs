using System;
using System.Text;

namespace RenkoEditor.Console.Internal.CustomCommand
{
	public class HelpCommand : ICustomCommander {

		public string CommandName {
			get { return "help"; }
		}

		public string Description {
			get { return "Displays this message."; }
		}


		public void Process (CustomCommandInfo info) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Interface:");
			sb.AppendLine("Enter C# command in the text area at the bottom.");
			sb.AppendLine("Press Evaluate button to execute that command.");
			sb.AppendLine("Press Clear button to clear all output history.");
			sb.AppendLine();
			sb.AppendLine("Commands:");
			for(int i=0; i<CustomCommandProcessor.Commands.Length; i++) {
				var handler = CustomCommandProcessor.Commands[i];
				sb.AppendFormat("{0} - {1}\n", handler.CommandName, handler.Description);
			}
			sb.AppendLine();
			sb.AppendLine("You can add -h or -help option after each command for their details.");

			OutputHistory.AddResultOutput(sb.ToString());
		}
	}
}

