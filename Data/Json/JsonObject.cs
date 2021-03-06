﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Renko.Diagnostics;

using SOCollection = System.Collections.Generic.ICollection<
	System.Collections.Generic.KeyValuePair<string, object>
>;

namespace Renko.Data
{
	/// <summary>
	/// A class that represents JSON object.
	/// </summary>
	public class JsonObject : IDictionary<string, JsonData> {

		/// <summary>
		/// A dictionary for storing data.
		/// </summary>
		private Dictionary<string, JsonData> objectData;


		/// <summary>
		/// Gets or sets data at the specified index.
		/// This property will return null JsonData if key doesn't exist.
		/// This property will add a new key if it doesn't exist.
		/// </summary>
		public JsonData this[string index] {
			get {
				if(!objectData.ContainsKey(index))
					return new JsonData(null);
				return objectData[index];
			}
			set {
				if(AddIfNotExists(index, value))
					return;
				objectData[index] = GetNullSafeData(value);
			}
		}

		/// <summary>
		/// Returns the number of items stored in this object.
		/// </summary>
		public int Count {
			get { return objectData.Count; }
		}

		/// <summary>
		/// Returns whether this object is readonly.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}

		/// <summary>
		/// Returns a collection of keys in this object.
		/// </summary>
		public ICollection<string> Keys {
			get { return objectData.Keys; }
		}

		/// <summary>
		/// Returns a collection of values in this object.
		/// </summary>
		public ICollection<JsonData> Values {
			get { return objectData.Values; }
		}


		public JsonObject() {
			objectData = new Dictionary<string, JsonData>();
		}

		/// <summary>
		/// Initializes a new JsonObject from specified collection.
		/// Ideally, this would be Dictionary<string, object>
		/// </summary>
		public JsonObject(SOCollection collection) : this() {
			AddCollection(collection);
		}

		/// <summary>
		/// Initializes a new JsonObject from another JsonObject.
		/// </summary>
		public JsonObject(JsonObject baseObject) {
			MergeFrom(baseObject);
		}

		/// <summary>
		/// Adds the specified key and value.
		/// </summary>
		public void Add (string key, JsonData value) {
			AddIfNotExists(key, value);
		}

		/// <summary>
		/// Adds the specified key value pair.
		/// </summary>
		public void Add (KeyValuePair<string, JsonData> item) {
			AddIfNotExists(item.Key, item.Value);
		}

		/// <summary>
		/// Adds all elements from specified collection.
		/// </summary>
		public void AddCollection(SOCollection collection) {
			var enumerator = collection.GetEnumerator();

			while(enumerator.MoveNext()) {
				var pair = (KeyValuePair<string, object>)enumerator.Current;
				JsonData data = pair.Value as JsonData;
				Add(pair.Key, data ?? new JsonData(pair.Value));
			}
		}

		/// <summary>
		/// Adds all items from specified JsonObject.
		/// </summary>
		public void MergeFrom (JsonObject other, bool overwrite = true) {
			foreach(var pair in other) {
				if(!overwrite) {
					if(objectData.ContainsKey(pair.Key))
						continue;
				}
				this[pair.Key] = pair.Value;
			}
		}

		/// <summary>
		/// Clears all keys and values.
		/// </summary>
		public void Clear() {
			objectData.Clear();
		}

		/// <summary>
		/// Copies the contents of this object to specified array from index.
		/// </summary>
		public void CopyTo (KeyValuePair<string, JsonData>[] array, int arrayIndex) {
			if(array == null) {
				RenLog.Log(LogLevel.Warning, "JsonObject.CopyTo - Parameter 'array' can't be null!");
				return;
			}
			int loopCount = Mathf.Clamp(
				objectData.Count,
				0,
				Mathf.Min(objectData.Count, array.Length - arrayIndex)
			);
			int looped = 0;
			foreach(var pair in objectData)
			{
				//Set reference
				array[looped+arrayIndex] = pair;

				//Change index
				looped ++;
				if(looped >= loopCount)
					break;
			}
		}

		/// <summary>
		/// Returns whether this object contains a value with specified key.
		/// </summary>
		public bool ContainsKey (string key) {
			return objectData.ContainsKey(key);
		}

		/// <summary>
		/// Returns whether this object contains the specified key value pair.
		/// </summary>
		public bool Contains (KeyValuePair<string, JsonData> item) {
			return objectData.ContainsKey(item.Key) && objectData.ContainsValue(item.Value);
		}

		/// <summary>
		/// Removes a value associated with the specified key.
		/// </summary>
		public bool Remove(string key) {
			return objectData.Remove(key);
		}

		/// <summary>
		/// Removes the specified key value pair.
		/// </summary>
		public bool Remove (KeyValuePair<string, JsonData> item) {
			return objectData.Remove(item.Key);
		}

		/// <summary>
		/// Tries to get the value from specified key.
		/// </summary>
		public bool TryGetValue(string key, out JsonData value) {
			return objectData.TryGetValue(key, out value);
		}

		/// <summary>
		/// Returns the generic enumerator of this object.
		/// </summary>
		public IEnumerator<KeyValuePair<string, JsonData>> GetEnumerator () {
			return objectData.GetEnumerator();
		}

		/// <summary>
		/// Returns the non-generic enumerator of this object.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return objectData.GetEnumerator();
		}

		/// <summary>
		/// Returns the string representation of this object using default options.
		/// </summary>
		public override string ToString () {
			return ToString(JsonSerializeOptions.Default);
		}

		/// <summary>
		/// Returns the string representation of this object using specified options.
		/// </summary>
		public string ToString(JsonSerializeOptions options) {
			return JsonSerializer.Serialize(this, options);
		}
		
		/// <summary>
		/// Adds the given key and value if key doesn't already exist.
		/// Returns true if successfully added.
		/// </summary>
		private bool AddIfNotExists(string key, JsonData value) {
			if(key == null) {
				RenLog.Log(LogLevel.Warning, "JsonObject.AddIfNotExists - Parameter 'key' can't be null.");
				return false;
			}
			if(objectData.ContainsKey(key))
				return false;
			objectData.Add(key, GetNullSafeData(value));
			return true;
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