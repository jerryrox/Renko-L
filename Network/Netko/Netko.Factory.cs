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
		public static INetkoItem Get(string url, string parameters = "", int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Get,
				url,
				HttpMethods.GET,
				timeOut
			);
			info.UrlParam = parameters;
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new POST request.
		/// </summary>
		public static INetkoItem Post(string url, WWWForm form, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Post,
				url,
				HttpMethods.POST,
				timeOut
			);
			info.Form = form;
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new POST request using a custom upload handler.
		/// </summary>
		public static INetkoItem Post(string url, UploadHandlerRaw handler, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Post,
				url,
				HttpMethods.POST,
				timeOut
			);
			info.UploadHandler = handler;
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new DELETE request.
		/// </summary>
		public static INetkoItem Delete(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Delete,
				url,
				HttpMethods.DELETE,
				timeOut
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new PUT request.
		/// </summary>
		public static INetkoItem Put(string url, string putData, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Put,
				url,
				HttpMethods.PUT,
				timeOut,
				putData
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new HEAD request.
		/// </summary>
		public static INetkoItem Head(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Head,
				url,
				HttpMethods.HEAD,
				timeOut
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio.
		/// </summary>
		public static INetkoItem GetAudio(string url, int groupId = 0, float timeOut = 60f, AudioType audioType = AudioType.UNKNOWN) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Audio,
				url,
				HttpMethods.GET,
				timeOut,

				audioType:audioType
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for audio stream.
		/// </summary>
		public static INetkoItem GetAudioStream(string url, int groupId = 0, float timeOut = 60f) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AudioStream,
				url,
				HttpMethods.GET,
				timeOut
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for asset bundle.
		/// </summary>
		public static INetkoItem GetAssetBundle(string url, int groupId = 0, float timeOut = 60f, uint bundleVersion = 0, uint bundleCRC = 0) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.AssetBundle,
				url,
				HttpMethods.GET,
				timeOut,

				assetBundleVersion:bundleVersion,
				assetBundleCRC:bundleCRC
			);
			return new NetkoItem(Netko.I, groupId, info);
		}

		/// <summary>
		/// Makes a new GET request for texture.
		/// </summary>
		public static INetkoItem GetTexture(string url, int groupId = 0, float timeOut = 60f, bool textureNonReadable = true) {
			NetkoRequestInfo info = new NetkoRequestInfo(
				RequestType.Texture,
				url,
				HttpMethods.GET,
				timeOut,

				textureNonReadable:textureNonReadable
			);
			return new NetkoItem(Netko.I, groupId, info);
		}
	}
}