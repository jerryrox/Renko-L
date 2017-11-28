using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Renko.Utility
{
	/// <summary>
	/// A class that removes names from specified gameobjects and their children.
	/// </summary>
	public class NameRemover {

		/// <summary>
		/// List for 
		/// </summary>
		private static List<Transform> children;


		/// <summary>
		/// Removes name for the specified object and the children, if "recursive" is true.
		/// </summary>
		public static void Remove(GameObject obj, bool recursive)
		{
			//If recursive
			if(recursive)
			{
				//Handle process through enumerator
				var children = NextDeepChild(obj.transform);

				//While there is a children
				while(children.MoveNext())
				{
					//Remove current object's name
					children.Current.name = null;
				}
			}
			//If single
			else
			{
				//Just nullify obj's name
				obj.name = null;
			}
		}

		/// <summary>
		/// Returns the next child of specified transform.
		/// </summary>
		private static IEnumerator<GameObject> NextDeepChild(Transform transform)
		{
			//For each child
			for(int i=0; i<transform.childCount; i++)
			{
				//Current child
				Transform curChild = transform.GetChild(i);

				//Setup another enumerator for the deep child
				var deepEnumerator = NextDeepChild(curChild);

				//Keep enumerating
				while(deepEnumerator.MoveNext())
					yield return deepEnumerator.Current;
			}

			//Return itself
			yield return transform.gameObject;
		}
	}
}