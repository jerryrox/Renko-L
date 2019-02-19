#if NGUI
using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;
using Renko.LapseFramework;

namespace Renko.MVCFramework
{
	/// <summary>
	/// The base class of all MVC view components.
	/// </summary>
	public class BaseMvcView : MonoBehaviour, IMvcView {
		
		/// <summary>
		/// Just keeping this value as a reference rather than hard-coding it every time an editor script needs it.
		/// </summary>
		public const string ClassName = "BaseMvcView";

		/// <summary>
		/// Whether to reset the UIPanel's rect based on current resolution.
		/// </summary>
		public bool ResizePanel = true;

		/// <summary>
		/// Backing field of ViewSize property.
		/// </summary>
		public ScreenAdaptor ViewSize;

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
		/// The longest hide animation for temporary use.
		/// </summary>
		private BaseMvcAnimation longestHideAni;


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
		/// For integration with auto generated code with MVC base views.
		/// You should use OnInitialize for the actual initialization process.
		/// </summary>
		public virtual void Awake() {
			// Nothing to do!
		}

		/// <summary>
		/// Use this method to resize UIPanel and handle view anchoring.
		/// Called ONLY once after Awake() and before OnInitialize().
		/// </summary>
		public virtual void OnAdaptView(ScreenAdaptor viewSize, MvcRescaleType type) {
			this.ViewSize = viewSize;

			if(ResizePanel) {
				Vector2 newSize = viewSize.GetScaledResolution((ScreenAdaptor.ScaleMode)(int)type);
				UIPanel up = GetComponent<UIPanel>();
				if(up != null) {
					up.SetRect(0f, 0f, newSize.x, newSize.y);
				}
			}
		}

		/// <summary>
		/// Use this method to handle initialization of fields, resources, etc.
		/// Called ONLY once after Awake() and OnAdaptView().
		/// </summary>
		public virtual void OnInitialize(int viewId, MvcParameter param) {
			this.viewId = viewId;
			isDestroyed = false;
		}

		/// <summary>
		/// Use this method to handle re-initialization of fields, resources, etc.
		/// Will invoke OnViewShow() afterwards.
		/// Called everytime this view is being recycled.
		/// </summary>
		public virtual void OnRecycle(int viewId, MvcParameter param) {
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
		/// You should return a MvcParameter value that represents a return data from this view.
		/// If none, just return null.
		/// Don't call this base method if you wish to handle animation yourself.
		/// </summary>
		public virtual MvcParameter OnViewHide() {
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
			longestHideAni = null;
			for(int i=0; i<ani.Length; i++) {
				var curAni = ani[i];
				if(curAni.Play(MvcAnimationEvent.OnViewHide)) {

					if(longestHideAni == null) {
						longestHideAni = curAni;
					}
					else if(curAni.Duration > longestHideAni.Duration) {
						longestHideAni = curAni;
					}
				}
				else if(curAni.TargetEvent == MvcAnimationEvent.OnViewShow) {
					curAni.FateAni.Stop();
				}
			}

			// Raising view disposal event.
			if(longestHideAni != null) {
				longestHideAni.FateAni.AddEvent(FateEvents.OnEnd, OnHideAniEnded);
			}
			else {
				MVC.DisposeView(this);
			}
		}

		/// <summary>
		/// Callback method after longestHideAni finishes.
		/// </summary>
		void OnHideAniEnded(IFateTimer item)
		{
			longestHideAni.FateAni.RemoveEvent(FateEvents.OnEnd, OnHideAniEnded);
			MVC.DisposeView(this);
		}
	}
}
#endif