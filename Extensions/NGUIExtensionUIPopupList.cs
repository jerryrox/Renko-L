#if NGUI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Extensions
{
	public static class NGUIExtensionUIPopupList {

		/// <summary>
		/// Returns the index of current selection.
		/// May return -1 if somehow failed.
		/// </summary>
		public static int GetSelectionIndex(this UIPopupList context) {
			string curValue = context.value;
			for(int i=0; i<context.items.Count; i++) {
				if(context.items[i].Equals(curValue))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Sets the selection index.
		/// </summary>
		public static void SetSelectionIndex(this UIPopupList context, int index) {
			context.value = context.items[Mathf.Clamp(index, 0, context.items.Count)];
		}
	}
}
#endif