using System;
using System.Text;

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
			for(int i=0; i<arguments.Length; i++) {
				//TODO
			}
		}

		void ProcessNull() {
			OutputHistory.AddResultOutput(string.Format(
				"Enter \"{0} -h\" for help.", CommandName
			));
		}
	}
}

