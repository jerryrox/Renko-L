using System.Collections;
using System.Collections.Generic;
using System;

namespace Renko.Utility
{
	public class QueuedList<T> : IEnumerable<T>, IEnumerable, ICollection {

        /// <summary>
        /// List of items being managed internally.
        /// </summary>
        private List<T> items;

        /// <summary>
        /// Index of item to return from the list when next item was requested.
        /// </summary>
        private int nextItemIndex;


        /// <summary>
        /// Returns the number of items in this list.
        /// </summary>
        public int Count {
			get { return items.Count; }
        }

		public T this[int index] {
			get {
				if(index >= items.Count)
					throw new IndexOutOfRangeException();
				return items[index];
			}
			set {
				if(index >= items.Count)
					throw new IndexOutOfRangeException();
				items[index] = value;
			}
		}


        public QueuedList() {
            items = new List<T>();
        }
        public QueuedList(IEnumerable<T> collection) {
            items = new List<T>(collection);
        }
        public QueuedList(int capacity) {
            items = new List<T>(capacity);
        }

		/// <summary>
		/// Clears all items inside this list.
		/// </summary>
		public void Clear() {
			nextItemIndex = 0;
			items.Clear();
		}

		/// <summary>
		/// Returns whether this list contains the specified item.
		/// </summary>
		public bool Contains(T item) {
			return items.Contains(item);
		}

		/// <summary>
		/// Copies the contents of this item to specified array from arrayIndex.
		/// </summary>
		public void CopyTo(T[] array, int arrayIndex) {
			items.CopyTo(array, arrayIndex);
		}
		void ICollection.CopyTo (Array array, int index) {
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Returns the next item in the queue and advances index for next call.
		/// If there is no item in the list, an exception will the thrown.
		/// </summary>
		public T NextItem() {
			if(items.Count == 0)
				throw new IndexOutOfRangeException();

			ValidateIndex();
			return items[nextItemIndex++];
		}

		/// <summary>
		/// Adds the specified item to the queue.
		/// </summary>
		public void Add(T item) {
			items.Add(item);
		}

		/// <summary>
		/// Gets the enumerator of this list.
		/// </summary>
		public IEnumerator<T> GetEnumerator() {
			return items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return items.GetEnumerator();
		}

		bool ICollection.IsSynchronized {
			get {
				throw new NotImplementedException ();
			}
		}

		object ICollection.SyncRoot {
			get {
				throw new NotImplementedException ();
			}
		}

		/// <summary>
		/// Returns the next item in the queue without advancing the index for next call.
		/// If there is no item in the list, an exception will be thrown.
		/// </summary>
		public T Peek() {
			if(items.Count == 0)
				throw new IndexOutOfRangeException();
			ValidateIndex();
			return items[nextItemIndex];
		}

		/// <summary>
		/// Returns a new array with the contents of this list.
		/// </summary>
		public T[] ToArray() {
			return items.ToArray();
		}

		/// <summary>
		/// Trims excess spaces of this list.
		/// </summary>
		public void TrimExcess() {
			items.TrimExcess();
		}

		/// <summary>
		/// Removes the first occurance of specified item.
		/// Returns true if successfully removed.
		/// </summary>
		public bool Remove(T item) {
			for(int i=0; i<items.Count; i++) {
				if(items[i].Equals(item)) {
					RemoveAt_Internal(i);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes the item located at specified index.
		/// May throw an IndexOutOfRangeException
		/// </summary>
		public void RemoveAt(int index) {
			if(index >= items.Count)
				throw new IndexOutOfRangeException();
			RemoveAt_Internal(index);
		}

		/// <summary>
		/// Removes the item at specified index and adjusts index.
		/// </summary>
		void RemoveAt_Internal(int index) {
			items.RemoveAt(index);
			if(nextItemIndex >= index)
				nextItemIndex --;
		}

		/// <summary>
		/// Adjusts item index to make sure its value does not be greater or equal to items count.
		/// </summary>
		void ValidateIndex() {
			if(nextItemIndex >= items.Count)
				nextItemIndex = 0;
		}
    }
}