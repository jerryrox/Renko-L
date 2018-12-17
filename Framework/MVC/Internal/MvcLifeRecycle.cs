#if NGUI
using System;
using UnityEngine;
using Renko.Utility;
using Renko.MVCFramework;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// Implementation of MvcLife using recycle method.
	/// </summary>
	public class MvcLifeRecycle : IMvcLife {

		private MvcViewMeta owner;

		private MvcRecycler recycler;


		public MvcLifeRecycle() {
			recycler = new MvcRecycler(delegate() {
				var view = ResourceLoader.CreateObject(owner.ViewParent, owner.ResourcePath).GetComponent<IMvcView>();
				return view;
			});
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
		public IMvcView NewView (int viewId, MvcRescaleType viewRescaleMode, MvcParameter param) {
			IMvcView view = null;

			// If a new view should be created
			if(recycler.Count == 0) {
				view = recycler.GetView();
				view.OnAdaptView(MVC.ViewSize, viewRescaleMode);
				view.OnInitialize(viewId, param);
				view.OnViewShow();
			}
			// Else, we have a recyclable view.
			else {
				view = recycler.GetView();
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
			view.OnDisposeView();
			recycler.ReturnView(view);
		}

	}
}
#endif