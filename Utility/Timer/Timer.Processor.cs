using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Utility
{
	public partial class Timer : MonoBehaviour {

		/// <summary>
		/// An encapsulated class that handles the Update() process of Timer class.
		/// </summary>
		private class Processor {

			/// <summary>
			/// List of items being processed.
			/// </summary>
			private List<Timer.Item> items;


			/// <summary>
			/// Returns the list of items being processed
			/// </summary>
			public List<Timer.Item> Items {
				get { return items; }
			}


			public Processor() {
				items = new List<Item>();
			}

			/// <summary>
			/// Adds the specified item to the process list.
			/// </summary>
			public void AddItem(Timer.Item item) {
				items.Add(item);
			}

			/// <summary>
			/// Stops the specified item for removal.
			/// </summary>
			public void RemoveItem(Timer.Item item) {
				item.Stop();
			}

			/// <summary>
			/// Stops the item with specified id for removal.
			/// </summary>
			public void RemoveByItemId(int id) {
				for(int i=0; i<items.Count; i++) {
					if(items[i].Id == id) {
						items[i].Stop();
						break;
					}
				}
			}

			/// <summary>
			/// Stops the items with specified group id for removal.
			/// </summary>
			public void RemoveByGroupId(int id) {
				for(int i=0; i<items.Count; i++) {
					if(items[i].GroupId == id) {
						items[i].Stop();
					}
				}
			}

			public void Update() {
				for(int i=items.Count-1; i>=0; i--) {
					Timer.Item item = items[i];

					if(!item.IsUpdating)
						item.Start();
					item.Update();
					if(item.IsStopped)
						items.RemoveAt(i--);
				}
			}
		}
	}
}

