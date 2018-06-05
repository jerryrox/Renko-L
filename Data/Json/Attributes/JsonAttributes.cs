using System;

namespace Renko.Data
{
	/// <summary>
	/// Attribute that tells the target non-public field or property will be included during serialization.
	/// Note that this attribute has a higher priority over ignore attibute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class JsonAllowSerializeAttribute : Attribute {
	}

	/// <summary>
	/// Attribute that tells the target member will be ignored during serialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
	public class JsonIgnoreSerializeAttribute : Attribute {
	}
}

