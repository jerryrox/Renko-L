#if NGUI
using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;
using Renko.Effects;

namespace Renko.MVCFramework
{
	/// <summary>
	/// The base class of all MVC view components.
	/// 
	/// </summary>
	public class BaseMvcView : MonoBehaviour, IMvcView {

		/// <summary>
		/// Just for keeping this value as a reference rather than hard-coding it every time I need it.
		/// </summary>
		public const string ClassName = "BaseMvcView";

		/// <summary>
		/// Whether to reset the UIPanel's rect based on current resolution.
		/// </summary>
		public bool ResizePanel = true;


		/// <summary>
		/// Whether the view is no longer being managed by MVC.
		/// </summary>
		private bool isDestroyed;

		/// <summary>
		/// Backing field of ViewId property.
		/// </summary>
		private int viewId;

		/// <summary>
		/// Backing field of Animations property for caching.
		/// </summary>
		private BaseMvcAnimation[] animations;


		/// <summary>
		/// Returns the gameObject component of this view.
		/// </summary>
		public GameObject ViewObject {
			get { return gameObject; }
		}

		/// <summary>
		/// The unique id received from UIController upon creation.
		/// </summary>
		public int ViewId {
			get { return viewId; }
		}

		/// <summary>
		/// Returns whether this view is active.
		/// If you're using Recycle method, MVC will check for this flag whether this view can be reused.
		/// </summary>
		public bool IsActive {
			get { return gameObject.activeInHierarchy && !isDestroyed; }
		}

		/// <summary>
		/// Array of animations included in this view.
		/// </summary>
		public BaseMvcAnimation[] Animations {
			get {
				if(animations == null)
					animations = GetComponentsInChildren<BaseMvcAnimation>(true);
				return animations;
			}
		}


		/// <summary>
		/// MonoBehaviour's standard Awake method for pre-initialization.
		/// </summary>
		public virtual void Awake() {
			if(ResizePanel) {
				UIPanel up = GetComponent<UIPanel>();
				if(up != null) {
					up.SetRect(0f, 0f, Resolutions.Width, Resolutions.Height);
				}
			}
		}

		/// <summary>
		/// Use this method to handle initialization of fields, resources, etc.
		/// Called right after Awake().
		/// </summary>
		public virtual void OnInitialize(int viewId, JsonObject param) {
			this.viewId = viewId;
			isDestroyed = false;
		}

		/// <summary>
		/// Use this method to handle re-initialization of fields, resources, etc.
		/// Will invoke OnViewInitialize() afterwards.
		/// </summary>
		public virtual void OnRecycle(int viewId,JsonObject param) {
			this.viewId = viewId;
			isDestroyed = false;
		}

		/// <summary>
		/// Use this method to handle view setup. Ideal place for a show animation, if any.
		/// </summary>
		public virtual void OnViewShow() {
			PlayShowAnimations();
		}

		/// <summary>
		/// Use this method to handle view hiding. Ideal place for a hide animation, if any.
		/// You should return a JsonObject value that represents a return data from this view.
		/// If none, just return null.
		/// Don't call this base method if you wish to handle animation yourself.
		/// </summary>
		public virtual JsonObject OnViewHide() {
			PlayHideAnimations();
			return null;
		}

		/// <summary>
		/// Use this method to dispose unused resources.
		/// Called right before destruction/deactivation of the view for cleanup.
		/// </summary>
		public virtual void OnDisposeView() {
			isDestroyed = true;
		}

		/// <summary>
		/// Plays view animations flagged to view show event.
		/// </summary>
		public void PlayShowAnimations() {
			var ani = Animations;
			for(int i=0; i<ani.Length; i++)
				ani[i].Play(MvcAnimationEvent.OnViewShow);
		}

		/// <summary>
		/// Plays view animations flagged to view hide event.
		/// Will also handle view disposal action.
		/// </summary>
		public void PlayHideAnimations() {
			var ani = Animations;
			BaseMvcAnimation longestAni = null;
			for(int i=0; i<ani.Length; i++) {
				var curAni = ani[i];
				if(curAni.Play(MvcAnimationEvent.OnViewHide)) {

					if(longestAni == null) {
						longestAni = curAni;
					}
					else if(curAni.Duration > longestAni.Duration) {
						longestAni = curAni;
					}
				}
				else if(curAni.TargetEvent == MvcAnimationEvent.OnViewShow) {
					curAni.FateAni.Stop();
				}
			}

			// Raising view disposal event.
			if(longestAni != null) {
				longestAni.FateAni.OnReset += delegate(FateItem item) {
					MVC.DisposeView(this);
				};
			}
			else {
				MVC.DisposeView(this);
			}
		}
	}
}
#endif