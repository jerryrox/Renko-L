using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Renko.Utility
{
	/// <summary>
	/// A class for loading assets from resources folder.
	/// </summary>
	public static class ResourceLoader {

		/// <summary>
		/// Loads an audio from resources.
		/// </summary>
		public static AudioClip LoadAudioClip(string path) {
			return Resources.Load(path, typeof(AudioClip)) as AudioClip;
		}

		/// <summary>
		/// Loads a texture2d from resources.
		/// </summary>
		public static Texture2D LoadTexture2D(string path) {
			return Resources.Load(path, typeof(Texture2D)) as Texture2D;
		}

		/// <summary>
		/// Loads a gameobject from resources.
		/// </summary>
		public static GameObject LoadGameObject(string path) {
			return Resources.Load(path, typeof(GameObject)) as GameObject;
		}

		#if NGUI
		/// <summary>
		/// Loads a gameobject from resources, instantiates it, and returns the gameobject.
		/// </summary>
		public static GameObject CreateObject(GameObject parent, string path) {
			return NGUITools.AddChild( parent, LoadGameObject(path) );
		}
		#endif

		/// <summary>
		/// Loads a text asset from resources.
		/// </summary>
		public static TextAsset LoadTextAsset(string path) {
			return Resources.Load(path, typeof(TextAsset)) as TextAsset;
		}

		/// <summary>
		/// Loads a generic type of object from resources.
		/// </summary>
		public static T Load<T>(string path) where T : Object {
			return Resources.Load(path, typeof(T)) as T;
		}

		/// <summary>
		/// Unloads the specified object.
		/// Equivalent to Resources.UnloadAsset method.
		/// </summary>
		public static void Unload(Object obj) {
			Resources.UnloadAsset(obj);
		}
	}
}