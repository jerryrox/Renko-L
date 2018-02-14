using System;
using System.Reflection;
using Renko.Diagnostics;

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
			if(t == null)
				return null;

			//Some primitive types that may need manual instantiation.
			if(t == typeof(String)) return "";
			
			try { return Activator.CreateInstance(t); }
			catch(Exception e) {
				RenLog.Log(LogLevel.Error, string.Format(
					"DynamicService.CreateObject - Could not instantiate type: {0}\n{1}\n{2}",
					t.FullName,
					e.Message,
					e.StackTrace
				));
				return null;
			}
		}
	}
}

