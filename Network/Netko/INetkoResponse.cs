using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Network
{
	/// <summary>
	/// Interface for netko item's response info.
	/// </summary>
	public interface INetkoResponse {

		/// <summary>
		/// Returns the response code from requested server.
		/// </summary>
		long Code { get; }

		/// <summary>
		/// Returns whether the request is successful.
		/// </summary>
		bool IsSuccess { get; }

		/// <summary>
		/// Returns a readable error message.
		/// </summary>
		string ErrorMessage { get; }

		/// <summary>
		/// Returns the response text data.
		/// </summary>
		string TextData { get; }

		/// <summary>
		/// Returns the response byte data.
		/// </summary>
		byte[] ByteData { get; }

		/// <summary>
		/// Returns the response audio (full/stream) data.
		/// </summary>
		AudioClip AudioData { get; }

		/// <summary>
		/// Returns the response asset bundle data.
		/// </summary>
		AssetBundle AssetBundleData { get; }

		/// <summary>
		/// Returns the response texture data.
		/// </summary>
		Texture2D TextureData { get; }

		/// <summary>
		/// Returns the response headers.
		/// </summary>
		Dictionary<string, string> ResponseHeaders { get; }

		/// <summary>
		/// Returns the Content-Type value from header.
		/// </summary>
		string ContentType { get; }

		/// <summary>
		/// Returns the Content-Length value from header.
		/// </summary>
		long ContentLength { get; }
	}
}