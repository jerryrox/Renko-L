using System;

namespace RenkoEditor.Console.Internal
{
	public class OutputInfo {

		/// <summary>
		/// String value to display.
		/// </summary>
		public string Text;

		/// <summary>
		/// Whether this output is from command input.
		/// </summary>
		public bool IsCommand;


		public OutputInfo(string text, bool isCommand) {
			if(isCommand) {
				text = "> " + text;
			}
			else {
				text = ": " + text;
			}

			this.Text = text;
			this.IsCommand = isCommand;
		}
	}
}

