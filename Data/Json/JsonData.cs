﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Renko.Extensions;
using Renko.Utility;

namespace Renko.Data
{
	/// <summary>
	/// A wrapper of System object class for this Json module.
	/// Supported implicit conversions: int, long, float, bool, JsonObject, JsonArray.
	/// Otherwise, use "new JsonData(your_object_here);"
	/// </summary>
	public class JsonData {

		/// <summary>
		/// The raw object with unknown type.
		/// </summary>
		private object rawObject;


		/// <summary>
		/// The raw object being used.
		/// </summary>
		public object Value {
			get { return rawObject; }
			set { rawObject = Value; }
		}

		/// <summary>
		/// Returns the specific type of the raw object.
		/// </summary>
		public Type Type {
			get { return rawObject.GetType(); }
		}

		/// <summary>
		/// Returns whether the raw object is null.
		/// </summary>
		public bool IsNull {
			get { return rawObject == null; }
		}


		/// <summary>
		/// Initializes a new JsonData instance with null object.
		/// </summary>
		public JsonData() : this(null) {

		}

		/// <summary>
		/// Initializes a new JsonData instance with an object.
		/// </summary>
		public JsonData(object obj) {
			rawObject = obj;
		}

		/// <summary>
		/// Returns an int value stored in this object.
		/// </summary>
		public int AsInt(int defaultValue = 0) {
			if(rawObject.IsNumeric()) {
				try {
					return Convert.ToInt32(rawObject);
				}
				catch(Exception) {
					return defaultValue;
				}
			}
			if(rawObject is string) {
				string str = (string)rawObject;
				int i = 0;
				if(int.TryParse(str, out i))
					return i;
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns a long value stored in this object.
		/// </summary>
		public long AsLong(long defaultValue = 0L) {
			if(rawObject.IsNumeric()) {
				try {
					return Convert.ToInt64(rawObject);
				}
				catch(Exception) {
					return defaultValue;
				}
			}
			if(rawObject is string) {
				string str = (string)rawObject;
				long l = 0L;
				long.TryParse(str, out l);
				return l;
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns a float value stored in this object.
		/// </summary>
		public float AsFloat(float defaultValue = 0f) {
			if(rawObject.IsNumeric()) {
				try {
					return Convert.ToSingle(rawObject);
				}
				catch(Exception) {
					return defaultValue;
				}
			}
			if(rawObject is string) {
				string str = (string)rawObject;
				float f = 0f;
				float.TryParse(str, out f);
				return f;
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns a double value stored in this object.
		/// </summary>
		public double AsDouble(double defaultValue = 0d) {
			if(rawObject.IsNumeric()) {
				try {
					return Convert.ToDouble(rawObject);
				}
				catch(Exception) {
					return defaultValue;
				}
			}
			if(rawObject is string) {
				string str = (string)rawObject;
				double d = 0f;
				double.TryParse(str, out d);
				return d;
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns a bool value stored in this object.
		/// </summary>
		public bool AsBool(bool defaultValue = false) {
			if(rawObject is bool) {
				return (bool)rawObject;
			}
			if(rawObject is string) {
				return BoolParser.Parse(((string)rawObject).ToLower());
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns a string value stored in this object.
		/// </summary>
		public string AsString(string defaultValue = null) {
			if(rawObject == null)
				return defaultValue;
			return rawObject.ToString();
		}

		/// <summary>
		/// Returns a JsonObject value stored in this object.
		/// If you're looking for a System.Object type, use "Value" property.
		/// </summary>
		public JsonObject AsObject(JsonObject defaultValue = null) {
			if(rawObject is JsonObject)
				return (JsonObject)rawObject;
			return defaultValue;
		}

		/// <summary>
		/// Returns a JsonArray value stored in this object.
		/// </summary>
		public JsonArray AsArray(JsonArray defaultValue = null) {
			if(rawObject is JsonArray)
				return (JsonArray)rawObject;
			return defaultValue;
		}

		/// <summary>
		/// Returns a generic type value stored in this object.
		/// </summary>
		public T As<T>(T defaultValue = default(T)) {
			if(rawObject is T)
				return (T)rawObject;
			return defaultValue;
		}

		/// <summary>
		/// Returns the string representation of this object using default options.
		/// If you're looking for a string value stored in this data, use "AsString" method.
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

		
		public static implicit operator int(JsonData context) { return context.AsInt(); }

		public static implicit operator long(JsonData context) { return context.AsLong(); }

		public static implicit operator float(JsonData context) { return context.AsFloat(); }

		public static implicit operator double(JsonData context) { return context.AsDouble(); }

		public static implicit operator bool(JsonData context) { return context.AsBool(); }

		public static implicit operator string(JsonData context) { return context.AsString(); }

		public static implicit operator JsonObject(JsonData context) { return context.AsObject(null); }

		public static implicit operator JsonArray(JsonData context) { return context.AsArray(null); }
		
		public static implicit operator JsonData(int context) { return new JsonData(context); }

		public static implicit operator JsonData(long context) { return new JsonData(context); }

		public static implicit operator JsonData(float context) { return new JsonData(context); }

		public static implicit operator JsonData(double context) { return new JsonData(context); }

		public static implicit operator JsonData(bool context) { return new JsonData(context); }

		public static implicit operator JsonData(string context) { return new JsonData(context); }

		public static implicit operator JsonData(JsonObject context) { return new JsonData(context); }

		public static implicit operator JsonData(JsonArray context) { return new JsonData(context); }
	}
}