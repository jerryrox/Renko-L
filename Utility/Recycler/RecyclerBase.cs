using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using UnityEngine;

using Renko.Debug;

namespace Renko.Utility
{
	/// <summary>
	/// The base class for recycling reference-type objects.
	/// If you want to use the recycling feature on MonoBehaviours, use the UnityRecycler<T> class instead.
	/// </summary>
	public class RecyclerBase<T> where T : new() {

		/// <summary>
		/// The list which contains all cached instances.
		/// </summary>
		protected List<T> lcObjects;

		/// <summary>
		/// The function which will be called upon creation of an object.
		/// </summary>
		protected Func<T> cInitializer;

		/// <summary>
		/// The event which will be called upon "enabling" of the object.
		/// </summary>
		protected Action<T> cResetter;

		/// <summary>
		/// The event which will be called explicitly by user to disable the object.
		/// </summary>
		protected Action<T> cDisabler;

		/// <summary>
		/// The event which will be called upon destruction of an object.
		/// </summary>
		protected Action<T> cDestroyer;

		/// <summary>
		/// The boolean function which determines whether an object is in active state or not.
		/// If this expression is null, objects will be returned in order from beginning regardless of its activity state (default).
		/// In such cases, you must manually create new objects through the CreateNew() method.
		/// </summary>
		protected Func<T,bool> cActiveValidator;

		/// <summary>
		/// The index value to use when cActiveValidator expression is null.
		/// </summary>
		protected int iQueueOrderInx;


		#region Properties
		/// <summary>
		/// Simply returns the current count of object cache list.
		/// </summary>
		public int Size
		{
			get
			{
				return lcObjects.Count;
			}
		}

		/// <summary>
		/// Returns the number of objects currently in active state.
		/// If the validator is null, 0 will be returned.
		/// </summary>
		public int ActiveCount
		{
			get
			{
				//If validator not defined, just return 0
				if(cActiveValidator == null)
					return 0;

				//Active object count
				int count = 0;

				//For each object in the list
				for(int i=0; i<lcObjects.Count; i++)
				{
					//Current object
					T t = lcObjects[i];

					//If active, add 1 count
					if(cActiveValidator(t))
						count ++;
				}

				//Return final count
				return count;
			}
		}

		/// <summary>
		/// Gets the object located at specified index.
		/// </summary>
		public T this[int index]
		{
			get
			{
				return lcObjects[index];
			}
		}
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes the recycler base with no preallocation.
		/// </summary>
		public RecyclerBase()
		{
			//Instantiate objects list
			lcObjects = new List<T>();

			//Reset queue order index
			iQueueOrderInx = 0;
		}

		/// <summary>
		/// Initializes the recycler base with preallocation.
		/// The parameter (initializer) must not be null.
		/// </summary>
		public RecyclerBase(Func<T> initializer, int preallocCount) : this()
		{
			//Set initializer
			SetInitializer(initializer);

			//Do preallocation given times.
			for(int i=0; i<preallocCount; i++)
				CreateNew();
		}
		#endregion

		#region Set events
		/// <summary>
		/// Sets the initializer event to be called upon creation of an object
		/// </summary>
		public void SetInitializer(Func<T> initializer)
		{
			//Store initializer event
			cInitializer = initializer;
		}

		/// <summary>
		/// Sets the re-initializer event to be called upon "enabling" of an object
		/// </summary>
		public void SetResetter(Action<T> resetter)
		{
			//Store reinitializer event
			cResetter = resetter;
		}

		/// <summary>
		/// Sets the disabler event to be called explicitly by user to disable the object.
		/// </summary>
		public void SetDisabler(Action<T> disabler)
		{
			//Store disabler event
			cDisabler = disabler;
		}

		/// <summary>
		/// Sets the destroyer event to be called upon destruction of an object
		/// </summary>
		public void SetDestroyer(Action<T> destroyer)
		{
			//Store destroyer event
			cDestroyer = destroyer;
		}

		/// <summary>
		/// Sets the validator function to be called upon determination of an object's active state.
		/// If this expression is null, objects will be returned in order from beginning regardless of its activity state (default).
		/// In such cases, you must manually create new objects through the CreateNew() method.
		/// </summary>
		public void SetActiveValidator(Func<T,bool> validator)
		{
			//Store validator function
			cActiveValidator = validator;
		}
		#endregion

		#region Call events
		/// <summary>
		/// Calls the initializer. This method will check whether the initializer is null first.
		/// If null, a default value of type T will be returned.
		/// </summary>
		public virtual T CallInitializer()
		{
			//If initializer not null
			if(cInitializer != null)
				return cInitializer();

			//Just return new instance
			return new T();
		}

		/// <summary>
		/// Calls the resetter for the given object. This method will check whether the resetter is null.
		/// </summary>
		public virtual void CallResetter(T obj)
		{
			//If resetter not null, call resetter
			if(cResetter != null)
				cResetter(obj);
		}

