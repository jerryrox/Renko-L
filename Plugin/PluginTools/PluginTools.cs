using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Plugin.Internal;

namespace Renko.Plugin
{
	/// <summary>
	/// A class that provides utility methods that aren't very important but may be useful.
	/// </summary>
	public class PluginTools : MonoBehaviour {

		/// <summary>
		/// Interface to platform-specifiec implementation.
		/// </summary>
		public static IPluginTools I {
			get; private set;
		}

		/// <summary>
		/// The instance of this class.
		/// </summary>
		private static PluginTools Instance;


		/// <summary>
		/// Initializes the plugin tools.
		/// </summary>
		public static void Initialize() {
			if(Instance == null)
				Instance = RenkoLibrary.CreateModule<PluginTools>();
			
			if(I == null) {
				#if UNITY_EDITOR
				I = new EditorPluginTools();
				#else
				if(Application.platform == RuntimePlatform.Android)
					I = new AndroidPluginTools();
				else
					I = new IosPluginTools();
				#endif

				I.Initialize();
			}
		}

		/// <summary>
		/// Releases the resources used by this plugin.
		/// </summary>
		public static void Destroy() {
			if(Instance != null)
				Destroy(Instance.gameObject);
			
			if(I != null)
				I.Destroy();
		}
	}
}