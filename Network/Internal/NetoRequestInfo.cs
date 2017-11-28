using System;
using UnityEngine;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A derivent of WebRequestInfo exclusively for Netko.
	/// </summary>
	public class NetkoRequestInfo : WebRequestInfo {

		public Netko.RequestType RequestType {
			get; set;
		}
		public float TimeOut {
			get; set;
		}
		public string PutData {
			get; set;
		}
		public AudioType AudioType {
			get; set;
		}
		public uint AssetBundleVersion {
			get; set;
		}
		public uint AssetBundleCRC {
			get; set;
		}
		public bool TextureNonReadable {
			get; set;
		}


		public NetkoRequestInfo(
			Netko.RequestType requestType,
			string _url,
			string _method,
			float timeOut,
			string putData = null,
			AudioType audioType = AudioType.UNKNOWN,
			uint assetBundleVersion = 0,
			uint assetBundleCRC = 0,
			bool textureNonReadable = true) : base(_url, _method) {

			RequestType = requestType;
			TimeOut = timeOut;
			PutData = putData;
			AudioType = audioType;
			AssetBundleVersion = assetBundleVersion;
			AssetBundleCRC = assetBundleCRC;
			TextureNonReadable = textureNonReadable;
		}
	}
}

