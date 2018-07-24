using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Diagnostics;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A class that represents a single request in Netko.
	/// </summary>
	public class NetkoItem : INetkoItem {
		
		/// <summary>
		/// An instance that holds Netko events.
		/// </summary>
		public NetkoEvent EventInfo;

		/// <summary>
		/// Contains methods and properties for making a request.
		/// You may access any public members except methods.
		/// </summary>
		public NetkoRequest RequestInfo;

		/// <summary>
		/// Contains properties for retrieving response data.
		/// You may access any public members except methods.
		/// </summary>
		public NetkoResponse ResponseInfo;



		/// <summary>
		/// Returns the event handler of this item.
		/// </summary>
		public INetkoEvent Events {
			get { return EventInfo; }
		}

		/// <summary>
		/// Returns the request info of this item.
		/// </summary>
		public INetkoRequest Request {
			get { return RequestInfo; }
		}

		/// <summary>
		/// Returns the response info of this item.
		/// </summary>
		public INetkoResponse Response {
			get { return ResponseInfo; }
		}

		/// <summary>
		/// An option data associated with this item.
		/// You can assign any value here to store value and use it later when OnFinished is called.
		/// </summary>
		public object ExtraData {
			get; set;
		}

		/// <summary>
		/// The group id which this item is associated with.
		/// </summary>
		public int GroupId {
			get; set;
		}



		/// <summary>
		/// Creates a new instance of NetkoItem.
		/// Highly recommended to use Netko's factory methods instead.
		/// </summary>
		public NetkoItem(Netko netko, int groupId, NetkoRequestInfo requestInfo) {
			GroupId = groupId;
			EventInfo = new NetkoEvent(this);
			RequestInfo = new NetkoRequest(requestInfo);
			ResponseInfo = new NetkoResponse(this);

			Initialize();

			// Register the item to Netko updater.
			netko.RegisterItem(this);
		}

		/// <summary>
		/// Resets this item's state for retry.
		/// </summary>
		public void Retry() {
			Initialize();
		}

		/// <summary>
		/// Forcefully stops request if on-going.
		/// </summary>
		public void ForceStop() {
			RequestInfo.SetError("The request was stopped by user.");
		}

		/// <summary>
		/// Sends the request to server.
		/// If you call this directly, this item will make a request immediately
		/// without waiting in the updater queue but it's not recommended.
		/// </summary>
		public void Send() {
			DispatchEvent(0);
			RequestInfo.Send();
		}

		/// <summary>
		/// Handles update.
		/// </summary>
		public void Update() {
			DispatchEvent(1);
			RequestInfo.CheckError();
		}

		/// <summary>
		/// Terminates this item.
		/// Once an item is terminated, you won't be able to access any useful data from the item.
		/// </summary>
		public void Terminate() {
			DispatchEvent(3);
			RequestInfo.Terminate();
		}

		/// <summary>
		/// Fires event based on the specified type.
		/// Don't call this directly unless you know what's going on.
		/// </summary>
		public void DispatchEvent(int type) {
			switch(type) {
			case 0: EventInfo.InvokeOnRequested(); break;
			case 1: EventInfo.InvokeOnProcessing(RequestInfo.Progress); break;
			case 2: EventInfo.InvokeOnFinished(); break;
			case 3: EventInfo.InvokeOnTerminated(); break;
			}
		}

		/// <summary>
		/// Sets this item to its initial state.
		/// </summary>
		void Initialize() {
			RequestInfo.Setup();
		}
	}
}