using System;
using System.Reflection;

namespace Renko.Reflection
{
	/// <summary>
	/// A helper class intended to make-up for the missing support of "dynamic" keyword in Unity.
	/// </summary>
	public static class DynamicService {

		/// <summary>
		/// Returns a new object of specified type.
		/// May return null if the type doesn't contain a parameterless constructor.
		/// </summary>
		public static object CreateObject(Type t) {
			//Some primitive types that may need manual instantiation.
			if(t == typeof(String)) return "";
			
			try { return Activator.CreateInstance(t); }
			catch(Exception e) { return null; }
		}
	}
}

