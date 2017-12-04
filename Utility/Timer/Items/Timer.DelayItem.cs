using System;
using UnityEngine;

namespace Renko.Utility
{
	public partial class Timer : MonoBehaviour {

		private class DelayItem : Item {

			/// <summary>
			/// Current number of times repeated.
			/// </summary>
			private int repeatedCount;

			/// <summary>
			/// Target number of times to repeat
			/// </summary>
			private int targetRepeats;


			/// <summary>
			/// Gets the number of repeated times.
			/// Sets the max number of repeats to make.
			/// </summary>
			public override int RepeatCount {
				get { return repeatedCount; }
				set { targetRepeats = value; }
			}


			public DelayItem(float duration, int repeat, ItemFinishHandler finishHandler) : base() {
				repeatedCount = 0;
				targetRepeats = 0;
				Duration = duration;
				RepeatCount = repeat;
				OnItemFinish = finishHandler;
			}

			/// <summary>
			/// Update this instance for current frame.
			/// </summary>
			public override void Update () {
				if(IsPaused || IsStopped)
					return;
				base.Update ();
				if(currentTime >= startedTime + Duration) {
					if(!Repeat())
						Stop();
				}
			}

			/// <summary>
			/// Returns whether this item should be repeated.
			/// </summary>
			bool Repeat() {
				repeatedCount ++;
				if(repeatedCount > targetRepeats)
					return false;

				startedTime += Duration;
				if(OnItemRepeat != null)
					OnItemRepeat(this, repeatedCount);
				return true;
			}
		}
	}
}

