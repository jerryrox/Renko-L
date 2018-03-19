using System;
using Renko.Utility;

namespace RenkoEditor.Console.Internal
{
	public class VariableInfo {

		/// <summary>
		/// The variable name that represents this instance.
		/// </summary>
		public string Name;

		/// <summary>
		/// The specific type of the object.
		/// </summary>
		public Type ObjectType;

		/// <summary>
		/// The actual object data.
		/// </summary>
		public object RawObject;


		public VariableInfo(string name, object value) {
			Name = name;
			SetValue(value);
		}

		/// <summary>
		/// Sets the specified value to this variable.
		/// </summary>
		public void SetValue(object value) {
			if(value == null) {
				ObjectType = null;
			}
			else {
				ObjectType = value.GetType();
			}

			RawObject = value;
		}
	}
}

