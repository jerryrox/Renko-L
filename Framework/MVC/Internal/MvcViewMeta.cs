#if NGUI
using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Data;

namespace Renko.MVCFramework.Internal
{
	/// <summary>
	/// A class that contains some metadata about a single MVC View.
	/// </summary>
	public class MvcViewMeta {

		/// <summary>
		/// The relative path of the view prefab inside resources folder.
		/// </summary>
		public string ResourcePath;

		/// <summary>
		/// The parent object which the newly created view will be nested in.
		/// </summary>
		public GameObject ViewParent;

		/// <summary>
		/// Interface to a specific MVC view life handler.
		/// </summary>
		private IMvcLife mvcLifeHandler;

		/// <summary>
		/// Type of MVC view scale mode.
		/// </summary>
		private MvcRescaleType viewRescaleMode;



		public MvcViewMeta(MVC mvc, MvcLifeType lifeType, MvcRescaleType viewRescaleMode,
			string resourcePath, GameObject viewParent)
		{
			this.ResourcePath = resourcePath;
			this.ViewParent = viewParent;

			if(viewRescaleMode == MvcRescaleType.Default)
				viewRescaleMode = mvc.RescaleMode;
			this.viewRescaleMode = viewRescaleMode;

			mvcLifeHandler = GetLifeHandler(lifeType == MvcLifeType.Default ? mvc.UiLifeType : lifeType);
			mvcLifeHandler.Initialize(this);
		}

		/// <summary>
		/// Returns a new MVC view instance.
		/// </summary>
		public IMvcView OnShow(int viewId, JsonObject param) {
			return mvcLifeHandler.NewView(viewId, viewRescaleMode, param);
		}

		/// <summary>
		/// Disposes the specified view instance.
		/// </summary>
		public void OnDispose(IMvcView view) {
			mvcLifeHandler.DisposeView(view);
		}

		/// <summary>
		/// Instantiates the appropriate MVC life handler for specified type.
		/// </summary>
		IMvcLife GetLifeHandler(MvcLifeType lifeType) {
			switch(lifeType) {
			case MvcLifeType.Destroy:
				return new MvcLifeDestroy();
			case MvcLifeType.Recycle:
				return new MvcLifeRecycle();
			}
			return null;
		}
	}
}
#endif