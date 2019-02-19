using System;
using UnityEngine;

namespace Renko.Legacy.Utility
{
	public partial class Timer : MonoBehaviour {
		
		private class UpdateItem : Item {


			public UpdateItem(float duration, ItemUpdateHandler updateHandler) : base() {
				Duration = duration;
				OnItemUpdate = updateHandler;
			}

			/// <summary>
			/// Update this instance for current frame.
			/// </summary>
			public override void Update () {
				if(IsPaused || IsStopped)
					return;
				base.Update ();
				if(currentTime >= startedTime + Duration)
					Stop();
			}
		}
	}
}

