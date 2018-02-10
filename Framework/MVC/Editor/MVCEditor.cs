#if NGUI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	[CustomEditor(typeof(MVC))]
	public class MVCEditor : Editor {

		/// <summary>
		/// Direct instance to the target object being inspected.
		/// </summary>
		public MVC I;

		public SerializedProperty BaseResolution;
		public SerializedProperty MatchResolutionToWidth;
		public SerializedProperty AppIsPortrait;
		public SerializedProperty InitializeViewOnAwake;
		public SerializedProperty UiLifecycleMethod;



		void OnEnable() {
			I = target as MVC;

			BaseResolution = serializedObject.FindProperty("BaseResolution");
			MatchResolutionToWidth = serializedObject.FindProperty("MatchResolutionToWidth");
			AppIsPortrait = serializedObject.FindProperty("AppIsPortrait");
			InitializeViewOnAwake = serializedObject.FindProperty("InitialViewOnAwake");
			UiLifecycleMethod = serializedObject.FindProperty("UiLifeType");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			if(!MvcWorkspace.HasValidWorkspace()) {
				UninitializedView.Render(this);
			}
			else if(!MvcResources.IsValid()) {
				ResourceErrorView.Render(this);
			}
			else {
				InitializedView.Render(this);
			}

			serializedObject.ApplyModifiedProperties();
		}


	}
}
#endif