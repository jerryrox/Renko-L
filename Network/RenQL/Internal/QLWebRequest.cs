using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Renko.Network.Internal
{
	/// <summary>
	/// Web request wrapper object for QLRequest.
	/// </summary>
	public class QLWebRequest : CustomYieldInstruction, IDisposable {
		
		private UnityWebRequest webRequest;
		private AsyncOperation operation;


		/// <summary>
		/// Returns the unity's web request instance.
		/// </summary>
		public UnityWebRequest WebRequest {
			get { return webRequest; }
		}

		/// <summary>
		/// Returns whether this object should pause current Coroutine context.
		/// </summary>
		public override bool keepWaiting {
			get { return operation != null && !operation.isDone; }
		}

		/// <summary>
		/// Sends a new web request.
		/// </summary>
		public void Send(QLData requestData)
		{
			// Dispose before initializing a new request.
			Dispose();

			// Create a new post request
			webRequest = UnityWebRequest.Post(requestData.Url, UnityWebRequest.kHttpVerbPOST);

			// Setup upload handler
			webRequest.uploadHandler = new UploadHandlerRaw(
				Encoding.UTF8.GetBytes(requestData.ToString())
			);

			// Set header
			webRequest.SetRequestHeader("Content-Type", "application/json");

			// Send request
			operation = webRequest.Send();
		}

		/// <summary>
		/// Stops current request and resets this object's state.
		/// </summary>
		public void Stop()
		{
			Dispose();
		}

		public void Dispose()
		{
			if(webRequest != null) {
				webRequest.Abort();
				webRequest.Dispose();
			}

			webRequest = null;
			operation = null;
		}
	}
}

