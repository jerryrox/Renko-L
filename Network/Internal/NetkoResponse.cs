using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using Renko.Data;
using Renko.Network.Internal;

namespace Renko.Network.Internal
{
	/// <summary>
	/// Holds response data of a NetkoItem.
	/// </summary>
	public class NetkoResponse : INetkoResponse {

		/// <summary>
		/// The request manager in item.
		/// </summary>
		private NetkoRequest request;


		/// <summary>
		/// Returns the response code from requested server.
		/// If called on AudioStream request, it may return 0.
		/// </summary>
		public long Code {
			get {
				if(request.UnityRequest != null)
					return request.UnityRequest.responseCode;
				if(request.AudioRequest != null)
					return request.AudioRequest.ResponseHeaders.GetResponseCode();
				return 0;
			}
		}

		/// <summary>
		/// Returns whether the request is successful.
		/// </summary>
		public bool IsSuccess {
			get {
				if(!string.IsNullOrEmpty(request.Error))
					return false;
				if(request.UnityRequest != null)
					return !request.UnityRequest.isError;
				if(request.AudioRequest != null)
					return request.AudioRequest.IsError;
				return false;
			}
		}

		/// <summary>
		/// Returns a readable error message.
		/// </summary>
		public string ErrorMessage {
			get {
				if(!string.IsNullOrEmpty(request.Error))
					return request.Error;
				if(request.UnityRequest != null)
					return request.UnityRequest.error;
				if(request.AudioRequest != null)
					return request.AudioRequest.ErrorMessage;
				return null;
			}
		}

		/// <summary>
		/// Returns the response text data.
		/// </summary>
		public string TextData {
			get {
				if(request.UnityRequest != null && request.UnityRequest.downloadHandler != null)
					return request.UnityRequest.downloadHandler.text;
				if(request.AudioRequest != null)
					return request.AudioRequest.TextData;
				return null;
			}
		}

		/// <summary>
		/// Returns the response byte data.
		/// </summary>
		public byte[] ByteData {
			get {
				if(request.UnityRequest != null && request.UnityRequest.downloadHandler != null)
					return request.UnityRequest.downloadHandler.data;
				if(request.AudioRequest != null)
					return request.AudioRequest.ByteData;
				return null;
			}
		}

		/// <summary>
		/// Returns the response audio (full/stream) data.
		/// </summary>
		public AudioClip AudioData {
			get {
				if(request.UnityRequest != null)
					return DownloadHandlerAudioClip.GetContent(request.UnityRequest);
				if(request.AudioRequest != null)
					return request.AudioRequest.AudioData;
				return null;
			}
		}

		/// <summary>
		/// Returns the response asset bundle data.
		/// </summary>
		public AssetBundle AssetBundleData {
			get {
				if(request.UnityRequest != null)
					return DownloadHandlerAssetBundle.GetContent(request.UnityRequest);
				return null;
			}
		}

		/// <summary>
		/// Returns the response texture data.
		/// </summary>
		public Texture2D TextureData {
			get {
				if(request.UnityRequest != null)
					return DownloadHandlerTexture.GetContent(request.UnityRequest);
				return null;
			}
		}

		/// <summary>
		/// Returns the response headers.
		/// </summary>
		public Dictionary<string,string> ResponseHeaders {
			get {
				if(request.UnityRequest != null)
					return request.UnityRequest.GetResponseHeaders();
				if(request.AudioRequest != null)
					return request.AudioRequest.ResponseHeaders;
				return null;
			}
		}

		/// <summary>
		/// Returns the Content-Type value from header.
		/// </summary>
		public string ContentType {
			get {
				var headers = ResponseHeaders;
				if(headers == null)
					return null;
				var type = headers.FirstOrDefault(
					pair => pair.Key.ToLower().Equals("content-type")
				);
				return type.Value;
			}
		}

		/// <summary>
		/// Returns the Content-Length value from header.
		/// </summary>
		public long ContentLength {
			get {
				var headers = ResponseHeaders;
				if(headers == null)
					return 0;
				var length = headers.FirstOrDefault(
					pair => pair.Key.ToLower().Equals("content-length")
				);
				return length.Value == null ? 0 : length.Value.ParseLong();
			}
		}


		public NetkoResponse(NetkoItem item) {
			this.request = item.Request as NetkoRequest;
		}

	}
}

