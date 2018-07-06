using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Diagnostics;
using Renko.Network.Internal;

namespace Renko.Network
{
	/// <summary>
	/// A class that represents a single request in Netko.
	/// </summary>
	public class NetkoItem {
		
		/// <summary>
		/// An instance that holds Netko events.
		/// </summary>
		public NetkoEvent Events;

		/// <summary>
		/// Contains methods and properties for making a request.
		/// You may access any public members except methods.
		/// </summary>
		public NetkoRequest Request;

		/// <summary>
		/// Contains properties for retrieving response data.
		/// You may access any public members except methods.
		/// </summary>
		public NetkoResponse Response;

		/// <summary>
		/// An optional data associated with this item.
		/// You can assign any value here to store value and use it later when OnFinished is fired, for example.
		/// </summary>
		public object ExtraData;

		/// <summary>
		/// The group id which this item is associated with.
		/// </summary>
		public int GroupId;


		/// <summary>
		/// Creates a new instance of NetkoItem.
		/// Highly recommended to use Netko's factory methods instead.
		/// </summary>
		public NetkoItem(int groupId, NetkoRequestInfo requestInfo) {
			GroupId = groupId;
			Events = new NetkoEvent(this);
			Request = new NetkoRequest(this, requestInfo);
			Response = new NetkoResponse(this);

			Initialize();

			// Register the item to Netko updater.
			Netko.RegisterItem(this);
		}

		/// <summary>
		/// Resets this item's state for retry.
		/// </summary>
		public void Retry() {
			Initialize();
		}

		/// <summary>
		/// Sends the request to server.
		/// If you call this directly, this item will make a request immediately
		/// without waiting in the updater queue but it's not recommended.
		/// </summary>
		public void Send() {
			DispatchEvent(0);
			Request.Send();
		}

		/// <summary>
		/// Stops an on-going request.
		/// </summary>
		public void Stop() {
			Request.SetError("The request was stopped by user.");
		}

		/// <summary>
		/// Handles update.
		/// </summary>
		public void Update() {
			DispatchEvent(1);
			Request.CheckError();
		}

		/// <summary>
		/// Terminates this item.
		/// Once an item is terminated, you won't be able to access any useful data from the item.
		/// </summary>
		public void Terminate() {
			DispatchEvent(3);
			Request.Terminate();
		}

		/// <summary>
		/// Fires event based on the specified type.
		/// Don't call this directly unless you know what's going on.
		/// </summary>
		public void DispatchEvent(int type) {
			switch(type) {
			case 0: Events.InvokeOnRequested(); break;
			case 1: Events.InvokeOnProcessing(Request.Progress); break;
			case 2: Events.InvokeOnFinished(); break;
			case 3: Events.InvokeOnTerminated(); break;
			}
		}

		/// <summary>
		/// Sets this item to its initial state.
		/// </summary>
		void Initialize() {
			Request.Setup();
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