#if NGUI
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko;
using Renko.Data;
using Renko.Utility;
using Renko.Effects;
using Renko.Diagnostics;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	/// <summary>
	/// MVC manager. You should always use this class for most MVC-related operations.
	/// </summary>
	public partial class MVC : MonoBehaviour {

		/// <summary>
		/// Instance of this class.
		/// </summary>
		private static MVC I;

		/// <summary>
		/// The parent object where all MVC views will be created in.
		/// </summary>
		public GameObject ViewParent;

		/// <summary>
		/// The base resolution to use.
		/// </summary>
		public Vector2 BaseResolution;

		/// <summary>
		/// The default MVC view scaling mode to use during screen size adaptation.
		/// </summary>
		public MvcRescaleType RescaleMode;

		/// <summary>
		/// Whether the application is portrait mode by default.
		/// </summary>
		public bool AppIsPortrait;

		/// <summary>
		/// Whether to initialize the first view on awake.
		/// If false, the developer should manually call the first view handler.
		/// </summary>
		public bool InitialViewOnAwake;

		/// <summary>
		/// Type of method to use for UI lifecycle.
		/// </summary>
		public MvcLifeType UiLifeType;

		/// <summary>
		/// Immutable collection of UI create handlers.
		/// </summary>
		private Dictionary<Type,MvcViewMeta> uiMetadatas;

		/// <summary>
		/// Mutable collection of UI callback handlers.
		/// </summary>
		private Dictionary<int,UICallbackHandler> uiCallbacks;

		/// <summary>
		/// List of all active views' interface.
		/// </summary>
		private List<IMvcView> activeViews;

		/// <summary>
		/// Whether the core components are initialized.
		/// </summary>
		private bool isCoreInitialized;

		/// <summary>
		/// The Type of initial view.
		/// </summary>
		private Type firstViewType;

		/// <summary>
		/// Backing field of NextViewID property.
		/// </summary>
		private int nextViewId;

		/// <summary>
		/// Backing field of ViewSize property.
		/// </summary>
		private ScreenAdaptor viewSize;


		/// <summary>
		/// Returns the ID for a newly created view.
		/// </summary>
		public static int NextViewID {
			get { return ++I.nextViewId; }
		}

		/// <summary>
		/// Returns a list of all active views.
		/// </summary>
		public static List<IMvcView> ActiveViews {
			get { return I.activeViews; }
		}

		/// <summary>
		/// Returns the screen adaptor for MVC views to scale their views if necessary.
		/// </summary>
		public static ScreenAdaptor ViewSize {
			get { return I.viewSize; }
		}


		/// <summary>
		/// Delegate for a callback from UI destruction.
		/// </summary>
		public delegate void UICallbackHandler(JsonObject param);


		void Awake() {
			// Field initialization
			I = this;
			uiCallbacks = new Dictionary<int, UICallbackHandler>();
			activeViews = new List<IMvcView>();

			// Initialize required libraries
			InitializeResolutions();
			Timer.Initialize();
			FateFX.Initialize();

			// Core MVC initialization.
			// This method is auto-generated in a new partial script so we use SendMessage.
			SendMessage("InitializeCore", SendMessageOptions.RequireReceiver);

			// Final process
			FinalizeAwake();
		}

		/// <summary>
		/// Shows the specified view with parameters.
		/// </summary>
		public static IMvcView ShowView(Type viewType, JsonObject param = null) {
			// Unregistered type is an invalid request.
			if(!I.uiMetadatas.ContainsKey(viewType)) {
				Debug.LogWarning("MVC.ShowView - No MVC metadata found for specified type: " + viewType.FullName);
				return null;
			}

			// Null-checks are dirty, so we just pass a non-null value.
			if(param == null)
				param = new JsonObject();

			// Show view
			var view = I.uiMetadatas[viewType].OnShow(NextViewID, param);
			I.activeViews.Add(view);
			return view;
		}

		/// <summary>
		/// Hides the specified view and returns its data.
		/// </summary>
		public static JsonObject HideView(IMvcView view) {
			JsonObject returnData = view.OnViewHide();
			DoCallback(view.ViewId, returnData);
			return returnData;
		}

		/// <summary>
		/// Disposes the view.
		/// </summary>
		public static void DisposeView(IMvcView view) {
			Type viewType = view.GetType();
			// Unregistered type is an invalid request.
			if(!I.uiMetadatas.ContainsKey(viewType)) {
				Debug.LogWarning("MVC.DisposeView - No MVC metadata found for specified type: " + viewType.FullName);
				return;
			}

			I.uiMetadatas[viewType].OnDispose(view);
			I.activeViews.Remove(view);
		}

		/// <summary>
		/// Registers a callback event to invoke when the target view is destroyed.
		/// </summary>
		public static void RegisterCallback(IMvcView targetView, UICallbackHandler callback) {
			if(I.uiCallbacks.ContainsKey(targetView.ViewId))
				I.uiCallbacks[targetView.ViewId] = callback;
			else
				I.uiCallbacks.Add(targetView.ViewId, callback);
		}

		/// <summary>
		/// Invokes the callback attached to the specified view id.
		/// </summary>
		public static void DoCallback(int viewId, JsonObject returnData) {
			if(I.uiCallbacks.ContainsKey(viewId)) {
				I.uiCallbacks[viewId].Invoke(returnData);
				I.uiCallbacks.Remove(viewId);
			}
		}

		/// <summary>
		/// Initializes the Resolutions library.
		/// </summary>
		void InitializeResolutions() {
			viewSize = new ScreenAdaptor(BaseResolution);
		}

		/// <summary>
		/// Finalizes the awake process.
		/// </summary>
		void FinalizeAwake() {
			// No core, no MVC
			if(!isCoreInitialized) {
				RenLog.Log(LogLevel.Error, "MVC.FinalizeAwake - Core MVC components are not initialized!");
				Destroy(gameObject);
				return;
			}

			// Show first view, then we're finished :D
			if(InitialViewOnAwake && firstViewType != null) {
				ShowView(firstViewType);
			}
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Shows the initial view via editor context menu.
		/// </summary>
		[ContextMenu("Show initial view")]
		void ShowInitialView() {
			if(!EditorApplication.isPlaying)
				return;
			if(firstViewType == null) {
				RenLog.Log(LogLevel.Warning, "MVC.ShowInitialView - The initial view type is not defined.");
				return;
			}

			ShowView(firstViewType);
		}
		#endif
	}
}
#endif