using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Renko.Debug;

namespace Renko.Utility
{
	/// <summary>
	/// A class that represents JSON object.
	/// </summary>
	public class JsonObject : IDictionary<string, JsonData> {

		/// <summary>
		/// A dictionary for storing data.
		/// </summary>
		private Dictionary<string, JsonData> objectData;


		#region Properties
		/// <summary>
		/// Gets or sets data at the specified index.
		/// This property will return null if key doesn't exist.
		/// This property will add a new key if it doesn't exist.
		/// </summary>
		public JsonData this[string index]
		{
			get
			{
				//If key doesn't exist, return null
				if(!objectData.ContainsKey(index))
					return null;

				//Return the data
				return objectData[index];
			}
			set
			{
				//If key didn't exist so a new entry was created, return
				if(AddIfNotExists(index, value))
					return;

				//Set value
				objectData[index].Value = GetNullSafeData(value);
			}
		}

		/// <summary>
		/// Returns the number of items stored in this object.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get { return objectData.Count; } }

		/// <summary>
		/// Returns whether this object is readonly.
		/// </summary>
		public bool IsReadOnly { get { return false; } }

		/// <summary>
		/// Returns a collection of keys in this object.
		/// </summary>
		/// <value>The keys.</value>
		public ICollection<string> Keys { get { return objectData.Keys; } }

		/// <summary>
		/// Returns a collection of values in this object.
		/// </summary>
		public ICollection<JsonData> Values { get { return objectData.Values; } }
		#endregion


		/// <summary>
		/// Initializes a new instance of the <see cref="Renko.Utility.JsonObject"/> class.
		/// </summary>
		public JsonObject()
		{
			//Instantiate raw data dictionary
			objectData = new Dictionary<string, JsonData>();
		}

		#region Core
		/// <summary>
		/// Adds the specified key and value.
		/// </summary>
		public void Add (string key, JsonData value) { AddIfNotExists(key, value); }

		/// <summary>
		/// Adds the specified key value pair.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (KeyValuePair<string, JsonData> item) { AddIfNotExists(item.Key, item.Value); }

		/// <summary>
		/// Clears all keys and values.
		/// </summary>
		public void Clear() { objectData.Clear(); }

		/// <summary>
		/// Copies the contents of this object to specified array from index.
		/// </summary>
		public void CopyTo (KeyValuePair<string, JsonData>[] array, int arrayIndex)
		{
			//Return if array is null
			if(array == null)
			{
				RenLog.Log(LogLevel.Warning, "JsonObject.CopyTo - Parameter 'array' can't be null!");
				return;
			}

			//Get an enumerator for looping
			IEnumerator enumerator = objectData.GetEnumerator();

			//Determine loop count
			int loopCount = Mathf.Clamp(
				objectData.Count,
				0,
				Mathf.Min(objectData.Count, array.Length - arrayIndex)
			);

			//Loop
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
		public bool ContainsKey (string key) { return objectData.ContainsKey(key); }

		/// <summary>
		/// Returns whether this object contains the specified key value pair.
		/// </summary>
		public bool Contains (KeyValuePair<string, JsonData> item)
		{ return objectData.ContainsKey(item.Key) && objectData.ContainsValue(item.Value); }

		/// <summary>
		/// Removes a value associated with the specified key.
		/// </summary>
		public bool Remove(string key) { return objectData.Remove(key); }

		/// <summary>
		/// Removes the specified key value pair.
		/// </summary>
		public bool Remove (KeyValuePair<string, JsonData> item) { return objectData.Remove(item.Key); }

		/// <summary>
		/// Tries to get the value from specified key.
		/// </summary>
		public bool TryGetValue(string key, out JsonData value) { return objectData.TryGetValue(key, out value); }

		/// <summary>
		/// Returns the generic enumerator of this object.
		/// </summary>
		public IEnumerator<KeyValuePair<string, JsonData>> GetEnumerator () { return objectData.GetEnumerator(); } //Just return the default enumerator

		/// <summary>
		/// Returns the non-generic enumerator of this object.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() { return objectData.GetEnumerator(); } //Just return the default enumerator

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Renko.Utility.JsonObject"/>.
		/// </summary>
		public override string ToString () { return JsonSerializer.Serialize(this); }
		#endregion

		#region Helpers
		/// <summary>
		/// Adds the given key and value if key doesn't already exist.
		/// Returns true if successfully added.
		/// </summary>
		private bool AddIfNotExists(string key, JsonData value)
		{
			//If key is null, return
			if(key == null)
			{
				RenLog.Log(LogLevel.Warning, "JsonObject.AddIfNotExists - Parameter 'key' can't be null.");
				return false;
			}

			//Return if key already exists.
			if(objectData.ContainsKey(key))
				return false;

			//Add the key-value to the raw data
			objectData.Add(key, GetNullSafeData(value));
			return true;
		}

		/// <summary>
		/// Returns a JsonData that makes sure null data is not passed.
		/// </summary>
		private JsonData GetNullSafeData(JsonData data)
		{
			//If data is null, return wrapped null inside JsonData
			if(data == null)
				return new JsonData(null);

			//No need to make it safe
			return data;
		}
        #endregion
	}
}