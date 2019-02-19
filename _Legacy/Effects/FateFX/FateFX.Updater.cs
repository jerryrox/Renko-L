using System;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Legacy.Effects
{
	public partial class FateFX {

		/// <summary>
		/// An encapsulated class for handling the update processes.
		/// </summary>
		private class Updater {

			/// <summary>
			/// List of fate items to process.
			/// </summary>
			private List<FateItem> items;


			public Updater(int listCapacity) {
				items = new List<FateItem>(listCapacity);
			}

			/// <summary>
			/// Adds the specified item to process.
			/// </summary>
			public void AddItem(FateItem item) {
				if(items.Contains(item))
					return;
				
				items.Add(item);
			}

			/// <summary>
			/// Removes the specified item from process.
			/// </summary>
			public void RemoveItem(FateItem item) {
				items.Remove(item);
			}

			/// <summary>
			/// Processes update.
			/// </summary>
			public void Update() {
				for(int i=items.Count-1; i>=0; i--) {
					items[i].Update();
				}
			}
		}
	}
}

