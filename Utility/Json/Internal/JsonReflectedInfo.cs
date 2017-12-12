using System;
using System.Reflection;
using System.Collections.Generic;

namespace Renko.Utility.Internal
{
	/// <summary>
	/// A class that contains cached field and property infos for performance.
	/// </summary>
	public class JsonReflectedInfo {

		/// <summary>
		/// Whether the targetType's attribute contains a JsonIgnoreSerializeAttribute.
		/// </summary>
		public bool shouldIgnore;

		public Type targetType;
		public List<FieldInfo> fields;
		public List<PropertyInfo> properties;
		public List<FieldInfo> enumerableFields;
		public List<PropertyInfo> enumerableProperties;


		public JsonReflectedInfo(Type type) {
			targetType = type;
			fields = new List<FieldInfo>();
			enumerableFields = new List<FieldInfo>();
			properties = new List<PropertyInfo>();
			enumerableProperties = new List<PropertyInfo>();
		}
	}
}

