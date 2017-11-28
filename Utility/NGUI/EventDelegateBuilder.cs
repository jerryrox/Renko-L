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


		#region Property
		/// <summary>
		/// Returns the event delegate object that you can add on certain events.
		/// </summary>
		public EventDelegate Value
		{ get { return myDelegate; } }
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Renko.Utility.EventDelegateBuilder"/> class.
		/// </summary>
		public EventDelegateBuilder()
		{
			//Instantiate a new event delegate object
			myDelegate = new EventDelegate();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Renko.Utility.EventDelegateBuilder"/> class.
		/// </summary>
		public EventDelegateBuilder(MonoBehaviour target, string methodName) : this()
		{
			//Set target
			SetTarget(target, methodName);
		}
		#endregion

		#region Core
		/// <summary>
		/// Sets the method name to call in target monobehaviour.
		/// Returns this object to allow further calls in a single line.
		/// </summary>
		public EventDelegateBuilder SetTarget(MonoBehaviour target, string methodName)
		{
			//Set target monobehaviour and method name
			myDelegate.Set(target, methodName);
			return this;
		}

		/// <summary>
		/// Sets the parameter details to send when the event delegate fires.
		/// Returns this object to allow further calls in a single line.
		/// </summary>
		public EventDelegateBuilder SetParam(int index, Object value, string fieldName)
		{
			//Create a new parameter at specified index.
			myDelegate.parameters[index] = new EventDelegate.Parameter(value, fieldName);
			return this;
		}
		#endregion
	}
}
#endif