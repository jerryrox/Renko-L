#if NGUI
using System;
using System.Text;
using System.IO;
using Renko.IO;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// Code auto-generator for MVC class.
	/// </summary>
	public static class MvcAutoGen {

		private const string ScriptFileName = "MVC.AutoGen.cs";
		private const string FirstViewLine = "\t\t\tfirstViewType = typeof({0});";
		private const string MetaListLine = "\t\t\tuiMetadatas = new Dictionary<Type, MvcViewMeta>({0});";
		private const string ViewMetaLine = "\t\t\tuiMetadatas.Add(typeof({0}), new MvcViewMeta(this, MvcLifeType.{1}, MvcRescaleType.{2}, \"{3}\", ViewParent));";

		private const string FirstViewReplaceTarget = "|0|";
		private const string MetaListReplaceTarget = "|1|";
		private const string ViewMetaReplaceTarget = "|2|";


		/// <summary>
		/// Path to the MVC autogen code template.
		/// </summary>
		private static string TemplateFilePath {
			get { return NyanPath.GetLibraryPath("Framework/MVC/Templates/MVCAutogen.txt"); }
		}


		/// <summary>
		/// Creates an MVC autogen script file.
		/// </summary>
		public static void Create(string autogenPath, MvcConfig config) {
			string scriptPath = Path.Combine(autogenPath, ScriptFileName);
			File.WriteAllText(scriptPath, GetScriptContents(config));
		}

		/// <summary>
		/// Returns the script content.
		/// </summary>
		private static string GetScriptContents(MvcConfig config) {
			StringBuilder baseSB = new StringBuilder(File.ReadAllText(TemplateFilePath));

			// Variable setup
			var initialView = config.InitialView;
			var views = config.Views;

			// Initial view setup
			if(initialView != null) {
				baseSB.Replace("|0|", string.Format(FirstViewLine, initialView.ViewName));
			}
			else {
				baseSB.Replace(FirstViewReplaceTarget, "");
			}

			// Metadata list instantiation setup
			baseSB.Replace(MetaListReplaceTarget, string.Format(
				MetaListLine, views.Count
			));

			// Views setup
			StringBuilder viewsSB = new StringBuilder();
			for(int i=0; i<views.Count; i++)  {
				viewsSB.AppendLine(string.Format(
					ViewMetaLine, views[i].ViewName, views[i].LifeType, views[i].ViewRescaleMode, views[i].ResourcePath
				));
			}
			baseSB.Replace(ViewMetaReplaceTarget, viewsSB.ToString());

			return baseSB.ToString();
		}
	}
}
#endif