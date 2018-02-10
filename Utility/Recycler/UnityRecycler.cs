using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Diagnostics;

namespace Renko.Utility
{
	/// <summary>
	/// A derivant of BaseRecycler dedicated for MonoBehaviour types.
	/// </summary>
	public class UnityRecycler<T> : BaseRecycler<T> where T : MonoBehaviour {

		/// <summary>
		/// Prefab to instantiate if OnCreateHandler is not assigned.
		/// </summary>
		public GameObject Prefab {
			get; set;
		}

		/// <summary>
		/// Parent transform to instantiate the prefab in.
		/// </summary>
		public Transform Parent {
			get; set;
		}


		/// <summary>
		/// Fires the OnCreate handler and returns new item.
		/// </summary>
		protected override T FireOnCreate() {
			T item = base.FireOnCreate();
			if(item != null)
				return item;
			//Prefab and its component (T) should exist.
			if(Prefab == null || Prefab.GetComponent<T>() == null) {
				RenLog.Log(LogLevel.Warning, "UnityRecycler.FireOnCreate - Prefab must contain the corresponding component.");
				return null;
			}
			return GameObject.Instantiate(Prefab, Parent).GetComponent<T>();
		}

		/// <summary>
		/// Fires the OnCheckValid handler and returns whether it's active (valid).
		/// If handler is undefined, the item's activeInHierarchy value will be returned.
		/// </summary>
		protected override bool FireOnCheckValid(T item) {
			if(OnCheckValidHandler != null)
				return OnCheckValidHandler(item);
			return item.gameObject.activeInHierarchy;
		}

		/// <summary>
		/// Fires the OnReset handler and returns the specified item.
		/// If handler is undefined, the item's GameObject will be activated.
		/// </summary>
		protected override T FireOnReset(T item) {
			if(OnResetHandler != null)
				OnResetHandler(item);
			else
				item.gameObject.SetActive(true);
			return item;
		}

		/// <summary>
		/// Fires the OnDestroy handler and returns the specified item.
		/// If handler is undefined, the item's GameObject will be destroyed.
		/// </summary>
		protected override void FireOnDestroy(T item) {
			if(OnDestroyHandler != null)
				OnDestroyHandler(item);
			else
				GameObject.Destroy(item.gameObject);
		}
	}
}