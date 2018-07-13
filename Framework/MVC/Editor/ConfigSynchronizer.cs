#if NGUI
using System;
using System.IO;
using UnityEditor;
using Renko.MVCFramework.Internal;
using Renko.IO;

namespace Renko.MVCFramework
{
	public static class ConfigSynchronizer {

		/// <summary>
		/// Synchronizes project with MVC configuration.
		/// </summary>
		public static void Sync(MVC mvc, MvcConfig config) {
			config.Save();

			MvcWorkspace.AutogenMVC(config);
			for(int i=0; i<config.Views.Count; i++) {
				MvcWorkspace.AutogenView(config.Views[i]);
				MvcPrefabMaker.Create(mvc, config.Views[i]);
			}

			AssetDatabase.Refresh();
		}
	}
}
#endif