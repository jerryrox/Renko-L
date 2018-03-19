using System;

namespace RenkoEditor.Console.Internal
{
	/// <summary>
	/// A helper class for using named property in VariableProvider class.
	/// </summary>
	public class VariablePropertyHelper {

		public object this[string key] {
			get {
				var variable = VariableProvider.GetVariable(key);
				if(variable == null)
					return null;
				return variable.RawObject;
			}
			set {
				VariableProvider.AssignValue(key, value);
			}
		}
	}
}

