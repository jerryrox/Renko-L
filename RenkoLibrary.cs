using System;
using UnityEngine;
using Renko.Extensions;
using Renko.IO;
using Renko.Network;
using Renko.Plugin;
using Renko.Threading;
using Renko.Utility;

namespace Renko
{
	/// <summary>
	/// Renko chan!
	/// </summary>
	public static class RenkoLibrary {

		public const string AuthorNickname = "jerryrox";
		public const string AuthorName = "Jerry Kim";
		public const string GithubLink = "https://github.com/jerryrox/Renko-L";


		/// <summary>
		/// Initializes all general modules.
		/// Modules that need manual initialization: IAPManager
		/// It's recommended to manually initialize the modules you only need.
		/// </summary>
		public static void Initialize(bool includePluginModules) {
			NyanPath.CreateRenkoDirectories();

			Netko.Initialize();
			RenQL.Initialize();
			UnityThread.Initialize();
			Easing.Initialize();

			if(includePluginModules) {
				GalleryPicker.Initialize();
				NativeCamera.Initialize();
				PluginTools.Initialize();
			}
		}

		/// <summary>
		/// Finds or instantiates a new GameObject with type T and returns it.
		/// </summary>
		public static T CreateModule<T>(bool dontDestoryOnLoad = true) where T : MonoBehaviour {
			T instance = GameObject.FindObjectOfType<T>();
			if(instance == null)
				instance = new GameObject("_"+typeof(T).GetName()).AddComponent<T>();
			
			if(dontDestoryOnLoad)
				GameObject.DontDestroyOnLoad(instance.gameObject);

			return instance;
		}
	}
}

