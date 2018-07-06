using System;
using Renko;
using Renko.MVCFramework;
using UnityEngine;
using UnityEditor;

namespace RenkoEditor
{
	public static class EditorMenuBar {

		[MenuItem("Renko-L/Create/MVC")]
		public static void CreateMVC() {
			var mvc = GameObject.FindObjectOfType<MVC>();
			if(mvc == null)
				mvc = new GameObject("MVC").AddComponent<MVC>();
			Selection.activeGameObject = mvc.gameObject;
		}

		[MenuItem("Renko-L/Help")]
		public static void OpenGithubPage() {
			Application.OpenURL(RenkoLibrary.GithubLink);
		}
	}
}

