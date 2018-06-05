using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Diagnostics;
using Renko.Network.Internal;
using Renko.Data;

namespace Renko.Network
{
	public partial class Netko : MonoBehaviour {

		public class Item : IDisposable {

			/// <summary>
			/// Web request object for almost all request types.
			/// </summary>
			private UnityWebRequest unityRequest;
			/// <summary>
			/// Web request object for audio streams only.
			/// </summary>
			private AudioStreamRequest audioRequest;
			/// <summary>
			/// Whether this item is processing or not.
			/// </summary>
			private bool isProcessing;
			/// <summary>
			/// A string value that represents a custom error message.
			/// </summary>
			private string customError;


			#region Properties
			/// <summary>
			/// Group id of this item.
			/// </summary>
			public int GroupId {
				get; set;
			}
			/// <summary>
			/// Returns the response code from requested server.
			/// If called on AudioStream request, it may return 0.
			/// </summary>
			public long ResponseCode {
				get {
					if(unityRequest != null)
						return unityRequest.responseCode;
					if(audioRequest != null)
						return audioRequest.ResponseHeaders.GetResponseCode();
					return 0;
				}
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
			/// Time which makes this item throw a timeout error.
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
			/// May be "finished" earlier due to request time-out.
			/// </summary>
			public bool IsFinished {
				get {
					if(IsTimeOut)
						return true;
					if(unityRequest != null)
						return unityRequest.isDone;
					if(audioRequest != null)
						return audioRequest.IsDone;
					return false;
				}
			}
			/// <summary>
			/// Returns whether the request is successful.
			/// </summary>
			public bool IsSuccess {
				get {
					if(IsTimeOut)
						return false;
					if(!string.IsNullOrEmpty(customError))
						return false;
					if(unityRequest != null)
						return !unityRequest.isError;
					if(audioRequest != null)
						return audioRequest.IsError;
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
			/// Whether this item should be terminated after the request has finished.
			/// If this is set to false, you must manually call Terminate method to prevent things from breaking.
			/// Furthermore, you must re-register this item to Netko process queue if you're retrying this item after current frame.
			/// </summary>
			public bool AutoTerminate {
				get; set;
			}
			/// <summary>
			/// Returns the request method currently being used.
			/// </summary>
			public string Method {
				get {
					return RequestInfo.Method;
				}
			}
			/// <summary>
			/// Returns a readable error message.
			/// </summary>
			public string ErrorMessage {
				get {
					if(!string.IsNullOrEmpty(customError))
						return customError;
					if(unityRequest != null)
						return unityRequest.error;
					if(audioRequest != null)
						return audioRequest.ErrorMessage;
					return null;
				}
			}
			/// <summary>
			/// Returns the retried text data.
			/// </summary>
			public string TextData {
				get {
					if(unityRequest != null && unityRequest.downloadHandler != null)
						return unityRequest.downloadHandler.text;
					if(audioRequest != null)
						return audioRequest.TextData;
					return null;
				}
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
			/// Returns the retrieved byte data.
			/// </summary>
			public byte[] ByteData {
				get {
					if(unityRequest != null && unityRequest.downloadHandler != null)
						return unityRequest.downloadHandler.data;
					if(audioRequest != null)
						return audioRequest.ByteData;
					return null;
				}
			}
			/// <summary>
			/// Returns a JsonData object from retrieved text data.
			/// </summary>
			public JsonData JsonData {
				get {
					return Json.Parse(TextData);
				}
			}
			/// <summary>
			/// Returns the response headers.
			/// </summary>
			public Dictionary<string,string> ResponseHeaders {
				get {
					if(unityRequest != null)
						return unityRequest.GetResponseHeaders();
					if(audioRequest != null)
						return audioRequest.ResponseHeaders;
					return null;
				}
			}
			/// <summary>
			/// Returns the retrieved audio data.
			/// </summary>
			public AudioClip AudioData {
				get {
					if(unityRequest != null)
						return DownloadHandlerAudioClip.GetContent(unityRequest);
					if(audioRequest != null)
						return audioRequest.AudioData;
					return null;
				}
			}
			/// <summary>
			/// Returns the retrieved asset bundle data.
			/// </summary>
			public AssetBundle AssetBundleData {
				get {
					if(unityRequest != null)
						return DownloadHandlerAssetBundle.GetContent(unityRequest);
					return null;
				}
			}
			/// <summary>
			/// Returns the retrieved texture data.
			/// </summary>
			public Texture2D TextureData {
				get {
					if(unityRequest != null)
						return DownloadHandlerTexture.GetContent(unityRequest);
					return null;
				}
			}
			/// <summary>
			/// Source of data to use for request.
			/// </summary>
			public NetkoRequestInfo RequestInfo {
				get; set;
			}
			/// <summary>
			/// An additional (optional) data associated with this item.
			/// </summary>
			public object ExtraData {
				get; set;
			}
			#endregion


			/// <summary>
			/// Event to invoke when the request is made.
			/// </summary>
			public event Action<Item> OnRequested;
			/// <summary>
			/// Event to invoke while request is being processed.
			/// </summary>
			public event Action<Item,float> OnProcessing;
			/// <summary>
			/// Event to invoke when the request is finished.
			/// </summary>
			public event Action<Item> OnFinished;
			/// <summary>
			/// Event to invoke when this item is terminated.
			/// </summary>
			public event Action<Item> OnTerminated;


			private Item(int groupId, NetkoRequestInfo _requestInfo) {
				GroupId = groupId;
				RequestInfo = _requestInfo;
				AutoTerminate = true;

				Initialize();
			}

			/// <summary>
			/// Creates a new Netko Item with specified params.
			/// </summary>
			public static Item Create(int groupId, NetkoRequestInfo requestInfo) {
				return RegisterItem(new Item(groupId, requestInfo));
			}

			/// <summary>
			/// Sets this item to its initial state.
			/// </summary>
			public void Initialize() {
				SetupRequest();
				SetupVariables();
			}

			/// <summary>
			/// Resets this item's state for retry.
			/// </summary>
			public void Retry() {
				Initialize();
			}

			/// <summary>
			/// Sends the request to server.
			/// Don't call this directly unless you know what's going on.
			/// </summary>
			public void Send() {
				DispatchEvent(0);
				if(unityRequest != null)
					unityRequest.Send();
				else if(audioRequest != null)
					audioRequest.Send();
				isProcessing = true;

				//Start timeout detection
				TimeOutTime += Time.realtimeSinceStartup;
			}

			/// <summary>
			/// Terminates this item.
			/// Once an item is terminated, you won't be able to access any useful data from the item.
			/// </summary>
			public void Terminate() {
				DispatchEvent(3);
				if(unityRequest != null)
					unityRequest.Abort();
				else if(audioRequest != null)
					audioRequest.Dispose();
				unityRequest = null;
				audioRequest = null;
			}

			/// <summary>
			/// Fires event based on the specified type.
			/// Don't call this directly unless you know what's going on.
			/// </summary>
			public void DispatchEvent(int type) {
				switch(type) {
				case 0: if(OnRequested != null) OnRequested(this); break;
				case 1: if(OnProcessing != null) OnProcessing(this, DownloadProgress > UploadProgress ? DownloadProgress : UploadProgress); break;
				case 2: if(OnFinished != null) OnFinished(this); break;
				case 3: if(OnTerminated != null) OnTerminated(this); break;
				}
			}

			/// <summary>
			/// Makes the request fail due to specified error.
			/// </summary>
			public void SetError(string error) {
				customError = error;
			}

			/// <summary>
			/// Releases all resource used by the <see cref="Renko.Network.Netko+Item"/> object.
			/// </summary>
			public void Dispose() {
				if(unityRequest != null)
					unityRequest.Dispose();
				else if(audioRequest != null)
					audioRequest.Dispose();
			}

			/// <summary>
			/// Initializes request objects.
			/// </summary>
			void SetupRequest() {
				switch(RequestInfo.RequestType) {
				case RequestType.Get:
					unityRequest = UnityWebRequest.Get(Uri.EscapeUriString(RequestInfo.Url));
					break;
				case RequestType.Post:
					unityRequest = UnityWebRequest.Post(Uri.EscapeUriString(RequestInfo.Url), RequestInfo.Form);
					break;
				case RequestType.Delete:
					unityRequest = UnityWebRequest.Delete(Uri.EscapeUriString(RequestInfo.Url));
					break;
				case RequestType.Put:
					unityRequest = UnityWebRequest.Put(Uri.EscapeUriString(RequestInfo.Url), RequestInfo.PutData);
					break;
				case RequestType.Head:
					unityRequest = UnityWebRequest.Head(Uri.EscapeUriString(RequestInfo.Url));
					break;
				case RequestType.Audio:
					unityRequest = UnityWebRequest.GetAudioClip(Uri.EscapeUriString(RequestInfo.Url), RequestInfo.AudioType);
					break;
				case RequestType.AudioStream:
					audioRequest = new AudioStreamRequest(GetEscapedUriString(RequestInfo.Url));
					break;
				case RequestType.AssetBundle:
					unityRequest = UnityWebRequest.GetAssetBundle(Uri.EscapeUriString(RequestInfo.Url), RequestInfo.AssetBundleVersion, RequestInfo.AssetBundleCRC);
					break;
				case RequestType.Texture:
					unityRequest = UnityWebRequest.GetTexture(Uri.EscapeUriString(RequestInfo.Url), RequestInfo.TextureNonReadable);
					break;
				}
			}

			/// <summary>
			/// Initializes variables.
			/// </summary>
			void SetupVariables() {
				SetError(null);
				isProcessing = false;
				TimeOutTime = RequestInfo.TimeOut;
			}

			/// <summary>
			/// A helper method that returns the specified uri after percent-escaping.
			/// </summary>
			string GetEscapedUriString(string uri) {
				string decoded = Uri.UnescapeDataString(uri);
				if(decoded.Equals(uri))
					return Uri.EscapeUriString(uri);
				return uri;
			}
		}


		/// <summary>
		/// The type of request for Netko Items.
		/// </summary>
		public enum RequestType {
			Get,
			Post,
			Delete,
			Put,
			Head,
			Audio,
			AudioStream,
			AssetBundle,
			Texture
		}
	}
}