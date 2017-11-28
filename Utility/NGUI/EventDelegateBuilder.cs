//TODO: Testing

#if NGUI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A class for building event delegates with parameters easily.
	/// </summary>
	public class EventDelegateBuilder {

		private EventDelegate myDelegate;


		/// <summary>
		/// Returns the event delegate object that you can add on certain events.
		/// </summary>
		public EventDelegate Value {
			get { return myDelegate; }
		}


		public EventDelegateBuilder() {
			myDelegate = new EventDelegate();
		}
		public EventDelegateBuilder(MonoBehaviour target, string methodName) : this() {
			SetTarget(target, methodName);
		}
		
		/// <summary>
		/// Sets the method name to call in target monobehaviour.
		/// Returns this object to allow further calls in a single line.
		/// </summary>
		public EventDelegateBuilder SetTarget(MonoBehaviour target, string methodName) {
			myDelegate.Set(target, methodName);
			return this;
		}

		/// <summary>
		/// Sets the parameter details to send when the event delegate fires.
		/// Returns this object to allow further calls in a single line.
		/// </summary>
		public EventDelegateBuilder SetParam(int index, Object value, string fieldName) {
			myDelegate.parameters[index] = new EventDelegate.Parameter(value, fieldName);
			return this;
		}
	}
}
#endif