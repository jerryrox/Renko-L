using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A simple wrapper over the legacy WWW class for Audio streams only.
	/// </summary>
	public class AudioStreamRequest : IDisposable {

		/// <summary>
		/// The WWW object.
		/// </summary>
		public WWW Request { get; private set; }
		/// <summary>
		/// The target url which the WWW object will request on.
		/// </summary>
		public string Url { get; private set; }
		public string ErrorMessage {
			get {
				if(Request != null)
					return Request.error;
				return null;
			}
		}
		public string TextData {
			get {
				if(Request != null)
					return Request.text;
				return null;
			}
		}
		public float DownloadProgress {
			get {
				if(Request != null)
					return Request.progress;
				return 0f;
			}
		}
		public bool IsDone {
			get {
				if(Request != null)
					return Request.isDone;
				return false;
			}
		}
		public bool IsError {
			get {
				if(Request != null)
					return string.IsNullOrEmpty(Request.error);
				return false;
			}
		}
		public byte[] ByteData {
			get {
				if(Request != null)
					return Request.bytes;
				return null;
			}
		}
		public Dictionary<string,string> ResponseHeaders {
			get {
				if(Request != null)
					return Request.responseHeaders;
				return null;
			}
		}
		public AudioClip AudioData {
			get {
				if(Request != null)
					return Request.GetAudioClip(false, true, AudioType.MPEG);
				return null;
			}
		}


		public AudioStreamRequest(string url) {
			Url = url;
		}

		/// <summary>
		/// Sends the request.
		/// </summary>
		public void Send() {
			Request = new WWW(Url);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Renko.Network.AudioStreamRequest"/> object.
		/// </summary>
		public void Dispose() { if(Request != null) Request.Dispose(); }
	}
}

