using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Renko.Extensions;
using Renko.Debug;

namespace Renko.Threading
{
	/// <summary>
	/// A simple class for executing dispatched actions in Unity's main thread.
	/// </summary>
	public class UnityThread : MonoBehaviour {

		private static UnityThread I;

		private Queue<Item> items;
		private object locker;


		/// <summary>
		/// The identifier of Unity's main thread.
		/// Used for thread equality check.
		/// </summary>
		public static int MainThreadId {
			get; private set;
		}


		/// <summary>
		/// Delegate that handles a process to be done in main thread.
		/// </summary>
		public delegate object ProcessHandler();


		void Awake() {
			items = new Queue<Item>();
			locker = new object();
		}

		/// <summary>
		/// Initializes this module.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;
			
			I = RenkoLibrary.CreateModule<UnityThread>();
			MainThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Dispatches the specified handler for main thread execution.
		/// </summary>
		public static object Dispatch(ProcessHandler handler) {
			//Things will get dirty if this method is called from the main thread,
			//as we need to pause the caller's current thread.
			if(Thread.CurrentThread.ManagedThreadId == MainThreadId) {
				return handler();
			}

			Item item = new Item(handler);
			RegisterItem(item);
			item.Standby();
			return item.ReturnedData;
		}

		/// <summary>
		/// Registers the specified item with thread-safety.
		/// </summary>
		private static void RegisterItem(Item item) {
			lock(I.locker) {
				I.items.Enqueue(item);
			}
		}

		void Update() {
			lock(locker) {
				while(true) {
					if(items.Count == 0)
						break;
					items.Dequeue().Invoke();
				}
			}
		}


		/// <summary>
		/// A class that holds information of 
		/// </summary>
		private class Item {

			/// <summary>
			/// The object returned from dispatched event.
			/// </summary>
			public object ReturnedData {
				get; set;
			}

			/// <summary>
			/// Event resetter for pausing / resuming the current thread.
			/// </summary>
			private ManualResetEvent manualEvent;

			/// <summary>
			/// Handler for invoking the process in main thread.
			/// </summary>
			private ProcessHandler processHandler;


			public Item(ProcessHandler handler) {
				manualEvent = new ManualResetEvent(false);
				processHandler = handler;
			}

			/// <summary>
			/// Invokes the stored process.
			/// </summary>
			public void Invoke() {
				ReturnedData = processHandler.Invoke();
				manualEvent.Set();
			}

			/// <summary>
			/// Pauses the current thread.
			/// </summary>
			public void Standby() {
				manualEvent.WaitOne();
			}
		}
	}
}

