using System;
using System.Reflection;
using System.Collections.Generic;

namespace Renko.Utility.Internal
{
	/// <summary>
	/// Manages cached information of reflected Types for serialization.
	/// </summary>
	public static class JsonSerializerReflectionCaches {

		/// <summary>
		/// List of cached reflected informations.
		/// </summary>
		private static List<JsonReflectedInfo> infos = new List<JsonReflectedInfo>();


		/// <summary>
		/// Extracts the required information from the specified type and adds to the cache list.
		/// Returns the extracted information.
		/// </summary>
		public static JsonReflectedInfo Add(Type type) {
			JsonReflectedInfo info = new JsonReflectedInfo(type);
			infos.Add(info);

			AddMetaInfo(info, type);

			// We want to avoid unneccessary processes when serialization should be ignored!
			if(!info.shouldIgnore) {
				AddFieldInfo(info, type);
				AddPropertyInfo(info, type);
			}

			return info;
		}

		/// <summary>
		/// Clears all cached reflection info.
		/// </summary>
		public static void Clear() {
			infos.Clear();
		}

		/// <summary>
		/// Returns the cached information associated with specified type.
		/// </summary>
		public static JsonReflectedInfo GetInfo(Type type) {
			for(int i=0; i<infos.Count; i++) {
				if(infos[i].targetType == type)
					return infos[i];
			}
			return Add(type);
		}

		/// <summary>
		/// Returns whether the specified type is ignored during deserialization.
		/// </summary>
		public static bool ShouldIgnoreType(Type type) {
			for(int i=0; i<infos.Count; i++) {
				if(infos[i].targetType == type) {
					return infos[i].shouldIgnore;
				}
			}
			return Add(type).shouldIgnore;
		}

		/// <summary>
		/// Adds miscellaneous info of specified type.
		/// </summary>
		private static void AddMetaInfo(JsonReflectedInfo info, Type type) {
			info.shouldIgnore = (
				type.GetCustomAttributes(typeof(JsonIgnoreSerializeAttribute), false).Length > 0
			);
		}

		/// <summary>
		/// Adds the specified type's fields to the info.
		/// </summary>
		private static void AddFieldInfo(JsonReflectedInfo info, Type type) {
			IEnumerator<FieldInfo> fields = FieldEnumerator(type);
			while(fields.MoveNext()) {
				FieldInfo field = fields.Current;
				Type fieldType = field.FieldType;

				//Field must be non-const, and non-readonly.
				if(field.IsLiteral || field.IsInitOnly)
					continue;
				//JsonIgnoreSerializeAttribute
				if(ShouldIgnoreType(fieldType))
					continue;

				//Is it enumerable? (except string)
				if(fieldType.GetInterface("IEnumerable") != null && fieldType != typeof(string))
					info.enumerableFields.Add(field);
				else
					info.fields.Add(field);
			}
		}

		/// <summary>
		/// Enumerates through all fields in specified type and returns "half-eligible" fields.
		/// </summary>
		private static IEnumerator<FieldInfo> FieldEnumerator(Type type) {
			FieldInfo[] nonPublicFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
			FieldInfo[] publicFields = type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);// | BindingFlags.NonPublic);

			for(int i=0; i<nonPublicFields.Length; i++) {
				// Non-public fields require explicit confirmation for serialization.
				if(nonPublicFields[i].GetCustomAttributes(typeof(JsonAllowSerializeAttribute), false).Length == 0)
					continue;

				yield return nonPublicFields[i];
			}
			for(int i=0; i<publicFields.Length; i++) {
				// Ignoring JsonIgnoreAttribute'd fields
				if(publicFields[i].GetCustomAttributes(typeof(JsonIgnoreSerializeAttribute), false).Length > 0)
					continue;

				yield return publicFields[i];
			}
		}

		/// <summary>
		/// Adds the specified type's properties to the info.
		/// </summary>
		private static void AddPropertyInfo(JsonReflectedInfo info, Type type) {
			IEnumerator<PropertyInfo> properties = PropertyEnumerator(type);
			while(properties.MoveNext()) {
				PropertyInfo property = properties.Current;
				Type propertyType = property.PropertyType;

				//Indexed property is not supported!
				if(property.GetIndexParameters().Length > 0)
					continue;
				//JsonIgnoreSerializeAttribute
				if(ShouldIgnoreType(propertyType))
					continue;

				//If it enumerable? (except string)
				if(propertyType.GetInterface("IEnumerable") != null && propertyType != typeof(string))
					info.enumerableProperties.Add(property);
				else
					info.properties.Add(property);
			}
		}

		/// <summary>
		/// Enumerates through all properties in specified type and returns "half-eligible" properties.
		/// </summary>
		private static IEnumerator<PropertyInfo> PropertyEnumerator(Type type) {
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);

			for(int i=0; i<properties.Length; i++) {
				// Property must be flagged explicitly whether it'll be serialized.
				if(properties[i].GetCustomAttributes(typeof(JsonAllowSerializeAttribute), false).Length == 0)
					continue;
				// Property must be gettable.
				if(properties[i].GetGetMethod(true) == null && properties[i].GetGetMethod(false) == null)
					continue;

				yield return properties[i];
			}
		}
	}
}

