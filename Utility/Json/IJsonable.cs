using System;

namespace Renko.Utility
{
	/// <summary>
	/// An interface that provides methods for customized serialization of a class.
	/// This interface is prioritized over JsonableAttribute processes.
	/// </summary>
	public interface IJsonable {

		/// <summary>
		/// Serializes this class's contents to a JsonObject type.
		/// </summary>
		JsonObject ToJsonObject();

		/// <summary>
		/// Parses values from specified JsonObject.
		/// </summary>
		void FromJsonObject(JsonObject value);
	}
}

