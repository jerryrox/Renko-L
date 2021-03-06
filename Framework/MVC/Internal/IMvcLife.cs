﻿#if NGUI
using System;
using Renko.Utility;
using Renko.MVCFramework;

namespace Renko.MVCFramework.Internal
{
	public interface IMvcLife {

		/// <summary>
		/// Initializes this instance
		/// </summary>
		void Initialize(MvcViewMeta owner);

		/// <summary>
		/// Returns a new MVC view.
		/// </summary>
		IMvcView NewView(int viewId, MvcRescaleType viewRescaleMode, MvcParameter param);

		/// <summary>
		/// Disposes the specified view.
		/// </summary>
		void DisposeView(IMvcView view);
	}
}
#endif