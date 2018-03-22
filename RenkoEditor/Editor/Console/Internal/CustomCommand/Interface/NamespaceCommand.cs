using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace RenkoEditor.Console.Internal.CustomCommand
{
	public class NamespaceCommand : ICustomCommander {
		
		public string CommandName {
			get { return "namespace"; }
		}

		public string Description {
			get { return "Manages namespaces used during command compilation."; }
		}


		public void Process(CustomCommandInfo info) {
			switch(info.ActionName) {
			case "-h":
			case "-help":
				ProcessHelp();
				break;

			case "-ls":
			case "-list":
				ProcessList();
				break;

			case "-a":
			case "-add":
				ProcessAdd(info.Arguments);
				break;

			case "-rm":
			case "-remove":
				ProcessRemove(info.Arguments);
				break;

			case null:
				ProcessNull();
				break;
			}
		}

		void ProcessHelp() {
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat(
				"{0} -h\t(Displays this message.)\n",
				CommandName
			);
			sb.AppendFormat(
				"{0} -ls\t(Displays the list of namespaces to use during compilation.)\n",
				CommandName
			);
			sb.AppendFormat(
				"{0} -a arg\t(Adds the argument to the namespace list for compilation.)\n",
				CommandName
			);

			OutputHistory.AddResultOutput(sb.ToString());
		}

		void ProcessList() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Namespaces:");

			var namespaces = CommandCompiler.GetNamespaces();
			for(int i=0; i<namespaces.Count; i++) {
				sb.AppendLine(namespaces[i]);
			}

			OutputHistory.AddResultOutput(sb.ToString());
		}

		void ProcessAdd(string[] arguments) {
			StringBuilder sb = new StringBuilder();

			var namespaces = CommandCompiler.GetNamespaces();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			for(int i=0; i<arguments.Length; i++) {
				var curArg = arguments[i];

				if(namespaces.Contains(curArg)) {
					sb.AppendFormat("Namespace {0} already exists.\n", curArg);
					continue;
				}

				if(!IsValidNamespace(assemblies, curArg)) {
					sb.AppendFormat("Namespace {0} is not valid.\n", curArg);
					continue;
				}

				sb.AppendFormat("Added namespace {0}.\n", curArg);
				namespaces.Add(curArg);
			}

			SaveNamespaces(namespaces);

			OutputHistory.AddResultOutput(sb.ToString());
		}

		void ProcessRemove(string[] arguments) {
			var namespaces = CommandCompiler.GetNamespaces();

			int removedCount = 0;
			for(int i=0; i<arguments.Length; i++) {
				if(namespaces.Remove(arguments[i])) {
					removedCount ++;
				}
			}

			SaveNamespaces(namespaces);

			OutputHistory.AddResultOutput(string.Format(
				"Removed {0} namespaces.",
				removedCount
			));
		}

		void ProcessNull() {
			OutputHistory.AddResultOutput(string.Format(
				"Enter \"{0} -h\" for help.", CommandName
			));
		}

		/// <summary>
		/// Returns whether specified namespace string is valid.
		/// </summary>
		private bool IsValidNamespace(Assembly[] assemblies, string ns) {
			for(int i=0; i<assemblies.Length; i++) {
				var types = assemblies[i].GetTypes();
				for(int c=0; c<types.Length; c++) {
					if(types[c].Namespace == ns)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Saves specified namespace list to configuration.
		/// </summary>
		private void SaveNamespaces(List<string> namespaces) {
			StringBuilder sb = new StringBuilder();
			for(int i=0; i<namespaces.Count; i++)
				sb.AppendLine(namespaces[i]);
			
			Configurations.Namespaces = sb.ToString();
		}
	}
}

