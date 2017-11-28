using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Network.Internal;

namespace Renko.Network
{
	public partial class Netko : MonoBehaviour {
		
		/// <summary>
		/// Makes a get new GET request.
		/// </summary>
		public static Item Get(string url, string parameters = "", int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Get,
				url,
				HttpVerb.GET,
				timeOut
			);
			info.UrlParam = parameters;

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new POST request.
		/// </summary>
		public static Item Post(string url, WWWForm form, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Post,
				url,
				HttpVerb.POST,
				timeOut
			);
			info.Form = form;

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new DELETE request.
		/// </summary>
		public static Item Delete(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Delete,
				url,
				HttpVerb.DELETE,
				timeOut
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new PUT request.
		/// </summary>
		public static Item Put(string url, string putData, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Put,
				url,
				HttpVerb.PUT,
				timeOut,
				putData
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new HEAD request.
		/// </summary>
		public static Item Head(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Head,
				url,
				HttpVerb.HEAD,
				timeOut
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio.
		/// </summary>
		public static Item GetAudio(string url, int groupId = 0, float timeOut = 60f, AudioType audioType = AudioType.UNKNOWN) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Audio,
				url,
				HttpVerb.GET,
				timeOut,

				audioType:audioType
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio stream.
		/// </summary>
		public static Item GetAudioStream(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AudioStream,
				url,
				HttpVerb.GET,
				timeOut
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for asset bundle.
		/// </summary>
		public static Item GetAssetBundle(string url, int groupId = 0, float timeOut = 60f, uint bundleVersion = 0, uint bundleCRC = 0) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AssetBundle,
				url,
				HttpVerb.GET,
				timeOut,

				assetBundleVersion:bundleVersion,
				assetBundleCRC:bundleCRC
			);

			return Item.Create(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for texture.
		/// </summary>
		public static Item GetTexture(string url, int groupId = 0, float timeOut = 60f, bool textureNonReadable = true) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Texture,
				url,
				HttpVerb.GET,
				timeOut,

				textureNonReadable:textureNonReadable
			);

			return Item.Create(groupId, info);
		}
	}
}