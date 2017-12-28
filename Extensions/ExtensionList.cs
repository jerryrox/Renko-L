using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

namespace Renko.Extensions
{
	public static class ExtensionList {

		/// <summary>
		/// Adds elements from the given list.
		/// </summary>
		public static void AddList<T>(this List<T> context, List<T> source) {
			for(int i=0; i<source.Count; i++)
				context.Add(source[i]);
		}

		/// <summary>
		/// Gets the last index of this list.
		/// </summary>
		public static int GetLastIndex<T>(this List<T> context) {
			return context.Count - 1;
		}

		/// <summary>
		/// Gets the last item of this list.
		/// </summary>
		public static T GetLast<T>(this List<T> context) {
			return context[ context.Count - 1 ];
		}

		/// <summary>
		/// Gets a random item from this list.
		/// </summary>
		public static T GetRandom<T>(this List<T> context) {
			return context[ Random.Range(0, context.Count) ];
		}

		/// <summary>
		/// Gets a random index from this list.
		/// </summary>
		public static int GetRandomIndex<T>(this List<T> context) {
			return Random.Range(0, context.Count);
		}

		/// <summary>
		/// Returns the first object that matches the given condition.
		/// If null, a default value will be returned.
		/// </summary>
		public static T FindFirst<T>(this List<T> context, Func<T, bool> condition) {
			for(int i=0; i<context.Count; i++) {
				if(condition(context[i]))
					return context[i];
			}
			return default(T);
		}

		/// <summary>
		/// Returns the last object that matches the given condition.
		/// If null, a default value will be returned.
		/// </summary>
		public static T FindLast<T>(this List<T> context, Func<T, bool> condition) {
			for(int i=context.Count-1; i>=0; i--) {
				if(condition(context[i]))
					return context[i];
			}
			return default(T);
		}

		/// <summary>
		/// Randomizes the item order of this list.
		/// </summary>
		public static void Randomize<T>(this List<T> context) {
			for(int i=0; i<context.Count; i++) {
				T val = context[i];
				int rInx = Random.Range(0, context.Count);
				context[i] = context[rInx];
				context[rInx] = val;
			}
		}
	}
}

