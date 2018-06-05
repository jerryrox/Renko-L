using System;
using System.Collections.Generic;
using Renko.Utility.Internal;

namespace Renko.Utility
{
	/// <summary>
	/// A class that contains a dictionary of customized (de)serialization handlers for a specific object type.
	/// This adaptor is prioritized over IJsonable and JsonableAttribute processes.
	/// </summary>
	public static class JsonAdaptor {

		/// <summary>
		/// Dictionary of custom serializers for a specific type.
		/// </summary>
		private static Dictionary<Type, Info> handlers = new Dictionary<Type, Info>(JsonAdaptorPresets.Fetch());


		/// <summary>
		/// Delegate for handling the actual serialization process and returning the JsonData value.
		/// </summary>
		public delegate JsonData SerializeHandler(object value);
		/// <summary>
		/// Delegate for handling the actual deserialization process and returning the object value.
		/// </summary>
		public delegate object DeserializeHandler(JsonObject value);


		/// <summary>
		/// Registers the specified type and handlers.
		/// If the specified type already exists, the handlers will be replaced by the newer ones.
		/// HIGHLY recommended to use static methods for the handlers.
		/// </summary>
		public static void Register(Type type, SerializeHandler serializer, DeserializeHandler deserializer) {
			if(!handlers.ContainsKey(type))
				handlers.Add(type, new Info(serializer, deserializer));
			else
				handlers[type].Replace(serializer, deserializer);
		}

		/// <summary>
		/// Returns a serialized JsonObject of specified value.
		/// May return null if no handler matching the type was registered.
		/// </summary>
		public static JsonObject Serialize(Type type, object value) {
			if(handlers.ContainsKey(type))
				return handlers[type].Serialize(value);
			return null;
		}

		/// <summary>
		/// Returns a deserialized object of specified JsonObject.
		/// May return null if no handler matching the type was registered.
		/// </summary>
		public static object Deserialize(Type type, JsonObject value) {
			if(handlers.ContainsKey(type))
				return handlers[type].Deserialize(value);
			return null;
		}


		/// <summary>
		/// A class that contains the actual (de)serialization handlers.
		/// </summary>
		public class Info {

			private SerializeHandler serializer;
			private DeserializeHandler deserializer;


			public Info(SerializeHandler s, DeserializeHandler d) {
				serializer = s;
				deserializer = d;
			}

			public JsonData Serialize(object value) {
				if(serializer == null)
					return null;
				return serializer(value);
			}

			public object Deserialize(JsonData value) {
				if(deserializer == null)
					return null;
				return deserializer(value);
			}

			/// <summary>
			/// Replaces the handlers in this object with the specified handlers.
			/// If a given handler is null, the replacement process will be ignored.
			/// </summary>
			public void Replace(SerializeHandler s, DeserializeHandler d) {
				if(s != null)
					serializer = s;
				if(d != null)
					deserializer = d;
			}
		}
	}
}

