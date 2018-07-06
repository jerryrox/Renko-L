using System;
using UnityEngine;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A derivent of WebRequestInfo exclusively for Netko.
	/// </summary>
	public class NetkoRequestInfo : WebRequestInfo {

		/// <summary>
		/// Type of request to make in Netko.
		/// </summary>
		public RequestType RequestType;

		/// <summary>
		/// Max duration in seconds allowed before terminating a Netko request due to timeout.
		/// </summary>
		public float TimeOut;

		/// <summary>
		/// (Optional) Data to pass during a PUT request.
		/// </summary>
		public string PutData;

		/// <summary>
		/// (Optional) Type of audio to request.
		/// </summary>
		public AudioType AudioType;

		/// <summary>
		/// (Optional) Version used for validating the downloaded asset bundle.
		/// </summary>
		public uint AssetBundleVersion;

		/// <summary>
		/// (Optional) Code used for validating the downloaded asset bundle.
		/// </summary>
		public uint AssetBundleCRC;

		/// <summary>
		/// (Optional) Whether the requested texture should be non-readable.
		/// </summary>
		public bool TextureNonReadable;


		public NetkoRequestInfo(
			RequestType requestType,
			string url,
			string method,
			float timeOut,
			string putData = null,
			AudioType audioType = AudioType.UNKNOWN,
			uint assetBundleVersion = 0,
			uint assetBundleCRC = 0,
			bool textureNonReadable = true) : base(url, method) {

			RequestType = requestType;
			TimeOut = timeOut;

			// Optional data
			PutData = putData;
			AudioType = audioType;
			AssetBundleVersion = assetBundleVersion;
			AssetBundleCRC = assetBundleCRC;
			TextureNonReadable = textureNonReadable;
		}
	}
}

