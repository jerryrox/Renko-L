#if NGUI
using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// Implementation of MvcLife using destruction method.
	/// </summary>
	public class MvcLifeDestroy : IMvcLife {

		private MvcViewMeta owner;


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
			var view = ResourceLoader.CreateObject(owner.ViewParent, owner.ResourcePath).GetComponent<IMvcView>();
			view.OnInitialize(viewId, param);
			Timer.CreateFrameDelay(delegate(Timer.Item item) {
				view.OnViewShow();
			});
			return view;
		}

		/// <summary>
		/// Disposes the specified view.
		/// </summary>
		public void DisposeView (IMvcView view) {
			GameObject.Destroy(view.ViewObject);
		}

	}
}
#endif