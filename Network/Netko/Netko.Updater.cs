using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Diagnostics;

namespace Renko.Network
{
	public partial class Netko : MonoBehaviour {

		/// <summary>
		/// An encapsulated class that manages Netko update routine.
		/// </summary>
		private class Updater {

			/// <summary>
			/// Number of requests currently being processed.
			/// </summary>
			public int currentProcessCount;

			/// <summary>
			/// Max number of requests that can be processed at once.
			/// </summary>
			public int maxProcessCount = 10;

			/// <summary>
			/// The list of Netko items queued for processing.
			/// </summary>
			public List<NetkoItem> Items;


			/// <summary>
			/// Returns whether there is a vacancy for a new request.
			/// </summary>
			public bool CanMakeRequest {
				get { return currentProcessCount < maxProcessCount; }
			}


			public Updater() {
				Items = new List<NetkoItem>();
			}

			/// <summary>
			/// Removes all items that match the specified group id.
			/// </summary>
			public void RemoveGroup(int id) {
				for(int i=Items.Count-1; i>=0; i--) {
					if(Items[i].GroupId == id)
						Items[i].Stop();
				}
			}

			/// <summary>
			/// Removes the specified item from queue.
			/// </summary>
			public void RemoveItem(NetkoItem item) {
				Items.Remove(item);
			}

			/// <summary>
			/// Adds the specified item to the queue.
			/// </summary>
			public void AddItem(NetkoItem item) {
				Items.Add(item);
			}

			/// <summary>
			/// Processes Netko's update routine.
			/// </summary>
			public void Update() {
				for(int i=0; i<Items.Count; i++) {
					NetkoItem item = Items[i];

					//Termination must be checked first...
					if(RemoveTerminated(item, ref i))
						continue;

					//Making a request
					if(StartRequest(item))
						continue;

					//Finished request
					if(FinishRequest(item))
						continue;

					//OnProcessing event
					item.Update();
				}
			}

			/// <summary>
			/// Removes specified item if terminated.
			/// Returns whether removal was successful.
			/// </summary>
			private bool RemoveTerminated(NetkoItem item, ref int i) {
				if(item.Request.IsTerminated) {
					// If items weren't being processed and just got terminated, we should manually handle the process count management.
					if(!item.Request.IsProcessing)
						currentProcessCount --;
					Items.RemoveAt(i--);
					return true;
				}
				return false;
			}

			/// <summary>
			/// Starts the specified item's request.
			/// Returns whether item has successfully started its request or is waiting in the queue.
			/// </summary>
			private bool StartRequest(NetkoItem item) {
				if(!item.Request.IsProcessing) {
					// Start the request only if there is an empty space in the queue.
					if(CanMakeRequest) {
						currentProcessCount ++;
						item.Send();
					}
					return true;
				}
				return false;
			}

			/// <summary>
			/// Finishes the specified item if there was an explicit error or request is done.
			/// Returns whether finish process was done.
			/// </summary>
			private bool FinishRequest(NetkoItem item) {
				if(item.Request.IsFinished) {
					// Request count management.
					currentProcessCount --;

					// OnFinished callack.
					item.DispatchEvent(2);

					// If item wasn't flagged for retry (IsProcessing == true), we should terminate it.
					if(item.Request.IsProcessing)
						item.Terminate();
					// Flagged retry
					else {
						//If attempted termination after calling retry, this is an invalid action.
						if(item.Request.IsTerminated) {
							RenLog.LogWarning(
								"Netko.Updater.FinishRequest - You should not call item.Terminate() directly after " +
								"retrying! Forcing retry..."
							);
							// Force retry.
							item.Retry();
						}
					}
					return true;
				}
				return false;
			}
		}
	}
}