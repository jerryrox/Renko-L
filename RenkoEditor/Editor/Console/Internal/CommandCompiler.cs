using System;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using CSharpCompiler;
using UnityEngine;
using Renko.Utility;

using CodeCompiler = CSharpCompiler.CodeCompiler;

namespace RenkoEditor.Console.Internal
{
	public static class CommandCompiler {

		/// <summary>
		/// Field name that points to VariableProvider dictionary.
		/// </summary>
		public const string VariablesReferenceName = "CommandVariables";

		/// <summary>
		/// List of namespaces to declare during compilation.
		/// </summary>
		private static List<string> Namespaces;

		private static bool IsInitialized = false;


		public static void Initialize() {
			if(IsInitialized)
				return;

			IsInitialized = true;

			Namespaces = new List<string>();
			using(StringReader sr = new StringReader(Configurations.Namespaces)) {
				while(sr.Peek() > -1)
					Namespaces.Add(sr.ReadLine());
			}
		}

		/// <summary>
		/// Compiles the specified command.
		/// </summary>
		public static void Compile(CommandInfo commandInfo) {
			Initialize();

			var assembly = CompileToAssembly(GetSourceString(commandInfo.ProcessedString));
			var method = assembly.GetType("DevConsole_Commander").GetMethod("DoCommand");
			var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
			del.Invoke();
		}

		/// <summary>
		/// Returns the list of namespaces to use during compilation.
		/// </summary>
		public static List<string> GetNamespaces() {
			Initialize();
			return Namespaces;
		}

		/// <summary>
		/// Compiles the specified code to an assembly.
		/// </summary>
		private static Assembly CompileToAssembly(string source) {
			var provider = new CodeCompiler();
			var param = new CompilerParameters();

			// Add ALL of the assembly references
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				param.ReferencedAssemblies.Add(assembly.Location);
			}

			// Generate a dll in memory
			param.GenerateExecutable = false;
			param.GenerateInMemory = true;

			// Compile the source
			var result = provider.CompileAssemblyFromSource(param, source);

			// Handle compile errors
			if (result.Errors.Count > 0) {
				var msg = new StringBuilder();
				msg.AppendLine("Compiler errors occured:");
				foreach (CompilerError error in result.Errors) {
					msg.AppendFormat("Error ({0}): {1}\n", error.ErrorNumber, error.ErrorText);
				}
				throw new Exception(msg.ToString());
			}

			// Return the assembly
			return result.CompiledAssembly;
		}

		/// <summary>
		/// Returns the full source that executes the specified command.
		/// </summary>
		private static string GetSourceString(string command) {
			StringBuilder sb = new StringBuilder();
			 
			for(int i=0; i<Namespaces.Count; i++) {
				sb.AppendFormat("using {0};\n", Namespaces[i]);
			}
			sb.AppendLine("using RenkoEditor.Console.Internal;");

			string code = (
@"
public class DevConsole_Commander {
	public static void DoCommand() {
		var CommandVariables = VariableProvider.Variables;
		{0}
	}
	public static void Log(object obj) {
		OutputHistory.AddResultOutput(obj.ToString());
	}
}
"
			).Replace("{0}", command);
			sb.Append(code);

			return sb.ToString();
		}
	}
}

