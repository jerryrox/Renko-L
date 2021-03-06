﻿#if NGUI
using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;

namespace Renko.MVCFramework
{
	/// <summary>
	/// The interface to all MVC views.
	/// You can implement this interface to create your custom base class for MVC views.
	/// </summary>
	public interface IMvcView {

		/// <summary>
		/// Returns the gameObject component of this view.
		/// </summary>
		GameObject ViewObject { get; }

		/// <summary>
		/// The unique id received from UIController upon creation.
		/// </summary>
		int ViewId { get; }

		/// <summary>
		/// Returns whether this view is active.
		/// If you're using Recycle method, MVC will check for this flag whether this view can be reused.
		/// </summary>
		bool IsActive { get; }


		/// <summary>
		/// For integration with auto generated code with MVC base views.
		/// You should use OnInitialize for the actual initialization process.
		/// </summary>
		void Awake();

		/// <summary>
		/// Use this method to resize UIPanel and handle view anchoring.
		/// Called ONLY once after Awake() and before OnInitialize().
		/// </summary>
		void OnAdaptView(ScreenAdaptor viewSize, MvcRescaleType type);

		/// <summary>
		/// Use this method to handle initialization of fields, resources, etc.
		/// Called ONLY once after Awake() and OnAdaptView().
		/// </summary>
		void OnInitialize(int viewId, MvcParameter param);

		/// <summary>
		/// Use this method to handle re-initialization of fields, resources, etc.
		/// Will invoke OnViewShow() afterwards.
		/// Called everytime this view is being recycled.
		/// </summary>
		void OnRecycle(int viewId, MvcParameter param);

		/// <summary>
		/// Use this method to handle view setup. Ideal place for a show animation, if any.
		/// </summary>
		void OnViewShow();

		/// <summary>
		/// Use this method to handle view hiding. Ideal place for a hide animation, if any.
		/// You should return a MvcParameter value that represents a return data from this view.
		/// If none, just return null.
		/// </summary>
		MvcParameter OnViewHide();

		/// <summary>
		/// Use this method to dispose unused resources.
		/// Called right before destruction/deactivation of the view for cleanup.
		/// </summary>
		void OnDisposeView();
	}
}
#endif