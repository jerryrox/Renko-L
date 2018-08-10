using UnityEngine;
using UnityEngine.Networking;
using Renko.Network.Internal;
using Renko.Diagnostics;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A class that manages requests for NetkoItem.
	/// </summary>
	public class NetkoRequest : INetkoRequest {

		/// <summary>
		/// Web request object for almost all request types.
		/// </summary>
		public UnityWebRequest UnityRequest;

		/// <summary>
		/// Web request object for audio streams only.
		/// </summary>
		public AudioStreamRequest AudioRequest;

		/// <summary>
		/// Backing field of RequestInfo property.
		/// </summary>
		private NetkoRequestInfo requestInfo;

		/// <summary>
		/// Whether this item is processing or not.
		/// </summary>
		private bool isProcessing;

		/// <summary>
		/// A string value that represents a custom error message.
		/// </summary>
		private string customError;


		/// <summary>
		/// Returns the request type being performed.
		/// </summary>
		public Netko.RequestType Type {
			get { return requestInfo.RequestType; }
		}

		/// <summary>
		/// Returns the url which this item is making a request to.
		/// </summary>
		public string Url {
			get {
				if(UnityRequest != null)
					return UnityRequest.url;
				else if(AudioRequest != null)
					return AudioRequest.Url;
				return null;
			}
		}

		/// <summary>
		/// Returns the request method currently being used.
		/// </summary>
		public string Method {
			get { return requestInfo.Method; }
		}

		/// <summary>
		/// Returns a readable message of request errors.
		/// </summary>
		public string Error {
			get { return customError; }
		}

		/// <summary>
		/// Returns the current download progress from 0~1.
		/// </summary>
		public float DownloadProgress {
			get {
				if(UnityRequest != null)
					return UnityRequest.downloadProgress;
				if(AudioRequest != null)
					return AudioRequest.DownloadProgress;
				return 0f;
			}
		}

		/// <summary>
		/// Returns the current upload progress from 0~1.
		/// If called on AudioStream request, it will always return 0.
		/// </summary>
		public float UploadProgress {
			get {
				if(UnityRequest != null)
					return UnityRequest.uploadProgress;
				return 0f;
			}
		}

		/// <summary>
		/// Returns either DownloadProgress or UploadProgress based on current request.
		/// </summary>
		public float Progress {
			get { return DownloadProgress > UploadProgress ? DownloadProgress : UploadProgress; }
		}

		/// <summary>
		/// Time in seconds which makes this item end with a timeout error.
		/// Relative to Time.realtimeSinceStartup.
		/// </summary>
		public float TimeOutTime {
			get; set;
		}

		/// <summary>
		/// Returns whether this item is terminated.
		/// </summary>
		public bool IsTerminated {
			get { return UnityRequest == null && AudioRequest == null; }
		}

		/// <summary>
		/// Returns whether this item is being processed.
		/// </summary>
		public bool IsProcessing {
			get { return isProcessing; }
		}

		/// <summary>
		/// Returns whether the request is finished.
		/// May be "finished" earlier due to explicit errors.
		/// </summary>
		public bool IsFinished {
			get {
				if(!string.IsNullOrEmpty(customError))
					return true;
				if(UnityRequest != null)
					return UnityRequest.isDone;
				if(AudioRequest != null)
					return AudioRequest.IsDone;
				return false;
			}
		}

		/// <summary>
		/// Returns whether the request has timed out.
		/// </summary>
		public bool IsTimeOut {
			get { return TimeOutTime < Time.realtimeSinceStartup; }
		}


		public NetkoRequest(NetkoRequestInfo requestInfo) {
			this.requestInfo = requestInfo;
		}

		/// <summary>
		/// Setup request.
		/// </summary>
		public void Setup() {
			// Determine which request to use.
			switch(requestInfo.RequestType) {
			case Netko.RequestType.Get:
				UnityRequest = UnityWebRequest.Get(requestInfo.Url.GetUriEscaped());
				break;
			case Netko.RequestType.Post:
				UnityRequest = UnityWebRequest.Post(requestInfo.Url.GetUriEscaped(), requestInfo.Form);
				if(requestInfo.UploadHandler != null)
					UnityRequest.uploadHandler = requestInfo.UploadHandler;
				break;
			case Netko.RequestType.Delete:
				UnityRequest = UnityWebRequest.Delete(requestInfo.Url.GetUriEscaped());
				break;
			case Netko.RequestType.Put:
				UnityRequest = UnityWebRequest.Put(requestInfo.Url.GetUriEscaped(), requestInfo.PutData);
				break;
			case Netko.RequestType.Head:
				UnityRequest = UnityWebRequest.Head(requestInfo.Url.GetUriEscaped());
				break;
			case Netko.RequestType.Audio:
				UnityRequest = UnityWebRequest.GetAudioClip(requestInfo.Url.GetUriEscaped(), requestInfo.AudioType);
				break;
			case Netko.RequestType.AudioStream:
				AudioRequest = new AudioStreamRequest(requestInfo.Url.GetUriEscaped(true));
				break;
			case Netko.RequestType.Texture:
				UnityRequest = UnityWebRequest.GetTexture(
					requestInfo.Url.GetUriEscaped(), requestInfo.TextureNonReadable
				);
				break;
			case Netko.RequestType.AssetBundle:
				UnityRequest = UnityWebRequest.GetAssetBundle(
					requestInfo.Url.GetUriEscaped(), requestInfo.AssetBundleVersion, requestInfo.AssetBundleCRC
				);
				break;
			}

			// Initialize variables
			SetError(null);
			isProcessing = false;
			TimeOutTime = requestInfo.TimeOut;
		}

		/// <summary>
		/// Sends the request.
		/// </summary>
		public void Send() {
			// Send request.
			if(UnityRequest != null)
				UnityRequest.Send();
			else if(AudioRequest != null)
				AudioRequest.Send();
			isProcessing = true;

			//Start timeout detection
			TimeOutTime += Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Terminates requests.
		/// </summary>
		public void Terminate() {
			if(UnityRequest != null)
				UnityRequest.Abort();
			else if(AudioRequest != null)
				AudioRequest.Dispose();
			UnityRequest = null;
			AudioRequest = null;
		}

		/// <summary>
		/// Sets request error message.
		/// </summary>
		public void SetError(string error) {
			customError = error;
		}

		/// <summary>
		/// Checks for any request error.
		/// </summary>
		public void CheckError() {
			//Timeout error
			if(IsTimeOut) {
				SetError("The request has timed out.");
				RenLog.Log(
					"NetkoRequest.CheckError - Timeout error at url: {1}" + Url
				);
			}
		}
	}
}

