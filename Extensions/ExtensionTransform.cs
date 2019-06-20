using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Extensions
{
	public static class ExtensionTransform {
		
		/// <summary>
		/// Sets the x position of this transform.
		/// </summary>
		public static void SetPositionX(this Transform context, float x) {
			Vector3 v3 = context.position;
			v3.x = x;
			context.position = v3;
		}

		/// <summary>
		/// Sets the y position of this transform.
		/// </summary>
		public static void SetPositionY(this Transform context, float y) {
			Vector3 v3 = context.position;
			v3.y = y;
			context.position = v3;
		}

		/// <summary>
		/// Sets the z position of this transform.
		/// </summary>
		public static void SetPositionZ(this Transform context, float z) {
			Vector3 v3 = context.position;
			v3.z = z;
			context.position = v3;
		}
		
		/// <summary>
		/// Sets the xy position of this transform.
		/// </summary>
		public static void SetPositionXY(this Transform context, float x, float y) {
			Vector3 v3 = context.position;
			v3.x = x;
			v3.y = y;
			context.position = v3;
		}

		/// <summary>
		/// Sets the yz position of this transform.
		/// </summary>
		public static void SetPositionYZ(this Transform context, float y, float z) {
			Vector3 v3 = context.position;
			v3.y = y;
			v3.z = z;
			context.position = v3;
		}

		/// <summary>
		/// Sets the xz position of this transform.
		/// </summary>
		public static void SetPositionXZ(this Transform context, float x, float z) {
			Vector3 v3 = context.position;
			v3.x = x;
			v3.z = z;
			context.position = v3;
		}
		
		/// <summary>
		/// Sets the xyz position of this transform.
		/// </summary>
		public static void SetPositionXYZ(this Transform context, float x, float y, float z) {
			context.position = new Vector3(x, y, z);
		}
		
		/// <summary>
		/// Sets the x local position of this transform.
		/// </summary>
		public static void SetLocalPositionX(this Transform context, float x) {
			Vector3 v3 = context.localPosition;
			v3.x = x;
			context.localPosition = v3;
		}

		/// <summary>
		/// Sets the y local position of this transform.
		/// </summary>
		public static void SetLocalPositionY(this Transform context, float y) {
			Vector3 v3 = context.localPosition;
			v3.y = y;
			context.localPosition = v3;
		}
		/// <summary>
		/// Sets the z local position of this transform.
		/// </summary>
		public static void SetLocalPositionZ(this Transform context, float z) {
			Vector3 v3 = context.localPosition;
			v3.z = z;
			context.localPosition = v3;
		}
		
		/// <summary>
		/// Sets the xy local position of this transform.
		/// </summary>
		public static void SetLocalPositionXY(this Transform context, float x, float y) {
			Vector3 v3 = context.localPosition;
			v3.x = x;
			v3.y = y;
			context.localPosition = v3;
		}

		/// <summary>
		/// Sets the yz local position of this transform.
		/// </summary>
		public static void SetLocalPositionYZ(this Transform context, float y, float z) {
			Vector3 v3 = context.localPosition;
			v3.y = y;
			v3.z = z;
			context.localPosition = v3;
		}
		/// <summary>
		/// Sets the xz local position of this transform.
		/// </summary>
		public static void SetLocalPositionXZ(this Transform context, float x, float z) {
			Vector3 v3 = context.localPosition;
			v3.x = x;
			v3.z = z;
			context.localPosition = v3;
		}
		
		/// <summary>
		/// Sets the xyz local position of this transform.
		/// </summary>
		public static void SetLocalPositionXYZ(this Transform context, float x, float y, float z) {
			context.localPosition = new Vector3(x, y, z);
		}
		
		/// <summary>
		/// Sets the x euler angle of this transform.
		/// </summary>
		public static void SetEulerX(this Transform context, float x) {
			Vector3 v3 = context.eulerAngles;
			v3.x = x;
			context.eulerAngles = v3;
		}

		/// <summary>
		/// Sets the y euler angle of this transform.
		/// </summary>
		public static void SetEulerY(this Transform context, float y) {
			Vector3 v3 = context.eulerAngles;
			v3.y = y;
			context.eulerAngles = v3;
		}

		/// <summary>
		/// Sets the z euler angle of this transform.
		/// </summary>
		public static void SetEulerZ(this Transform context, float z) {
			Vector3 v3 = context.eulerAngles;
			v3.z = z;
			context.eulerAngles = v3;
		}
		
		/// <summary>
		/// Sets the xy euler angle of this transform.
		/// </summary>
		public static void SetEulerXY(this Transform context, float x, float y) {
			Vector3 v3 = context.eulerAngles;
			v3.x = x;
			v3.y = y;
			context.eulerAngles = v3;
		}

		/// <summary>
		/// Sets the yz euler angle of this transform.
		/// </summary>
		public static void SetEulerYZ(this Transform context, float y, float z) {
			Vector3 v3 = context.eulerAngles;
			v3.y = y;
			v3.z = z;
			context.eulerAngles = v3;
		}

		/// <summary>
		/// Sets the xz euler angle of this transform.
		/// </summary>
		public static void SetEulerXZ(this Transform context, float x, float z) {
			Vector3 v3 = context.eulerAngles;
			v3.x = x;
			v3.z = z;
			context.eulerAngles = v3;
		}
		
		/// <summary>
		/// Sets the xyz euler angle of this transform.
		/// </summary>
		public static void SetEulerXYZ(this Transform context, float x, float y, float z) {
			context.eulerAngles = new Vector3(x, y, z);
		}
		
		/// <summary>
		/// Sets the x local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerX(this Transform context, float x) {
			Vector3 v3 = context.localEulerAngles;
			v3.x = x;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the y local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerY(this Transform context, float y) {
			Vector3 v3 = context.localEulerAngles;
			v3.y = y;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the z local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerZ(this Transform context, float z) {
			Vector3 v3 = context.localEulerAngles;
			v3.z = z;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the xy local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerXY(this Transform context, float x, float y) {
			Vector3 v3 = context.localEulerAngles;
			v3.x = x;
			v3.y = y;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the yz local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerYZ(this Transform context, float y, float z) {
			Vector3 v3 = context.localEulerAngles;
			v3.y = y;
			v3.z = z;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the xz local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerXZ(this Transform context, float x, float z) {
			Vector3 v3 = context.localEulerAngles;
			v3.x = x;
			v3.z = z;
			context.localEulerAngles = v3;
		}

		/// <summary>
		/// Sets the xyz local euler angle of this transform.
		/// </summary>
		public static void SetLocalEulerXYZ(this Transform context, float x, float y, float z) {
			context.localEulerAngles = new Vector3(x, y, z);
		}
		
		/// <summary>
		/// Sets the x local scale of this transform.
		/// </summary>
		public static void SetLocalScaleX(this Transform context, float x) {
			Vector3 v3 = context.localScale;
			v3.x = x;
			context.localScale = v3;
		}

		/// <summary>
		/// Sets the y local scale of this transform.
		/// </summary>
		public static void SetLocalScaleY(this Transform context, float y) {
			Vector3 v3 = context.localScale;
			v3.y = y;
			context.localScale = v3;
		}

		/// <summary>
		/// Sets the z local scale of this transform.
		/// </summary>
		public static void SetLocalScaleZ(this Transform context, float z) {
			Vector3 v3 = context.localScale;
			v3.z = z;
			context.localScale = v3;
		}

		/// <summary>
		/// Sets the xy local scale of this transform.
		/// </summary>
		public static void SetLocalScaleXY(this Transform context, float x, float y) {
			Vector3 v3 = context.localScale;
			v3.x = x;
			v3.y = y;
			context.localScale = v3;
		}

		/// <summary>
		/// Sets the yz local scale of this transform.
		/// </summary>
		public static void SetLocalScaleYZ(this Transform context, float y, float z) {
			Vector3 v3 = context.localScale;
			v3.y = y;
			v3.z = z;
			context.localScale = v3;
		}

		/// <summary>
		/// Sets the xz local scale of this transform.
		/// </summary>
		public static void SetLocalScaleXZ(this Transform context, float x, float z) {
			Vector3 v3 = context.localScale;
			v3.x = x;
			v3.z = z;
			context.localScale = v3;
		}
		
		public static void SetLocalScaleXYZ(this Transform context, float x, float y, float z) {
			context.localScale = new Vector3(x, y, z);
		}

		/// <summary>
		/// Destroys all child objects in this transform.
		/// </summary>
		public static void DestroyChildren(this Transform context) {
			while(context.childCount > 0)
				GameObject.Destroy(context.GetChild(0));
		}

		/// <summary>
		/// Returns an enumerator for getting this transform's deep children.
		/// </summary>	
		public static IEnumerator<Transform> DeepChildren(this Transform context) {
			for(int i=0; i<context.childCount; i++) {
				Transform curChild = context.GetChild(i);
				var deepEnumerator = DeepChildren(curChild);

				//Keep enumerating
				while(deepEnumerator.MoveNext())
					yield return deepEnumerator.Current;
			}
			yield return context;
		}
	}
}