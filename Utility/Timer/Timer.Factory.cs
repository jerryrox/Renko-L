using System;
using UnityEngine;

namespace Renko.Utility
{
	public partial class Timer : MonoBehaviour {
		
		/// <summary>
		/// Creates a new item that provides a new Update() functionality.
		/// </summary>
		public static Item CreateUpdate(float duration, ItemUpdateHandler updateHandler) {
			UpdateItem item = new UpdateItem(duration, updateHandler);
			RegisterItem(item);
			return item;
		}

		/// <summary>
		/// Creates a new item that provides a delayed callback after specified duration.
		/// </summary>
		public static Item CreateDelay(float duration, int repeat, ItemFinishHandler finishHandler) {
			DelayItem item = new DelayItem(duration, repeat, finishHandler);
			RegisterItem(item);
			return item;
		}
	}
}

