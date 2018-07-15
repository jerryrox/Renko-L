#if NGUI
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Renko.IO;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// Code auto-generator for MVC View, Model, and Controller.
	/// </summary>
	public static class MvcViewAutoGen {

		private const string BaseFileName = "{0}.AutoGen.cs";
		private const string ViewDirectory = "{0}View";
		private const string ViewFileName = ViewDirectory+"/{0}View.cs";
		private const string ModelFileName = ViewDirectory+"/{0}Model.cs";
		private const string ControllerFileName = ViewDirectory+"/{0}Controller.cs";


		/// <summary>
		/// Creates an MVC autogen script file.
		/// </summary>
		public static void Create(string autogenPath, string workspacePath, MvcConfig.View configView) {
			// Generate base script
			string baseScriptPath = GetBaseScriptPath(autogenPath, configView.Name);
			File.WriteAllText(baseScriptPath, GetBaseScriptContents(configView));

			// Generate view, model, controller scripts.
			// They must be generated ONLY if there is no existing file.
			CreateViewDirectory(configView);
			string viewScriptPath = GetViewScriptPath(workspacePath, configView.Name);
			string modelScriptPath = GetModelScriptPath(workspacePath, configView.Name);
			string controllerScriptPath = GetControllerScriptPath(workspacePath, configView.Name);
			if(!File.Exists(viewScriptPath))
				File.WriteAllText(viewScriptPath, GetViewScriptContents(configView));
			if(!File.Exists(modelScriptPath))
				File.WriteAllText(modelScriptPath, GetModelScriptContents(configView));
			if(!File.Exists(controllerScriptPath))
				File.WriteAllText(controllerScriptPath, GetControllerScriptContents(configView));
		}

		/// <summary>
		/// Returns the path to MVC scripts directory.
		/// </summary>
		public static string GetDirectoryPath(string autogenPath, string viewName) {
			return Path.Combine(autogenPath, string.Format(ViewDirectory, viewName));
		}

		/// <summary>
		/// Returns the path to base view script file.
		/// </summary>
		public static string GetBaseScriptPath(string autogenPath, string viewName) {
			return Path.Combine(autogenPath, string.Format(BaseFileName, viewName));
		}

		/// <summary>
		/// Returns the path to the MVC View script file.
		/// </summary>
		public static string GetViewScriptPath(string workspacePath, string viewName) {
			return Path.Combine(workspacePath, string.Format(ViewFileName, viewName));
		}

		/// <summary>
		/// Returns the path to the MVC View script file.
		/// </summary>
		public static string GetModelScriptPath(string workspacePath, string viewName) {
			return Path.Combine(workspacePath, string.Format(ModelFileName, viewName));
		}

		/// <summary>
		/// Returns the path to the MVC View script file.
		/// </summary>
		public static string GetControllerScriptPath(string workspacePath, string viewName) {
			return Path.Combine(workspacePath, string.Format(ControllerFileName, viewName));
		}

		/// <summary>
		/// Creates directory for the view, model, and controller scripts.
		/// </summary>
		private static void CreateViewDirectory(MvcConfig.View configView) {
			string viewDir = MvcWorkspace.GetWorkspacePath(string.Format(ViewDirectory, configView.Name));
			if(!Directory.Exists(viewDir))
				Directory.CreateDirectory(viewDir);
		}

		/// <summary>
		/// Returns the base script content.
		/// </summary>
		private static string GetBaseScriptContents(MvcConfig.View configView) {
			StringBuilder baseSB = new StringBuilder(File.ReadAllText(TemplateFilePath("Base")));
			baseSB.Replace("{0}", configView.Name);
			baseSB.Replace("{1}", configView.GetBaseClass());
			return baseSB.ToString();
		}

		/// <summary>
		/// Returns the view script content.
		/// </summary>
		private static string GetViewScriptContents(MvcConfig.View configView) {
			StringBuilder baseSB = new StringBuilder(File.ReadAllText(TemplateFilePath("View")));
			baseSB.Replace("{0}", configView.Name);
			baseSB.Replace("{1}", configView.GetBaseClass());
			return baseSB.ToString();
		}

		/// <summary>
		/// Returns the model script content.
		/// </summary>
		private static string GetModelScriptContents(MvcConfig.View configView) {
			StringBuilder baseSB = new StringBuilder(File.ReadAllText(TemplateFilePath("Model")));
			baseSB.Replace("{0}", configView.Name);
			return baseSB.ToString();
		}

		/// <summary>
		/// Returns the controller script content.
		/// </summary>
		private static string GetControllerScriptContents(MvcConfig.View configView) {
			StringBuilder baseSB = new StringBuilder(File.ReadAllText(TemplateFilePath("Controller")));
			baseSB.Replace("{0}", configView.Name);
			return baseSB.ToString();
		}

		/// <summary>
		/// Template file path for specified type.
		/// Types are as follows:
		/// Base,
		/// View,
		/// Model,
		/// Controller
		/// </summary>
		private static string TemplateFilePath(string type) {
			return NyanPath.GetLibraryPath("Framework/MVC/Templates/"+type+"Autogen.txt");
		}
	}
}
#endif