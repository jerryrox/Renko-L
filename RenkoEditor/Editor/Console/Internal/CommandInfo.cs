using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Renko.Extensions;
using Renko.Utility;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// Metadata of a command.
	/// </summary>
	public class CommandInfo : IJsonable {
		
		/// <summary>
		/// The command string after processing the OriginalString.
		/// </summary>
		public string ProcessedString;

		/// <summary>
		/// The original command string value that was entered on console.
		/// </summary>
		public string OriginalString;

		/// <summary>
		/// List of variable names that are being assigned to a specific value.
		/// </summary>
		public List<string> AssignedVariableNames;

		/// <summary>
		/// List of variable names that are being referenced for further use within their context.
		/// </summary>
		public List<string> ReferencedVariableNames;


		public CommandInfo() {}

		public CommandInfo(string command) {
			OriginalString = command;
			AssignedVariableNames = new List<string>(2);
			ReferencedVariableNames = new List<string>(2);

			ProcessCommandString();
		}

		/// <summary>
		/// Resets all fields and processes the original command again.
		/// </summary>
		public void ReprocessCommand() {
			ProcessedString = null;
			AssignedVariableNames.Clear();
			ReferencedVariableNames.Clear();

			ProcessCommandString();
		}

		public JsonObject ToJsonObject () {
			JsonObject json = new JsonObject();
			json["ProcessedString"] = ProcessedString;
			json["OriginalString"] = OriginalString;
			json["AssignedVariableNames"] = new JsonArray(AssignedVariableNames);
			json["ReferencedVariableNames"] = new JsonArray(ReferencedVariableNames);
			return json;
		}

		public void FromJsonObject (JsonObject value) {
			ProcessedString = value["ReferencedVariableNames"].AsString();
			OriginalString = value["OriginalString"].AsString();

			var assignedVarNames = value["AssignedVariableNames"].AsArray();
			for(int i=0; i<assignedVarNames.Count; i++) {
				AssignedVariableNames.Add(assignedVarNames[i].AsString());
			}

			var referencedVarNames = value["ReferencedVariableNames"].AsArray();
			for(int i=0; i<referencedVarNames.Count; i++) {
				ReferencedVariableNames.Add(referencedVarNames[i].AsString());
			}
		}

		/// <summary>
		/// Processes the original command string to parse some values.
		/// </summary>
		private void ProcessCommandString() {
			StringBuilder sb = new StringBuilder();
			bool parsingString = false;
			bool parsingChar = false;
			bool parsingVar = false;
			string curVariable = null;

			for(int i=0; i<OriginalString.Length; i++) {
				char prevCh = (i > 0 ? OriginalString[i-1] : ' ');
				char ch = OriginalString[i];

				// Encountered a string or char literal that is not escaped.
				if(prevCh != '\\') {
					if(ch == '"') {
						parsingString = !parsingString;
					}
					else if(ch == '\'') {
						parsingChar = !parsingChar;
					}
				}

				// Any string values inside the string or char literals must be preserved as-is.
				if(parsingString || parsingChar)
					sb.Append(ch);
				else {
					// Parsing variables
					if(!parsingVar) {
						// Should we start parsing a new variable name?
						if(ch == '$') {
							curVariable = "$";
							parsingVar = true;
						}
					}
					else {
						string charString = null;
						do {
							ch = OriginalString[i++];
							charString = char.ToString(ch);
						}
						while(char.IsWhiteSpace(ch) && i < OriginalString.Length);
						if(i >= OriginalString.Length)
							break;

						// Checking for different ending characters.
						// The variable should keep registering.
						if(Regex.IsMatch(charString, "[a-zA-Z_0-9]")) {
							curVariable += charString;
						}
						// End of variable parsing.
						else {
							// The variable is being assigned to a certain value.
							if(ch == '=') {
								AssignedVariableNames.Add(curVariable);
							}
							// The variable is simply being referenced for further use within its context.
							else {
								ReferencedVariableNames.Add(curVariable);
							}
							parsingVar = false;
						}
					}

					sb.Append(ch);
				}
			}

			// Finalizing variable parse manually if original string doesn't end with a special character.
			if(parsingVar)
				ReferencedVariableNames.Add(curVariable);

			// End with semicolon if not.
			if(sb[sb.Length-1] != ';')
				sb.Append(';');

			// Finalize command string process
			ProcessedString = sb.ToString();
			ReplaceVariables();
		}

		/// <summary>
		/// Replaces all variable names to compile-ready format.
		/// </summary>
		private void ReplaceVariables() {
			for(int i=0; i<AssignedVariableNames.Count; i++) {
				string curName = AssignedVariableNames[i];

				ProcessedString = ProcessedString.Replace(
					curName,
					string.Format(
						"{0}[\"{1}\"]",
						CommandCompiler.VariablesReferenceName,
						curName
					)
				);
			}

			for(int i=0; i<ReferencedVariableNames.Count; i++) {
				string curName = ReferencedVariableNames[i];
				var varInfo = VariableProvider.GetVariable(curName);

				if(varInfo == null)
					throw new Exception("Variable " + curName + " is undefined.");

				ProcessedString = ProcessedString.Replace(
					curName,
					string.Format(
						"(({0}){1}[\"{2}\"])",
						varInfo.ObjectType.FullName,
						CommandCompiler.VariablesReferenceName,
						curName
					)
				);
			}
		}
	}
}

