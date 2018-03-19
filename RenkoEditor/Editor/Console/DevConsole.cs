using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RenkoEditor.Console.Internal;

namespace RenkoEditor.Console
{
	public static class DevConsole {
		
		/// <summary>
		/// A delegate for handling callback action when a command evaluation is finished.
		/// </summary>
		public delegate void EvaluationEndHandler(object returnData);


		/// <summary>
		/// Evauates the specified command.
		/// </summary>
		public static void Evaluate(string command, EvaluationEndHandler callback = null) {
			// Removing empty spaces for some consistency.
			command = command.Trim();

			try {
				OutputHistory.AddCommandOutput(command);

				CommandInfo commandInfo = new CommandInfo(command);

				// Receive custom command input first,
				// then go for code compilation if not a custom command.
				if(!CustomCommandProcessor.Process(commandInfo))
					CommandCompiler.Compile(commandInfo);

				CommandHistory.AddCommand(commandInfo);
			}
			catch(Exception e) {
				OutputHistory.AddResultOutput("An exception occured while evaluating: " + e.Message);
			}
		}
	}
}