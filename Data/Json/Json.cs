using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Data
{
	/// <summary>
	/// A static helper class for this json module.
	/// </summary>
	public static class Json {
		
		/// <summary>
		/// Parses the specified json string and returns a JsonData object.
		/// </summary>
		public static JsonData Parse(string json) {
			return JsonDeserializer.Parse(json);
		}

		/// <summary>
		/// Parses the specified json string for a specific type.
		/// Instance may be null, but if no adaptor is registered for this type and there is no parameterless constructor, the deserialization will not work.
		/// </summary>
		public static T Parse<T>(string json, T instance) where T : new() {
			return Parse<T>(Parse(json).AsObject(), instance);
		}

		/// <summary>
		/// Parses the specified json object for a specific type.
		/// Instance may be null, but if no adaptor is registered for this type and there is no parameterless constructor, the deserialization will not work.
		/// </summary>
		public static T Parse<T>(JsonObject json, T instance) where T : new() {
			if(json == null)
				return default(T);
				
			Type type = typeof(T);
			object value = JsonDeserializer.Deserialize(type, instance, json);
			if(value == null || value is T)
				return default(T);
			return (T)value;
		}

		/// <summary>
		/// Returns a json representation of specified object.
		/// </summary>
		public static string ToString(object data) {
			return ToString(data, JsonSerializeOptions.Default);
		}

		/// <summary>
		/// Returns a json representation of specified object and options.
		/// </summary>
		public static string ToString(object data, JsonSerializeOptions options) {
			return JsonSerializer.Serialize(new JsonData(data), options);
		}
	}
}