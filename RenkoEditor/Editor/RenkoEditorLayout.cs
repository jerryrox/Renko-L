using UnityEngine;
using UnityEditor;

using Type = System.Type;

namespace RenkoEditor
{
	/// <summary>
	/// Editor GUI layout helper class.
	/// </summary>
	public static class RenkoEditorLayout {


		/// <summary>
		/// Makes a field to receive scene objects.
		/// </summary>
		public static Object SceneObjectField(Object obj, Type type) {
			return SceneObjectField(null, obj, type);
		}

		/// <summary>
		/// Makes a field to receive scene objects.
		/// </summary>
		public static Object SceneObjectField(string label, Object obj, Type type) {
			Object newObj = EditorGUILayout.ObjectField(
				label, obj, type, true
			);

			if(newObj != null && !EditorUtility.IsPersistent(newObj))
				return newObj;
			return obj;
		}
	}
}

