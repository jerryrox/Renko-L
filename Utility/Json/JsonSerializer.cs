using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using Renko.Extensions;

namespace Renko.Utility
{
	/// <summary>
	/// A class that serializes JsonData to a string value.
	/// Used MiniJSON for reference.
	/// </summary>
	public class JsonSerializer
	{
		private StringBuilder sb;


		#region Constructor
		private JsonSerializer()
		{
			//Prepare a new string builder
			sb = new StringBuilder();
		}
		#endregion

		#region Private methods
		/// <summary>
		/// The base method for serializing an unknown json data.
		/// </summary>
		private void Process(object data)
		{
			//Temporary ref for type checking
			JsonObject obj;
			JsonArray arr;

			//If data is JsonObject
			if((obj = data as JsonObject) != null)
				SerializeObject(obj);
			//If data is JsonArray
			else if((arr = data as JsonArray) != null)
				SerializeArray(arr);
			//If none of above
			else
				SerializeData(data);
		}

		/// <summary>
		/// Serializer method for json object.
		/// </summary>
		private void SerializeObject(JsonObject obj)
		{
			//Open bracket
			sb.Append('{');

			//Whether it's the first loop
			bool isFirst = true;

			//For each pair in object
			foreach(var pair in obj)
			{
				//If not first loop, add a comma first
				if(!isFirst)
					sb.Append(',');

				//The key must be enclosed in quotes.
				sb.Append('"').Append(pair.Key).Append("\":");

				//Serize the pair's value
				Process(pair.Value.Value);

				//First loop end
				isFirst = false;
			}

			//Close bracket
			sb.Append('}');
		}

		/// <summary>
		/// Serializer method for json array.
		/// </summary>
		private void SerializeArray(JsonArray arr)
		{
			//Open bracket
			sb.Append('[');

			//Whether it's the first loop
			bool isFirst = true;

			//For each item in array
			for(int i=0; i<arr.Count; i++)
			{
				//If not first loop, add a comma first
				if(!isFirst)
					sb.Append(',');

				//Serialize the item value
				Process(arr[i].Value);

				//First loop end
				isFirst = false;
			}

			//Close bracket
			sb.Append(']');
		}

		/// <summary>
		/// Seializer method for json data.
		/// </summary>
		private void SerializeData(object data)
		{
			//If null
			if(data == null)
			{
				sb.Append("null");
				return;
			}

			//If numeric or bool type
			if(data.IsNumeric() || data is bool)
			{
				//Add the value without enclosing between quotes.
				sb.Append(data.ToString().ToLower());
			}
			//If not numeric
			else
			{
				//Add the value enclosed between quotes.
				sb.Append('"');
				AppendEscapedString(data.ToString());
				sb.Append('"');
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Appends the given string value while escaping non-json-safe characters.
		/// </summary>
		private void AppendEscapedString(string source)
		{
			//For each char
			for(int i=0; i<source.Length; i++)
			{
				//Current character
				char c = source[i];

				//Check for changes required
				switch(c)
				{
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
				default:
					{
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
		#endregion

		#region Public methods
		/// <summary>
		/// Serializes the specified JsonData to a string value.
		/// </summary>
		public static string Serialize(JsonData data)
		{
			//Instantiate a new serializer
			JsonSerializer serializer = new JsonSerializer();

			//Handle serialization and return result
			serializer.Process(data.Value);
			return serializer.sb.ToString();
		}
		#endregion
	}
}

