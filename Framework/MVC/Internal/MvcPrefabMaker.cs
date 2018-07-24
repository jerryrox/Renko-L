#if NGUI && UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Renko.Diagnostics;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework.Internal
{
	public static class MvcPrefabMaker {

		/// <summary>
		/// Creates prefabs for all views in specified config if not already exists.
		/// </summary>
		public static void CreatePrefabs(MVC mvc, MvcConfig config)
		{
			if(mvc.ViewParent == null) {
				RenLog.LogWarning("CreatePrefab - You must assign the ViewParent property on MVC object first.");
				return;
			}

			for(int i=0; i<config.Views.Count; i++)
				Create(mvc, config.Views[i]);
		}

		/// <summary>
		/// Creates a new MVC view prefab for specified view.
		/// </summary>
		private static void Create(MVC mvc, MvcConfig.View view)
		{
			// If prefab file already exists, return
			if(File.Exists(view.GetResourcePath(true)))
				return;
			
			// Create a new object to make prefab.
			GameObject prefab = new GameObject(view.GetViewName());
			InitializeTransform(prefab, mvc.ViewParent.transform);

			// Create a child _Holder object.
			GameObject child = new GameObject("Container");
			InitializeTransform(child, prefab.transform);

			// Attach the UIPanel component first, then WidgetContainer.
			// Even though MVC Views require them as a dependency, it doesn't really look nice with the
			// component order all mixed up.
			prefab.AddComponent<UIPanel>();
			prefab.AddComponent<UIWidgetContainer>();

			// Find view's type and attach a view component on it.
			Type type = null;
			var checkResult = MvcValidator.CheckTypeExists(view.GetViewName(), out type);
			if(checkResult != ValidationResult.Success || type == null) {
				RenLog.LogWarning(string.Format(
					"MvcPrefabMaker.Create - Failed to find type: {0}. Make sure the configuration is applied.",
					view.GetViewName()
				));
				GameObject.DestroyImmediate(prefab);
				return;
			}
			prefab.AddComponent(type);

			// Get UIPanel
			var panel = prefab.GetComponent<UIPanel>();
			panel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
			panel.SetRect(0f, 0f, mvc.BaseResolution.x, mvc.BaseResolution.y);

			// Create prefab
			string path = "Assets/Resources/" + view.GetResourcePath(false) + ".prefab";
			PrefabUtility.CreatePrefab(path, prefab, ReplacePrefabOptions.ConnectToPrefab);
		}

		private static void InitializeTransform(GameObject obj, Transform parent)
		{
			// Set layer
			obj.layer = parent.gameObject.layer;

			// Initialize transform
			Transform prefabTm = obj.transform;
			prefabTm.parent = parent;
			prefabTm.localPosition = Vector3.zero;
			prefabTm.localScale = Vector3.one;
			prefabTm.localRotation = Quaternion.identity;
		}
	}
}
#endif