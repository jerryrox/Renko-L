using System;
using UnityEngine;

namespace Renko.Utility
{
	public partial class Timer : MonoBehaviour {
		
		/// <summary>
		/// Creates a new item that provides a new Update() functionality.
		/// </summary>
		public static Item CreateUpdate(float duration) {
			UpdateItem item = new UpdateItem(duration);
			RegisterItem(item);
			return item;
		}
	}
}

