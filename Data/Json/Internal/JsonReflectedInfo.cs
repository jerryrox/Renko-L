using System;
using System.Reflection;
using System.Collections.Generic;
using Renko.Extensions;

namespace Renko.Data.Internal
{
	/// <summary>
	/// A class that contains cached field and property infos for performance.
	/// </summary>
	public class JsonReflectedInfo {

		/// <summary>
		/// Whether the TargetType's attribute contains a JsonIgnoreSerializeAttribute.
		/// </summary>
		public bool ShouldIgnore;

		/// <summary>
		/// Whether the TargetType is an anonymous type.
		/// </summary>
		public bool IsAnonymous;

		public Type TargetType;
		public List<FieldInfo> Fields;
		public List<PropertyInfo> Properties;
		public List<FieldInfo> EnumerableFields;
		public List<PropertyInfo> EnumerableProperties;


		public JsonReflectedInfo(Type type) {
			TargetType = type;

			// Assign anonymous flag
			IsAnonymous = type.IsAnonymous();

			Fields = new List<FieldInfo>();
			EnumerableFields = new List<FieldInfo>();
			Properties = new List<PropertyInfo>();
			EnumerableProperties = new List<PropertyInfo>();
		}
	}
}

