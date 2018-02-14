#if NGUI
using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// Implementation of MvcLife using recycle method.
	/// </summary>
	public class MvcLifeRecycle : IMvcLife {

		private MvcViewMeta owner;

		private BaseRecycler<IMvcView> recycler;


		public MvcLifeRecycle() {
			recycler = new BaseRecycler<IMvcView>(
				delegate() {
					var view = ResourceLoader.CreateObject(owner.ViewParent, owner.ResourcePath).GetComponent<IMvcView>();
					return view;
				},
				delegate(IMvcView obj) {
					return obj.IsActive;
				},
				null,
				delegate(IMvcView obj) {
					var viewObj = obj.ViewObject;
					GameObject.Destroy(viewObj);
				}
			);
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public void Initialize(MvcViewMeta owner) {
			this.owner = owner;
		}

		/// <summary>
		/// Returns a new MVC view.
		/// </summary>
		public IMvcView NewView (int viewId, JsonObject param) {
			IMvcView view = null;

			// If a new view should be created
			if(recycler.InactiveCount == 0) {
				view = recycler.NextItem();
				view.OnInitialize(viewId, param);
				Timer.CreateFrameDelay(delegate(Timer.Item item) {
					view.OnViewShow();
				});
			}
			// Else, we have a recyclable view.
			else {
				view = recycler.NextItem();
				view.ViewObject.SetActive(true);
				view.OnRecycle(viewId, param);
				view.OnViewShow();
			}

			return view;
		}

		/// <summary>
		/// Disposes the specified view.
		/// </summary>
		public void DisposeView (IMvcView view) {
			view.ViewObject.SetActive(false);
		}

	}
}
#endif