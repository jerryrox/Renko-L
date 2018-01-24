using System.Collections;
using System.Collections.Generic;
using System;
using Renko.Diagnostics;

namespace Renko.Utility
{
    /// <summary>
    /// A base recycler class that can be extended for custom functionalities.
    /// </summary>
	public class BaseRecycler<T> where T : class {
        
        /// <summary>
        /// List that contains all objects being managed.
        /// </summary>
		protected QueuedList<T> items;

		public CreateHandler OnCreateHandler {
			protected get; set;
		}
		public CheckValidHandler OnCheckValidHandler {
			protected get; set;
		}
		public ResetHandler OnResetHandler {
			protected get; set;
		}
		public DestroyHandler OnDestroyHandler {
			protected get; set;
		}

		public T this[int index] {
			get {
				if(index < 0 || index >= items.Count)
					throw new IndexOutOfRangeException();
				return items[index];
			}
			set {
				if(index < 0 || index >= items.Count)
					throw new IndexOutOfRangeException();
				if(value == null) {
					RenLog.Log(LogLevel.Warning, "BaseRecycler.this - value can't be null!");
					return;
				}
				items[index] = value;
			}
		}

		/// <summary>
		/// Returns the total number of items in this recycler.
		/// </summary>
		public int Count {
			get {
				return items.Count;
			}
		}

		/// <summary>
		/// Returns the number of active (valid) items in this recycler.
		/// </summary>
		public int ValidCount {
			get {
				int valid = 0;
				for(int i=0; i<items.Count; i++)
					valid += (FireOnCheckValid(items[i]) ? 1 : 0);
				return valid;
			}
		}

        /// <summary>
        /// Delegate that handles creation of a new instance.
        /// </summary>
        public delegate T CreateHandler();
		/// <summary>
		/// Delegate that handles validation of the specified instance.
		/// </summary>
		public delegate bool CheckValidHandler(T obj);
        /// <summary>
        /// Delegate that handles resetting of the specified instance.
        /// </summary>
        public delegate void ResetHandler(T obj);
        /// <summary>
        /// Delegate that handles destruction of the specified instance.
        /// </summary>
        public delegate void DestroyHandler(T obj);


		public BaseRecycler() {
			items = new QueuedList<T>();
        }
		public BaseRecycler(CreateHandler createHandler, CheckValidHandler checkValidHandler, ResetHandler resetHandler, DestroyHandler destroyHandler) {
			items = new QueuedList<T>();
			OnCreateHandler = createHandler;
			OnCheckValidHandler = checkValidHandler;
			OnResetHandler = resetHandler;
			OnDestroyHandler = destroyHandler;
		}

		/// <summary>
		/// Adds the specified item.
		/// If null, the OnCreateHandler event will be fired.
		/// Returns the added item.
		/// </summary>
		public virtual T Add(T item = null) {
			if(item == null)
				item = FireOnCreate();
			if(item == null) {
				RenLog.Log(LogLevel.Warning, "BaseRecycler.Add - OnCreateHandler and the specified item is null!");
				return null;
			}
			items.Add(item);
			return item;
		}

		/// <summary>
		/// Returns whether the specified item is valid (active).
		/// </summary>
		public virtual bool IsItemValid(T item) {
			if(item == null) {
				RenLog.Log(LogLevel.Warning, "BaseRecycler.IsItemActive - Parameter 'item' can't be null!");
				return true;
			}
			return FireOnCheckValid(item);
		}

		/// <summary>
		/// Resets the specified item to its re-usable state.
		/// </summary>
		public virtual void Reset(T item) {
			if(item == null) {
				RenLog.Log(LogLevel.Warning, "BaseRecycler.Reset - Parameter 'item' can't be null!");
				return;
			}
			FireOnReset(item);
		}

		/// <summary>
		/// Destroys (disposes) the specified item for removal.
		/// Returns whether it's a success or failure.
		/// </summary>
		public virtual bool Destroy(T item) {
			if(item == null) {
				RenLog.Log(LogLevel.Warning, "BaseRecycler.Destroy - Parameter 'item' can't be null!");
				return false;
			}
			FireOnDestroy(item);
			return true;
		}

		/// <summary>
		/// Returns the next inactive (invalid) item in the list.
		/// </summary>
		public T NextItem() {
			for(int i=0; i<items.Count; i++) {
				T item = items.NextItem();
				if(FireOnCheckValid(item))
					continue;
				return FireOnReset(item);
			}
			RenLog.Log(LogLevel.Info, "BaseRecycler.NextItem - There is no available item. Creating a new item.");
			return FireOnReset(Add());
		}

		/// <summary>
		/// Removes the specified item from management.
		/// </summary>
		public void Remove(T item) {
			if(Destroy(item))
				items.Remove(item);
		}

		/// <summary>
		/// Removes all items from management.
		/// </summary>
		public void RemoveAll() {
			for(int i=0; i<items.Count; i++) {
				FireOnDestroy(items[i]);
			}
			items.Clear();
		}

		/// <summary>
		/// Fires the OnCreate handler and returns new item.
		/// </summary>
		protected virtual T FireOnCreate() {
			if(OnCreateHandler != null)
				return OnCreateHandler();
			return null;
		}

		/// <summary>
		/// Fires the OnCheckValid handler and returns whether it's active (valid).
		/// </summary>
		protected virtual bool FireOnCheckValid(T item) {
			if(OnCheckValidHandler != null)
				return OnCheckValidHandler(item);
			return false;
		}

		/// <summary>
		/// Fires the OnReset handler and returns the specified item.
		/// </summary>
		protected virtual T FireOnReset(T item) {
			if(OnResetHandler != null)
				OnResetHandler(item);
			return item;
		}

		/// <summary>
		/// Fires the OnDestroy handler.
		/// </summary>
		protected virtual void FireOnDestroy(T item) {
			if(OnDestroyHandler != null)
				OnDestroyHandler(item);
		}
    }
}