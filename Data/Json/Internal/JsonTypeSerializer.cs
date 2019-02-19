using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Renko.Data;

namespace Renko.Data.Internal
{
	/// <summary>
	/// A class that automatically serializes another class to a JsonData type using its properties and fields.
	/// </summary>
	public class JsonTypeSerializer {

		private JsonReflectedInfo info;
		private JsonObject json;
		private object instance;


		private JsonTypeSerializer(Type type, object obj) {
			info = JsonSerializerReflectionCaches.GetInfo(type);
			if(info == null)
				info = JsonSerializerReflectionCaches.Add(type);
			json = new JsonObject();
			instance = obj;
		}

		/// <summary>
		/// Serializes the specified object to a JsonObject object.
		/// </summary>
		public static JsonObject Serialize(Type type, object obj) {
			return new JsonTypeSerializer(type, obj).Process();
		}

		/// <summary>
		/// Enumerates through target fields and properties to include in the JsonData.
		/// Returns the final output.
		/// </summary>
		public JsonObject Process() {
			for(int i=0; i<info.Fields.Count; i++) {
				ProcessField(info.Fields[i]);
			}
			for(int i=0; i<info.EnumerableFields.Count; i++) {
				ProcessEnumerableField(info.EnumerableFields[i]);
			}
			for(int i=0; i<info.Properties.Count; i++) {
				ProcessProperty(info.Properties[i]);
			}
			for(int i=0; i<info.EnumerableProperties.Count; i++) {
				ProcessEnumerableProperty(info.EnumerableProperties[i]);
			}
			return json;
		}

		/// <summary>
		/// Creates a new item with the specified info's name and instance value.
		/// </summary>
		void ProcessField(FieldInfo info) {
			json[info.Name] = new JsonData(info.GetValue(instance));
		}

		/// <summary>
		/// Creates a new array with the specified info's name and instance values.
		/// </summary>
		void ProcessEnumerableField(FieldInfo info) {
			JsonArray arr = json[info.Name] = new JsonArray();
			IEnumerable enumerable = info.GetValue(instance) as IEnumerable;
			if(enumerable != null) {
				IEnumerator enumerator = enumerable.GetEnumerator();
				if(enumerator != null) {
					while(enumerator.MoveNext())
						arr.Add(new JsonData(enumerator.Current));
				}
			}
		}

		/// <summary>
		/// Creates a new item with the specified info's name and instance value.
		/// </summary>
		void ProcessProperty(PropertyInfo info) {
			json[info.Name] = new JsonData(info.GetValue(instance, null));
		}

		/// <summary>
		/// Creates a new item with the specified info's name and instance values.
		/// </summary>
		void ProcessEnumerableProperty(PropertyInfo info) {
			JsonArray arr = json[info.Name] = new JsonArray();
			IEnumerable enumerable = info.GetValue(instance, null) as IEnumerable;
			if(enumerable != null) {
				IEnumerator enumerator = enumerable.GetEnumerator();
				if(enumerator != null) {
					while(enumerator.MoveNext())
						arr.Add(new JsonData(enumerator.Current));
				}
			}
		}
	}
}