		/// <summary>
		/// Calls the disabler for the given object. This method will check whether the disabler is null.
		/// </summary>
		public virtual void CallDisabler(T obj)
		{
			//If disabler not null, call disabler
			if(cDisabler != null)
				cDisabler(obj);
		}

		/// <summary>
		/// Calls the disabler for all objects in this recycler. This method will check whether the disabler is null.
		/// </summary>
		public virtual void CallDisablerAll()
		{
			//If disabler null, return
			if(cDisabler == null)
				return;

			//For each object in the objects list
			for(int i=0; i<lcObjects.Count; i++)
			{
				//Call disabler for this object
				cDisabler(lcObjects[i]);
			}
		}

		/// <summary>
		/// Calls the destroyer for the given object. This method will check whether the destroyer is null.
		/// </summary>
		public virtual void CallDestroyer(T obj)
		{
			//If destroyer not null, call destroyer
			if(cDestroyer != null)
				cDestroyer(obj);
		}

		/// <summary>
		/// Determines whether the given object is active or not. This method will check whether the validator is null.
		/// If the validator is null, a true value will be returned.
		/// </summary>
		public virtual bool IsObjectActive(T obj)
		{
			//If validator not null, call validator
			if(cActiveValidator != null)
				return cActiveValidator(obj);

			//Return true by default
			return false;
		}
		#endregion

		#region Creating
		/// <summary>
		/// Creates a new instance of an object, adds to object cache list, and returns it.
		/// </summary>
		public virtual T CreateNew(bool ignoreResetter = false)
		{
			//Create new object
			T t = CallInitializer();

			//If not ignoring resetter, call resetter
			if(!ignoreResetter)
				CallResetter(t);

			//Add to objects list
			lcObjects.Add(t);

			//Return the instance
			return t;
		}
		#endregion

		#region Getting
		/// <summary>
		/// Finds a disabled object in the list and returns it.
		/// If no disabled object exists, a new instance will be returned.
		/// If the active evaluator is not defined, objects will be returned in order from first, regardless of its active state.
		/// </summary>
		public virtual T GetObject(bool ignoreResetter = false)
		{
			//Hold object count
			int objCount = lcObjects.Count;
			//Amount to loop
			int loopCount = objCount;
			//Loop until count reaches 0
			while(loopCount > 0)
			{
				//Decrease loop count
				loopCount --;

				//Get object
				T t = lcObjects[iQueueOrderInx++];
				//Send queue index back to 0 if out of bound
				if(iQueueOrderInx >= objCount)
					iQueueOrderInx = 0;

				//If disabled
				if(!IsObjectActive(t))
				{
					//If not ignoring resetter, call resetter
					if(!ignoreResetter)
						CallResetter(t);

					//Return this object
					return t;
				}
			}

			//Return new instance
			return CreateNew(ignoreResetter);
		}
		#endregion

		#region Removing
		/// <summary>
		/// Removes all disabled instances stored in the object list.
		/// </summary>
		public virtual void Trim(bool ignoreDestroyer = false)
		{
			//If active validator not defined
			if(cActiveValidator == null)
			{
				//Show warning message
				RenLog.Log(LogLevel.Warning, "You must define the active validator function before using this method.");
				//Just return.
				return;
			}

			//For each object
			for(int i=lcObjects.Count-1; i>=0; i--)
			{
				//Get instance
				T t = lcObjects[i];

				//If not active
				if(!cActiveValidator(t))
				{
					//If not ignoring destroyer, call destroyer
					if(!ignoreDestroyer)
						CallDestroyer(t);

					//Remove from list
					lcObjects.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Removes all instances stored in the object list.
		/// </summary>
		public virtual void Clear(bool ignoreDestroyer = false)
		{
			//For each object
			for(int i=lcObjects.Count-1; i>=0; i--)
			{
				//If not ignoring destroyer, call destroyer
				if(!ignoreDestroyer)
					CallDestroyer(lcObjects[i]);

				//Remove from list
				lcObjects.RemoveAt(i);
			}
		}

		/// <summary>
		/// Removes the specified object from the recycle list after calling destroyer (if prompted).
		/// </summary>
		public virtual void Remove(T obj, bool ignoreDestroyer = false)
		{
			//If not ignoring destroyer, call destroyer
			if(!ignoreDestroyer)
				CallDestroyer(obj);

			//Remove object from list
			lcObjects.Remove(obj);
		}

		/// <summary>
		/// Removes an object from the recycle list at specified index after calling destroyer (if prompted).
		/// This method will throw an exception if the index is out of range.
		/// </summary>
		public virtual void RemoveAt(int index, bool ignoreDestroyer = false)
		{
			//If out of bounds, throw an error
			if(index < 0 || index >= lcObjects.Count)
				throw new IndexOutOfRangeException();

			//Find the object
			T t = lcObjects[index];

			//If not ignoring destroyer, call destroyer
			if(!ignoreDestroyer)
				CallDestroyer(t);

			//Remove object from list
			lcObjects.RemoveAt(index);
		}
		#endregion
	}
}