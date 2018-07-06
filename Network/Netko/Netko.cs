using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Renko.Network
{
	/// <summary>
	/// A module that manages any web requests made by this class.
	/// </summary>
	public partial class Netko : MonoBehaviour {
		
		/// <summary>
		/// Reference to Netko instance. Shouldn't be visible
		/// </summary>
		private static Netko I;

		/// <summary>
		/// Object that handles Netko's update process.
		/// </summary>
		private Updater updater;

		/// <summary>
		/// Backing field of NewGroupId property.
		/// </summary>
		private int nextGroupId;


		/// <summary>
		/// Returns the number of requests currently being processed.
		/// </summary>
		public static int CurrentRequestCount {
			get { return I.updater.currentProcessCount; }
		}

		/// <summary>
		/// Max number of requests that can be processed at once.
		/// </summary>
		public static int MaxConcurrentRequests {
			get { return I.updater.maxProcessCount; }
			set { I.updater.maxProcessCount = value; }
		}

		/// <summary>
		/// Returns a new integer value for specifying a group id for Netko Items.
		/// Will increment every call.
		/// </summary>
		public static int NewGroupId {
			get { return ++I.nextGroupId; }
		}


		/// <summary>
		/// Initializes Netko library.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;
			
			I = RenkoLibrary.CreateModule<Netko>(true);
			I.updater = new Updater();
		}

		/// <summary>
		/// Registers the specified item to processing queue and returns it.
		/// </summary>
		public static NetkoItem RegisterItem(NetkoItem item) {
			I.updater.AddItem(item);
			return item;
		}

		/// <summary>
		/// Stops all items with specified id.
		/// </summary>
		public static void StopGroup(int id) {
			I.updater.RemoveGroup(id);
		}

		/// <summary>
		/// Whether Netko updater contains the specified item.
		/// </summary>
		public static bool ContainsItem(NetkoItem item) {
			return I.updater.Items.Contains(item);
		}

		/// <summary>
		/// Returns an enumerator of all items.
		/// </summary>
		public static IEnumerator<NetkoItem> GetItems() {
			return I.updater.Items.GetEnumerator();
		}

		void Update() {
			updater.Update();
		}
	}
}