using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Renko.Network.Internal;

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
			get { return I.updater.CurrentProcessCount; }
		}

		/// <summary>
		/// Max number of requests that can be processed at once.
		/// </summary>
		public static int MaxConcurrentRequests {
			get { return I.updater.MaxProcessCount; }
			set { I.updater.MaxProcessCount = value; }
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
		/// Stops all items with specified id.
		/// </summary>
		public static void StopGroup(int id) {
			I.updater.RemoveGroup(id);
		}

		/// <summary>
		/// Whether Netko updater contains the specified item.
		/// </summary>
		public static bool ContainsItem(INetkoItem item) {
			return I.updater.Items.Contains(item as NetkoItem);
		}

		/// <summary>
		/// Returns an enumerator of all items.
		/// </summary>
		public static IEnumerator<INetkoItem> GetItems() {
			var enumerator = I.updater.Items.GetEnumerator();
			while(enumerator.MoveNext()) {
				yield return enumerator.Current as INetkoItem;
			}
		}

		/// <summary>
		/// Registers the specified item to processing queue and returns it.
		/// </summary>
		public NetkoItem RegisterItem(NetkoItem item) {
			updater.AddItem(item);
			return item;
		}

		void Update() {
			updater.Update();
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