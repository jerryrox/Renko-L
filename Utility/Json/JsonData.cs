using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Renko.Extensions;

namespace Renko.Utility
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


		#region Properties
		/// <summary>
		/// The raw object being used.
		/// </summary>
		public object Value { get { return rawObject; } set { rawObject = Value; } }

		/// <summary>
		/// Returns the specific type of the raw object.
		/// </summary>
		public Type Type { get { return rawObject.GetType(); } }

		/// <summary>
		/// Returns whether the raw object is null.
		/// </summary>
		public bool IsNull { get { return rawObject == null; } }
		#endregion


		/// <summary>
		/// Initializes a new JsonData instance with null object.
		/// </summary>
		public JsonData() : this(null) { }

		/// <summary>
		/// Initializes a new JsonData instance with an object.
		/// </summary>
		public JsonData(object obj) { rawObject = obj; }

		#region Type conversions
		/// <summary>
		/// Returns an int value stored in this object.
		/// </summary>
		public int AsInt(int defaultValue = 0)
		{
			//If numeric value
			if(rawObject.IsNumeric())
				return (int)rawObject;

			//If a string value
			if(rawObject is string)
			{
				//Get the string value and parse int from that.
				string str = (string)rawObject;
				int i = 0;
				if(int.TryParse(str, out i))
					return i;
			}

			//Else, return 0
			return defaultValue;
		}

		/// <summary>
		/// Returns a long value stored in this object.
		/// </summary>
		public long AsLong(long defaultValue = 0L)
		{
			//If numeric value
			if(rawObject.IsNumeric())
				return (long)rawObject;

			//If a string value
			if(rawObject is string)
			{
				//Get the string value and parse long from that.
				string str = (string)rawObject;
				long l = 0L;
				long.TryParse(str, out l);
				return l;
			}

			//Else, return 0
			return defaultValue;
		}

		/// <summary>
		/// Returns a float value stored in this object.
		/// </summary>
		public float AsFloat(float defaultValue = 0f)
		{
			//If numeric value
			if(rawObject.IsNumeric())
				return (float)rawObject;

			//If a string value
			if(rawObject is string)
			{
				//Get the string value and parse float from that.
				string str = (string)rawObject;
				float f = 0f;
				float.TryParse(str, out f);
				return f;
			}

			//Else, return 0
			return defaultValue;
		}

		/// <summary>
		/// Returns a double value stored in this object.
		/// </summary>
		public double AsDouble(double defaultValue = 0d)
		{
			//If numeric value
			if(rawObject.IsNumeric())
				return (double)rawObject;

			//If a string value
			if(rawObject is string)
			{
				//Get the string value and parse double from that.
				string str = (string)rawObject;
				double d = 0f;
				double.TryParse(str, out d);
				return d;
			}

			//Else, return 0
			return defaultValue;
		}

		/// <summary>
		/// Returns a bool value stored in this object.
		/// </summary>
		public bool AsBool(bool defaultValue = false)
		{
			//If object is a bool type
			if(rawObject is bool)
				return (bool)rawObject;

			//If object is a string type
			if(rawObject is string)
			{
				//Get string value and convert to bool
				return BoolParser.Parse(((string)rawObject).ToLower());
			}

			//Return default
			return defaultValue;
		}

		/// <summary>
		/// Returns a string value stored in this object.
		/// </summary>
		public string AsString(string defaultValue = null)
		{
			//If null object, return default value
			if(rawObject == null)
				return defaultValue;

			//Return a string representation.
			return rawObject.ToString();
		}

		/// <summary>
		/// Returns a JsonObject value stored in this object.
		/// If you're looking for a System.Object type, use "Value" property.
		/// </summary>
		public JsonObject AsObject(JsonObject defaultValue = null)
		{
			//If raw object is the type of jsonobject
			if(rawObject is JsonObject)
				return (JsonObject)rawObject;

			//Return default value
			return defaultValue;
		}

		/// <summary>
		/// Returns a JsonArray value stored in this object.
		/// </summary>
		public JsonArray AsArray(JsonArray defaultValue = null)
		{
			//If raw object is the type of jsonarray
			if(rawObject is JsonArray)
				return (JsonArray)rawObject;

			//Return default value
			return defaultValue;
		}

		/// <summary>
		/// Returns a generic type value stored in this object.
		/// </summary>
		public T As<T>(T defaultValue = default(T))
		{
			//If convertible
			if(rawObject is T) return (T)rawObject;

			//If not, return default value
			return defaultValue;
		}

		/// <summary>
		/// Returns a serialized form of this data.
		/// If you're looking for a string value stored in this data, use "AsString" method.
		/// </summary>
		public override string ToString () { return JsonSerializer.Serialize(this); }
		#endregion

		#region Implicit conversion from JsonData
		public static implicit operator int(JsonData context) { return context.AsInt(); }

		public static implicit operator long(JsonData context) { return context.AsLong(); }

		public static implicit operator float(JsonData context) { return context.AsFloat(); }

		public static implicit operator double(JsonData context) { return context.AsDouble(); }

		public static implicit operator bool(JsonData context) { return context.AsBool(); }

		public static implicit operator string(JsonData context) { return context.AsString(); }

		public static implicit operator JsonObject(JsonData context) { return context.AsObject(null); }

		public static implicit operator JsonArray(JsonData context) { return context.AsArray(null); }
		#endregion

		#region Implicit conversion to JsonData
		public static implicit operator JsonData(int context) { return new JsonData(context); }

		public static implicit operator JsonData(long context) { return new JsonData(context); }

		public static implicit operator JsonData(float context) { return new JsonData(context); }

		public static implicit operator JsonData(double context) { return new JsonData(context); }

		public static implicit operator JsonData(bool context) { return new JsonData(context); }

		public static implicit operator JsonData(string context) { return new JsonData(context); }

		public static implicit operator JsonData(JsonObject context) { return new JsonData(context); }

		public static implicit operator JsonData(JsonArray context) { return new JsonData(context); }
		#endregion
	}
}