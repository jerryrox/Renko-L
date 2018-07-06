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
		public static NetkoItem Get(string url, string parameters = "", int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Get,
				url,
				HttpMethods.GET,
				timeOut
			);
			info.UrlParam = parameters;
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new POST request.
		/// </summary>
		public static NetkoItem Post(string url, WWWForm form, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Post,
				url,
				HttpMethods.POST,
				timeOut
			);
			info.Form = form;
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new DELETE request.
		/// </summary>
		public static NetkoItem Delete(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Delete,
				url,
				HttpMethods.DELETE,
				timeOut
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new PUT request.
		/// </summary>
		public static NetkoItem Put(string url, string putData, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Put,
				url,
				HttpMethods.PUT,
				timeOut,
				putData
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new HEAD request.
		/// </summary>
		public static NetkoItem Head(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Head,
				url,
				HttpMethods.HEAD,
				timeOut
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio.
		/// </summary>
		public static NetkoItem GetAudio(string url, int groupId = 0, float timeOut = 60f, AudioType audioType = AudioType.UNKNOWN) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Audio,
				url,
				HttpMethods.GET,
				timeOut,

				audioType:audioType
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio stream.
		/// </summary>
		public static NetkoItem GetAudioStream(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AudioStream,
				url,
				HttpMethods.GET,
				timeOut
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for asset bundle.
		/// </summary>
		public static NetkoItem GetAssetBundle(string url, int groupId = 0, float timeOut = 60f, uint bundleVersion = 0, uint bundleCRC = 0) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AssetBundle,
				url,
				HttpMethods.GET,
				timeOut,

				assetBundleVersion:bundleVersion,
				assetBundleCRC:bundleCRC
			);
			return new NetkoItem(groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for texture.
		/// </summary>
		public static NetkoItem GetTexture(string url, int groupId = 0, float timeOut = 60f, bool textureNonReadable = true) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Texture,
				url,
				HttpMethods.GET,
				timeOut,

				textureNonReadable:textureNonReadable
			);
			return new NetkoItem(groupId, info);
		}
	}
}