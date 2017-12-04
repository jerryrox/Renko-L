using System;
using UnityEngine;

namespace Renko.Utility
{
	public partial class Timer : MonoBehaviour {
		
		private class UpdateItem : Item {

			/// <summary>
			/// The current time value.
			/// </summary>
			private float currentTime;


			/// <summary>
			/// Lerp value of current time between update start and end times.
			/// </summary>
			public override float Progress {
				get { return Mathf.Lerp(startedTime, startedTime + Duration, currentTime); }
				set { currentTime = Mathf.Lerp(startedTime, startedTime + Duration, value); }
			}

			/// <summary>
			/// Returns the amount of time passed since update start time.
			/// </summary>
			public override float TimePassed {
				get { return currentTime - startedTime; }
			}


			public UpdateItem(float duration) : base() {
				Duration = duration;
			}

			/// <summary>
			/// Starts updating.
			/// </summary>
			public override void Start () {
				base.Start ();
				currentTime = startedTime;
			}

			/// <summary>
			/// Update this instance for current frame.
			/// </summary>
			public override void Update () {
				if(IsPaused || IsStopped)
					return;

				currentTime += DeltaTime * Speed;
				base.Update ();
				if(currentTime >= startedTime + Duration)
					Stop();
			}
		}
	}
}

