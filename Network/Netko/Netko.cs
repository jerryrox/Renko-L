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
		private static Netko Instance;
		/// <summary>
		/// Object that handles Netko's update process.
		/// </summary>
		private ProcessQueue processQueue;


		/// <summary>
		/// Number of requests currently being processed.
		/// </summary>
		public static int CurrentRequestCount {
			get { return Instance.processQueue.currentProcessCount; }
		}
		/// <summary>
		/// Max number of requests that can be processed at once.
		/// </summary>
		public static int MaxConcurrentRequests {
			get { return Instance.processQueue.maxProcessCount; }
			set { Instance.processQueue.maxProcessCount = value; }
		}


		/// <summary>
		/// Initializes Netko library.
		/// </summary>
		public static void Initialize() {
			if(Instance != null)
				return;
			Instance = GameObject.FindObjectOfType<Netko>();
			if(Instance == null)
				Instance = new GameObject("_Netko").AddComponent<Netko>();
			Instance.processQueue = new ProcessQueue();
			DontDestroyOnLoad(Instance.gameObject);
		}

		/// <summary>
		/// Registers the specified item to processing queue and returns it.
		/// </summary>
		public static Item RegisterItem(Item item) {
			Instance.processQueue.AddItem(item);
			return item;
		}

		/// <summary>
		/// Terminates all items with specified id.
		/// </summary>
		public static void TerminateGroup(int id) {
			Instance.processQueue.RemoveGroup(id);
		}

		void Update() {
			processQueue.Process();
		}
	}
}