using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Renko.Debug;

namespace Renko.Utility
{
	/// <summary>
	/// A class that simply wraps a Dictionary object.
	/// Will be 
	/// </summary>
	public class ObjectMap {
		
		/// <summary>
		/// The actual parameter map used internally
		/// </summary>
		private Dictionary<string, object> myMap;


		/// <summary>
		/// An identifier (key) which represents this map instance.
		/// Use it if you need for some reason.
		/// </summary>
		public string Identifier {
			get; set;
		}


		/// <summary>
		/// The one and only constructor :)
		/// </summary>
		public ObjectMap() {
			myMap = new Dictionary<string, object>();
		}

		/// <summary>
		/// Gets the value associated with given key.
		/// </summary>
		public T GetValue<T>(string key, T defaultValue = default(T)) {
			try {
				return (T)myMap[key];
			}
			catch(Exception e) {
				RenLog.Log(LogLevel.Warning, "ObjectMap.GetValue - " + e.Message + "\n" + e.StackTrace);
				return defaultValue;
			}
		}

		/// <summary>
		/// Creates a new key/value pair to this map.
		/// Replaces value if the key already exists.
		/// </summary>
		public ObjectMap SetValue(string key, object value) {
			try {
				if(myMap.ContainsKey(key))
					myMap[key] = value;
				else
					myMap.Add(key, value);
			}
			catch(Exception e) {
				RenLog.Log(LogLevel.Warning, "ObjectMap.SetValue - " + e.Message + "\n" + e.StackTrace);
			}
			return this;
		}

		/// <summary>
		/// Imports data from the other ObjectMap instance.
		/// Override priority is on the other map.
		/// </summary>
		public ObjectMap Combine(ObjectMap otherMap) {
			foreach(var pair in otherMap.myMap) {
				SetValue(pair.Key, pair.Value);
			}
			return this;
		}

		/// <summary>
		/// Returns whether this map contains the specified key.
		/// </summary>
		public bool ContainsKey(string key) {
			return myMap.ContainsKey(key);
		}
	}
}