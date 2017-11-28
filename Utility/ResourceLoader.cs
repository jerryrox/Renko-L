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
			return (AudioClip)Resources.Load(path, typeof(AudioClip));
		}

		/// <summary>
		/// Loads a texture2d from resources.
		/// </summary>
		public static Texture2D LoadTexture2D(string path) {
			return (Texture2D)Resources.Load(path, typeof(Texture2D));
		}

		/// <summary>
		/// Loads a gameobject from resources.
		/// </summary>
		public static GameObject LoadGameObject(string path) {
			return (GameObject)Resources.Load(path, typeof(GameObject));
		}

		/// <summary>
		/// Loads a text asset from resources.
		/// </summary>
		public static TextAsset LoadTextAsset(string path) {
			return (TextAsset)Resources.Load(path, typeof(TextAsset));
		}

		/// <summary>
		/// Loads a generic type of object from resources.
		/// </summary>
		public static T Load<T>(string path) where T : Object {
			return (T)Resources.Load(path, typeof(T));
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