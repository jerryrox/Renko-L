using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A module that helps with time-controlled events.
	/// </summary>
	public partial class Timer : MonoBehaviour {

		/// <summary>
		/// A cached unscaled delta time value on this frame.
		/// </summary>
		public static float DeltaTime;

		/// <summary>
		/// A cached unscaled time value on this frame.
		/// </summary>
		public static float TimeSinceStartup;

		/// <summary>
		/// Direct instance to this object.
		/// Should be hidden... at least for now.
		/// </summary>
		private static Timer I;

		/// <summary>
		/// Local cached value for Speed/TimeScale property.
		/// </summary>
		private static float speed;

		/// <summary>
		/// Object that will handle this object's Update() process.
		/// </summary>
		private Processor processor;


		/// <summary>
		/// Sets the Time.timeScale and all items' speed to specified value.
		/// </summary>
		public static float TimeScale {
			get { return speed; }
			set {
				if(value < 0)
					value = 0f;
				speed = Time.timeScale = value;
				var items = I.processor.Items;
				for(int i=0; i<items.Count; i++)
					items[i].Speed = value;
			}
		}

		/// <summary>
		/// Sets all items' speed to specified value without changing Time.timeScale.
		/// </summary>
		public static float Speed {
			get { return speed; }
			set {
				if(value < 0)
					value = 0f;
				speed = value;
				var items = I.processor.Items;
				for(int i=0; i<items.Count; i++)
					items[i].Speed = value;
			}
		}


		/// <summary>
		/// Delegate that handles callback event when an item has started updating.
		/// </summary>
		public delegate void ItemStartHandler(Item item);
		/// <summary>
		/// Delegate that handles callback event while an item is updating.
		/// </summary>
		public delegate void ItemUpdateHandler(Item item);
		/// <summary>
		/// Delegate that handles special callback event for item-specifiec events.
		/// </summary>
		public delegate void ItemCustomHandler(Item item);
		/// <summary>
		/// Delegate that handles callback event when an item has finished updating.
		/// </summary>
		public delegate void ItemFinishHandler(Item item);


		void Awake() {
			processor = new Processor();
		}

		/// <summary>
		/// Initializes a new instance of Timer if doesn't already exist.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;
			
			I = RenkoLibrary.CreateModule<Timer>();
			speed = Time.timeScale;
		}

		/// <summary>
		/// Registers the specified item to processing list.
		/// </summary>
		public static void RegisterItem(Timer.Item item) {
			I.processor.AddItem(item);
		}

		/// <summary>
		/// Stops the specified item from processing list.
		/// </summary>
		public static void StopItem(Timer.Item item) {
			I.processor.RemoveItem(item);
		}

		/// <summary>
		/// Removes the item with specified id.
		/// </summary>
		public static void StopByItemId(int itemId) {
			I.processor.RemoveByItemId(itemId);
		}

		/// <summary>
		/// Removes the items with specified group id.
		/// </summary>
		public static void StopByGroupId(int groupId) {
			I.processor.RemoveByGroupId(groupId);
		}

		/// <summary>
		/// Caches current deltatime and realtimesincestartup values for this frame.
		/// </summary>
		private static void CacheTime() {
			DeltaTime = Time.unscaledDeltaTime;
			TimeSinceStartup = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Update process will be handled by processor class.
		/// </summary>
		void Update() {
			CacheTime();
			processor.Update();
		}
	}
}