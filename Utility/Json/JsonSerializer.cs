﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using Renko.Extensions;
using Renko.Utility.Internal;

namespace Renko.Utility
{
	/// <summary>
	/// A class that serializes JsonData to a string value.
	/// Used MiniJSON for reference.
	/// </summary>
	public class JsonSerializer {

		private StringBuilder sb;


		private JsonSerializer() {
			sb = new StringBuilder();
		}

		/// <summary>
		/// Serializes the specified JsonData to a string value.
		/// </summary>
		public static string Serialize(JsonData data) {
			JsonSerializer serializer = new JsonSerializer();
			serializer.Process(data.Value);
			return serializer.sb.ToString();
		}
		
		/// <summary>
		/// The base method for serializing an unknown json data.
		/// </summary>
		private void Process(object data) {
			JsonObject obj;
			JsonArray arr;

			if((obj = data as JsonObject) != null)
				SerializeObject(obj);
			else if((arr = data as JsonArray) != null)
				SerializeArray(arr);
			else
				SerializeData(data);
		}

		/// <summary>
		/// Serializer method for json object.
		/// </summary>
		private void SerializeObject(JsonObject obj) {
			//Whether it's the first loop
			bool isFirst = true;
			sb.Append('{');

			foreach(var pair in obj) {
				if(!isFirst)
					sb.Append(',');
				sb.Append('"').Append(pair.Key).Append("\":");
				Process(pair.Value.Value);
				isFirst = false;
			}

			sb.Append('}');
		}

		/// <summary>
		/// Serializer method for json array.
		/// </summary>
		private void SerializeArray(JsonArray arr) {
			//Whether it's the first loop
			bool isFirst = true;
			sb.Append('[');

			for(int i=0; i<arr.Count; i++) {
				if(!isFirst)
					sb.Append(',');
				Process(arr[i].Value);
				isFirst = false;
			}

			sb.Append(']');
		}

		/// <summary>
		/// Seializer method for json data.
		/// </summary>
		private void SerializeData(object data) {
			if(data == null) {
				sb.Append("null");
			}
			else if(data.IsNumeric() || data is bool) {
				sb.Append(data.ToString().ToLower());
			}
			else if(data is string) {
				sb.Append('"');	
				AppendEscapedString(data.ToString());
				sb.Append('"');
			}
			else {
				SerializeCustomData(data);
			}
		}

		/// <summary>
		/// Serializes using custom methods (JsonAdaptor, IJsonable, Type serializer).
		/// </summary>
		private void SerializeCustomData(object data) {
			Type dataType = data.GetType();
			JsonObject serializedData = null;

			serializedData = JsonAdaptor.Serialize(dataType, data);
			if(serializedData != null) {
				SerializeObject(serializedData);
				return;
			}

			IJsonable jsonableObject = data as IJsonable;
			if(jsonableObject != null) {
				serializedData = jsonableObject.ToJsonObject();
				if(serializedData != null) {
					SerializeObject(jsonableObject.ToJsonObject());
					return;
				}
			}

			serializedData = JsonTypeSerializer.Serialize(dataType, data);
			if(serializedData != null) {
				SerializeObject(serializedData);
				return;
			}

			//This is when all the above methods fail.
			//Just return the stringified data.
			sb.Append('"');	
			AppendEscapedString(data.ToString());
			sb.Append('"');
		}
		
		/// <summary>
		/// Appends the given string value while escaping non-json-safe characters.
		/// </summary>
		private void AppendEscapedString(string source) {
			for(int i=0; i<source.Length; i++) {
				char c = source[i];
				switch(c) {
				case '"':
					sb.Append("\\\"");
					break;
				case '\\':
					sb.Append("\\\\");
					break;
				case '\b':
					sb.Append("\\b");
					break;
				case '\f':
					sb.Append("\\f");
					break;
				case '\n':
					sb.Append("\\n");
					break;
				case '\r':
					sb.Append("\\r");
					break;
				case '\t':
					sb.Append("\\t");
					break;
				default: {
						//Reference: http://www.asciitable.com/
						int codeNumber = (int)c;
						if ((codeNumber >= 32) && (codeNumber <= 126))
							sb.Append(c);
						//Other characters should be added as \u**** format.
						else
							sb.Append("\\u").Append(Convert.ToString(codeNumber, 16).PadLeft(4, '0'));
					}
					break;
				}
			}
		}
	}
}

