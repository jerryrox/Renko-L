using UnityEngine;
using UnityEngine.Networking;
using Renko.Network.Internal;
using Renko.Diagnostics;

namespace Renko.Network
{
	/// <summary>
	/// A class that manages requests for NetkoItem.
	/// </summary>
	public class NetkoRequest {

		/// <summary>
		/// The item containing this instance.
		/// </summary>
		private NetkoItem item;

		/// <summary>
		/// Web request object for almost all request types.
		/// </summary>
		private UnityWebRequest unityRequest;

		/// <summary>
		/// Web request object for audio streams only.
		/// </summary>
		private AudioStreamRequest audioRequest;

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
		/// Source of data to use for request.
		/// </summary>
		public NetkoRequestInfo RequestInfo {
			get { return requestInfo; }
		}

		/// <summary>
		/// Returns the direct instance of UnityWebRequest.
		/// </summary>
		public UnityWebRequest UnityRequest {
			get { return unityRequest; }
		}

		/// <summary>
		/// Returns the direct instance of AudioStreamRequest.
		/// </summary>
		public AudioStreamRequest AudioRequest {
			get { return audioRequest; }
		}

		/// <summary>
		/// Returns the url which this item is making a request to.
		/// </summary>
		public string Url {
			get {
				if(unityRequest != null)
					return unityRequest.url;
				else if(audioRequest != null)
					return audioRequest.Url;
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
		/// Returns the current download progress from 0~1.
		/// </summary>
		public float DownloadProgress {
			get {
				if(unityRequest != null)
					return unityRequest.downloadProgress;
				if(audioRequest != null)
					return audioRequest.DownloadProgress;
				return 0f;
			}
		}

		/// <summary>
		/// Returns the current upload progress from 0~1.
		/// If called on AudioStream request, it will always return 0.
		/// </summary>
		public float UploadProgress {
			get {
				if(unityRequest != null)
					return unityRequest.uploadProgress;
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
			get { return unityRequest == null && audioRequest == null; }
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
				if(unityRequest != null)
					return unityRequest.isDone;
				if(audioRequest != null)
					return audioRequest.IsDone;
				return false;
			}
		}

		/// <summary>
		/// Returns whether the request has timed out.
		/// </summary>
		public bool IsTimeOut {
			get { return TimeOutTime < Time.realtimeSinceStartup; }
		}

		/// <summary>
		/// Returns a readable message of request errors.
		/// </summary>
		public string Error {
			get { return customError; }
		}


		public NetkoRequest(NetkoItem item, NetkoRequestInfo requestInfo) {
			this.item = item;
			this.requestInfo = requestInfo;
		}

		/// <summary>
		/// Setup request.
		/// </summary>
		public void Setup() {
			// Determine which request to use.
			switch(requestInfo.RequestType) {
			case RequestType.Get:
				unityRequest = UnityWebRequest.Get(requestInfo.Url.GetUriEscaped());
				break;
			case RequestType.Post:
				unityRequest = UnityWebRequest.Post(requestInfo.Url.GetUriEscaped(), requestInfo.Form);
				break;
			case RequestType.Delete:
				unityRequest = UnityWebRequest.Delete(requestInfo.Url.GetUriEscaped());
				break;
			case RequestType.Put:
				unityRequest = UnityWebRequest.Put(requestInfo.Url.GetUriEscaped(), requestInfo.PutData);
				break;
			case RequestType.Head:
				unityRequest = UnityWebRequest.Head(requestInfo.Url.GetUriEscaped());
				break;
			case RequestType.Audio:
				unityRequest = UnityWebRequest.GetAudioClip(requestInfo.Url.GetUriEscaped(), requestInfo.AudioType);
				break;
			case RequestType.AudioStream:
				audioRequest = new AudioStreamRequest(requestInfo.Url.GetUriEscaped(true));
				break;
			case RequestType.Texture:
				unityRequest = UnityWebRequest.GetTexture(
					requestInfo.Url.GetUriEscaped(), requestInfo.TextureNonReadable
				);
				break;
			case RequestType.AssetBundle:
				unityRequest = UnityWebRequest.GetAssetBundle(
					requestInfo.Url.GetUriEscaped(), requestInfo.AssetBundleVersion, RequestInfo.AssetBundleCRC
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
			if(unityRequest != null)
				unityRequest.Send();
			else if(audioRequest != null)
				audioRequest.Send();
			isProcessing = true;

			//Start timeout detection
			TimeOutTime += Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Terminates requests.
		/// </summary>
		public void Terminate() {
			if(unityRequest != null)
				unityRequest.Abort();
			else if(audioRequest != null)
				audioRequest.Dispose();
			unityRequest = null;
			audioRequest = null;
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

