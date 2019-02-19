using System;
using UnityEngine;

namespace Renko.Legacy.Utility
{
	public partial class Timer : MonoBehaviour {
		
		/// <summary>
		/// A base class for any Timer Item.
		/// </summary>
		public class Item {

			/// <summary>
			/// An incremental field for item id management.
			/// </summary>
			private static int NextId = 1;

			/// <summary>
			/// The time value that represents update start time.
			/// </summary>
			protected float startedTime;

			/// <summary>
			/// The current time value.
			/// </summary>
			protected float currentTime;


			/// <summary>
			/// The unique identifier for this item.
			/// </summary>
			public int Id {
				get; private set;
			}

			/// <summary>
			/// The group number of this item.
			/// </summary>
			public int GroupId {
				get; set;
			}

			/// <summary>
			/// The amount of timeScale to apply on this item.
			/// </summary>
			public float Speed {
				get; set;
			}

			/// <summary>
			/// Gets the number of repeated times.
			/// Sets the max number of repeats to make.
			/// </summary>
			public virtual int RepeatCount {
				get; set;
			}

			/// <summary>
			/// The duration of this item.
			/// </summary>
			public float Duration {
				get; set;
			}

			/// <summary>
			/// A value of current time (0~1) between update start and end times.
			/// </summary>
			public virtual float Progress {
				get { return Mathf.Lerp(startedTime, startedTime + Duration, currentTime); }
				set { currentTime = Mathf.Lerp(startedTime, startedTime + Duration, value); }
			}

			/// <summary>
			/// The amount of time passed since update start time.
			/// </summary>
			public virtual float TimePassed {
				get { return currentTime - startedTime; }
				set {
					currentTime = Mathf.Clamp(
						value + startedTime,
						startedTime,
						startedTime + Duration
					);
				}
			}

			/// <summary>
			/// The amount of time left before current repeat finishes.
			/// </summary>
			public virtual float TimeLeft {
				get { return startedTime + Duration - currentTime; }
				set {
					currentTime = Mathf.Clamp(
						startedTime + Duration - value,
						startedTime,
						startedTime + Duration
					);
				}
			}

			/// <summary>
			/// Whether this item has paused its update.
			/// </summary>
			public bool IsPaused {
				get; set;
			}

			/// <summary>
			/// Whether this item is updating.
			/// </summary>
			public bool IsUpdating {
				get; protected set;
			}

			/// <summary>
			/// Whether this item has stopped processing and should be disposed in the next update.
			/// </summary>
			public bool IsStopped {
				get; protected set;
    		}

			/// <summary>
			/// Event to fire when an item has started updating.
			/// </summary>
			public ItemStartHandler OnItemStart {
				protected get; set;
			}

			/// <summary>
			/// Event to fire while an item is updating.
			/// </summary>
			public ItemUpdateHandler OnItemUpdate {
				protected get; set;
			}

			/// <summary>
			/// A custom handler for unique item-specific events.
			/// For example: Repeat event for DelayItem
			/// </summary>
			public ItemCustomHandler OnItemCustom {
				protected get; set;
			}

			/// <summary>
			/// Event to fire when an item has finished.
			/// </summary>
			public ItemFinishHandler OnItemFinish {
				protected get; set;
			}


			public Item() {
				Id = NextId++;
				GroupId = 0;
				Speed = Timer.speed;
				IsPaused = false;
				IsUpdating = false;
				IsStopped = false;
			}

			/// <summary>
			/// Starts updating.
			/// </summary>
			public virtual void Start() {
				if(IsUpdating)
					return;
				currentTime = startedTime = Timer.TimeSinceStartup;
				Reset();
				if(OnItemStart != null)
					OnItemStart(this);
			}

			/// <summary>
			/// Update this instance for current frame.
			/// </summary>
			public virtual void Update() {
				currentTime += DeltaTime * Speed;
				if(OnItemUpdate != null)
					OnItemUpdate(this);
			}

			/// <summary>
			/// Stops updating.
			/// </summary>
			public virtual void Stop() {
				IsStopped = true;
				if(OnItemFinish != null)
					OnItemFinish(this);
			}

			/// <summary>
			/// Resets this item for update.
			/// </summary>
			protected virtual void Reset() {
				IsPaused = false;
				IsUpdating = true;
				IsStopped = false;
			}
		}
	}
}

