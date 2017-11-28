using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Debug;

namespace Renko.Utility
{
	/// <summary>
	/// A class that represents JSON array.
	/// </summary>
	public class JsonArray : IList<JsonData> {

		/// <summary>
		/// A list for storing data.
		/// </summary>
		private List<JsonData> listData;


		/// <summary>
		/// Gets or sets data at specified index.
		/// This property returns null if index is out of range.
		/// This property will automatically resize the array if index >= item count.
		/// </summary>
		public JsonData this[int index] {
			get {
				if(index >= listData.Count || index < 0)
					return null;
				return listData[index];
			}
			set {
				if(index < 0) {
					RenLog.Log(LogLevel.Warning, "JsonArray.this[] - Index can't be less than 0.");
					return;
				}
				//Add dummy items to avoid index out of range exception
				AddDummy(index+1 - listData.Count);
				listData[index] = GetNullSafeData(value);;
			}
		}

		/// <summary>
		/// Returns the number of items stored.
		/// </summary>
		public int Count {
			get { return listData.Count; }
		}

		/// <summary>
		/// Returns whether this object is readonly.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}


		public JsonArray() {
			listData = new List<JsonData>();
		}

		/// <summary>
		/// Returns the index of specified item.
		/// </summary>
		public int IndexOf (JsonData item) {
			return listData.IndexOf(item);
		}

		/// <summary>
		/// Inserts the specified item at index.
		/// </summary>
		public void Insert (int index, JsonData item) {
			listData.Insert(index, GetNullSafeData(item));
		}

		/// <summary>
		/// Removes the item at specified index.
		/// </summary>
		public void RemoveAt (int index) {
			listData.RemoveAt(index);
		}

		/// <summary>
		/// Adds the specified item to the end
		/// </summary>
		public void Add(JsonData item) {
			listData.Add(GetNullSafeData(item));
		}

		/// <summary>
		/// Clears all items from the array.
		/// </summary>
		public void Clear () {
			listData.Clear();
		}

		/// <summary>
		/// Returns whether the array contains the specified data.
		/// </summary>
		public bool Contains (JsonData item) {
			return listData.Contains(item);
		}

		/// <summary>
		/// Copies data to specified array from index.
		/// </summary>
		void ICollection<JsonData>.CopyTo (JsonData[] array, int arrayIndex) {
			listData.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the specified item from this array.
		/// </summary>
		public bool Remove (JsonData item) {
			return listData.Remove(item);
		}

		/// <summary>
		/// Returns the generic enumerator of this object.
		/// </summary>
		public IEnumerator<JsonData> GetEnumerator () {
			return listData.GetEnumerator();
		}

		/// <summary>
		/// Returns the non-generic enumerator of this object.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator () {
			return listData.GetEnumerator();
		}

		public override string ToString () {
			return JsonSerializer.Serialize(this);
		}

		/// <summary>
		/// Adds JsonData with null value by specified count.
		/// </summary>
		private void AddDummy(int count) {
			for(int i=0; i<count; i++)
				listData.Add(new JsonData(null));
		}

		/// <summary>
		/// Returns a JsonData that makes sure null data is not passed.
		/// </summary>
		private JsonData GetNullSafeData(JsonData data) {
			if(data == null)
				return new JsonData(null);
			return data;
		}
	}
}