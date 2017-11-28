using System;

using Random = UnityEngine.Random;

namespace Renko.Extensions
{
	public static class ExtensionArray {
		
		/// <summary>
		/// Returns whether an item exists with the given selector.
		/// </summary>
		public static bool Exists<T>(this T[] context, Func<T, bool> selector) {
			for(int i=0; i<context.Length; i++) {
				if(selector(context[i]))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Randomizes the item order of this array.
		/// </summary>
		public static void Randomize<T>(this T[] context) {
			for(int i=0; i<context.Length; i++) {
				T val = context[i];
				int rInx = context.GetRandomIndex();
				context[i] = context[rInx];
				context[rInx] = val;
			}
		}

		/// <summary>
		/// Returns the index of given item.
		/// If doesn't exist, -1 is returned.
		/// </summary>
		public static int IndexOf<T>(this T[] context, T item) {
			for(int i=0; i<context.Length; i++) {
				if(context[i].Equals(item))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets a random item from this array.
		/// </summary>
		public static T GetRandom<T>(this T[] context) {
			return context[ Random.Range(0, context.Length) ];
		}

		/// <summary>
		/// Returns a random index from this array.
		/// </summary>
		public static int GetRandomIndex<T>(this T[] context) {
			return Random.Range(0, context.Length);
		}

		/// <summary>
		/// Returns the last object of this array.
		/// </summary>
		public static T GetLast<T>(this T[] context) {
			return context[ context.Length-1 ];
		}

		/// <summary>
		/// Returns whether this array contains the given item.
		/// </summary>
		public static bool Contains<T>(this T[] context, T item) {
			for(int i=0; i<context.Length; i++) {
				if(context[i].Equals(item))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns the first object that matches the given condition.
		/// If doesn't exist, a default value will be returned.
		/// </summary>
		public static T FindFirst<T>(this T[] context, Func<T, bool> condition) {
			for(int i=0; i<context.Length; i++) {
				if(condition(context[i]))
					return context[i];
			}
			return default(T);
		}

		/// <summary>
		/// Returns the last object that matches the given condition.
		/// If doesn't exist, a default value will be returned.
		/// </summary>
		public static T FindLast<T>(this T[] context, Func<T, bool> condition) {
			for(int i=context.Length-1; i>=0; i--) {
				if(condition(context[i]))
					return context[i];
			}
			return default(T);
		}
	}
}

