using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Network
{
	public partial class Netko : MonoBehaviour {

		private class ProcessQueue {

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
			private List<Netko.Item> items;


			/// <summary>
			/// Returns whether there is a vacancy for a new request.
			/// </summary>
			public bool CanMakeRequest {
				get { return currentProcessCount < maxProcessCount; }
			}


			public ProcessQueue() {
				items = new List<Item>();
			}

			/// <summary>
			/// Removes all items that match the specified group id.
			/// </summary>
			public void RemoveGroup(int id) {
				for(int i=items.Count-1; i>=0; i--) {
					if(items[i].GroupId == id)
						items[i].Terminate();
				}
			}

			/// <summary>
			/// Removes the specified item from queue.
			/// </summary>
			public void RemoveItem(Item item) {
				items.Remove(item);
			}

			/// <summary>
			/// Adds the specified item to the queue.
			/// </summary>
			public void AddItem(Item item) {
				items.Add(item);
			}

			/// <summary>
			/// Processes Netko's update routine.
			/// </summary>
			public void Process() {
				for(int i=0; i<items.Count; i++) {
					Item item = items[i];

					//Termination must be checked first...
					if(item.IsTerminated) {
						items.RemoveAt(i--);
						if(item.IsProcessing)
							currentProcessCount --;
						continue;
					}

					//Making a request
					if(!item.IsProcessing) {
						if(CanMakeRequest) {
							currentProcessCount ++;
							item.Send();
						}
						continue;
					}

					//Finished request
					if(item.IsFinished) {
						currentProcessCount --;
						CheckError(item);
						item.DispatchEvent(2);
						// Even if the item is flagged for auto termination (which is true, by default)
						// We must also take 'retries' into account.
						if(item.AutoTerminate && item.IsProcessing)
							item.Terminate();
						continue;
					}

					//OnProcessing event
					item.DispatchEvent(1);
				}
			}

			/// <summary>
			/// Checks for any explicit errors.
			/// </summary>
			void CheckError(Item item) {
				//Timeout error
				if(item.IsTimeOut) {
					item.SetError("The request has timed out.");
					RenLog.Log(
						LogLevel.Info,
						"Netko.ProcessQueue.Process - Timeout error at url: {1}" + item.Url
					);
				}
			}
		}
	}
}