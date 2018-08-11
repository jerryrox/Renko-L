using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Data;

namespace Renko.Network.Internal
{
	/// <summary>
	/// Request object of a GraphQL request.
	/// </summary>
	public class QLRequest : CustomYieldInstruction, IDisposable, IQLRequest {
		
		private QLWebRequest webRequest;
		private RenQL.ResponseHandler callback;
		private RenQL renQL;

		private QLData requestData;
		private bool isDisposed;


		/// <summary>
		/// Returns a yield instruction instance for Coroutine yields.
		/// </summary>
		public CustomYieldInstruction Yield {
			get { return this; }
		}

		/// <summary>
		/// Returns whether the request is on-going.
		/// </summary>
		public bool IsRequesting {
			get { return keepWaiting; }
		}

		/// <summary>
		/// The GraphQL request data object.
		/// </summary>
		public QLData Data {
			get { return requestData; }
			set { requestData = value; }
		}

		/// <summary>
		/// Returns a new response object from last request.
		/// * Will always create a new IQLResponse.
		/// </summary>
		public IQLResponse Response {
			get { return new QLResponse(webRequest); }
		}

		/// <summary>
		/// Returns whether this object should pause current Coroutine context.
		/// </summary>
		public override bool keepWaiting {
			get { return webRequest != null && webRequest.keepWaiting; }
		}


		public QLRequest(RenQL renQL, string url, string query, object variables)
		{
			this.renQL = renQL;

			requestData = new QLData(url, query, variables);
			webRequest = new QLWebRequest();
		}

		~QLRequest()
		{
			Dispose(false);
		}

		/// <summary>
		/// Sends the request with no callback.
		/// </summary>
		public void Send()
		{
			if(isDisposed)
				throw new ObjectDisposedException("QLRequest");
			
			Send(null);
		}

		/// <summary>
		/// Sends the request with specified callback.
		/// </summary>
		public void Send(RenQL.ResponseHandler callback)
		{
			if(isDisposed)
				throw new ObjectDisposedException("QLRequest");
			if(IsRequesting)
				return;

			// Set callback and register to update queue only if there is a valid callback.
			this.callback = callback;
			if(callback != null)
				renQL.RegisterUpdate(this);
			
			webRequest.Send(requestData);
		}

		/// <summary>
		/// Stops the request.
		/// </summary>
		public void Stop()
		{
			if(isDisposed)
				throw new ObjectDisposedException("QLRequest");
			if(!IsRequesting)
				return;
			
			renQL.UnregisterUpdate(this);
			webRequest.Stop();
		}

		/// <summary>
		/// Disposes the request instance.
		/// The object becomes unusable after calling this method.
		/// </summary>
		public void Dispose()
		{
			if(isDisposed)
				throw new ObjectDisposedException("QLRequest");
			
			Stop();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Handles update on this frame to check if request is finished.
		/// </summary>
		public void Update()
		{
			if(!keepWaiting)
				OnRequestEnded();
		}

		/// <summary>
		/// Event called on request finish.
		/// </summary>
		void OnRequestEnded()
		{
			// Invoke callback
			if(callback != null)
				callback(this, Response);

			// Dequeue from update.
			renQL.UnregisterUpdate(this);
		}

		void Dispose(bool isDisposing)
		{
			if(isDisposing && !isDisposed) {
				isDisposed = true;

				webRequest.Dispose();
				webRequest = null;
			}
		}
	}
}

