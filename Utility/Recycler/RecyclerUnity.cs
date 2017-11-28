using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// The recycler class built exclusively for objects type of MonoBehaviour.
	/// If you want to use the recycling feature on non-MonoBehaviours, use the RecyclerBase<T> class instead.
	/// </summary>
	public class RecyclerUnity<T> : RecyclerBase<T> where T : MonoBehaviour, new() {

		#region Constructors
		public RecyclerUnity() : base()
		{

		}

		public RecyclerUnity(Func<T> initializer, int preallocCount) : base(initializer, preallocCount)
		{

		}
		#endregion

		#region Call events
		/// <summary>
		/// Calls the initializer. This method will check whether the initializer is null first.
		/// If null, a new GameObject will be instantiated with MonoBehaviour T attached to it.
		/// </summary>
		public override T CallInitializer()
		{
			//If initializer not null
			if(cInitializer != null)
				return cInitializer();

			//Just return new instance
			return new GameObject().AddComponent<T>();
		}

		/// <summary>
		/// Calls the resetter for the given object. This method will check whether the resetter is null.
		/// If null, the object's GameObject component will be activated.
		/// </summary>
		public override void CallResetter(T obj)
		{
			//If resetter not null, call resetter
			if(cResetter != null)
			{
				cResetter(obj);
				return;
			}

			//Activate the component's game object
			obj.gameObject.SetActive(true);
		}

		/// <summary>
		/// Calls the disabler for the given object. This method will check whether the disabler is null.
		/// If null, the object's GameObject component will be deactivated.
		/// </summary>
		public override void CallDisabler(T obj)
		{
			//If disabler not null, call disabler
			if(cDisabler != null)
			{
				cDisabler(obj);
				return;
			}

			//Disable the component's game object
			obj.gameObject.SetActive(false);
		}

		/// <summary>
		/// Calls the disabler for all objects in this recycler. This method will check whether the disabler is null.
		/// If null, all objects' GameObject component will be deactivated.
		/// </summary>
		public override void CallDisablerAll()
		{
			//If disabler null
			if(cDisabler == null)
			{
				//For each object
				for(int i=0; i<lcObjects.Count; i++)
				{
					//Deactivate the object's game object component
					lcObjects[i].gameObject.SetActive(false);
				}

				//Return
				return;
			}

			//For each object
			for(int i=0; i<lcObjects.Count; i++)
			{
				//Call disabler for this object
				cDisabler(lcObjects[i]);
			}
		}

		/// <summary>
		/// Calls the destroyer for the given object. This method will check whether the destroyer is null.
		/// If null, the GameObject component of T will be destroyed.
		/// </summary>
		public override void CallDestroyer(T obj)
		{
			//If destroyer not null, call destroyer
			if(cDestroyer != null)
			{
				cDestroyer(obj);
				return;
			}

			//Destroy the gameobject
			UnityEngine.Object.Destroy(obj.gameObject);
		}

		/// <summary>
		/// Determines whether the given object is active or not. This method will check whether the validator is null.
		/// If null, the "activeInHierarchy" property of T's game object will be returned.
		/// </summary>
		public override bool IsObjectActive(T obj)
		{
			//If validator not null, call validator
			if(cActiveValidator != null)
				return cActiveValidator(obj);

			//Return gameobject's active state by default
			return obj.gameObject.activeInHierarchy;
		}
		#endregion
	}
}