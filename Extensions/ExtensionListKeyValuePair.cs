using System;
using System.Collections.Generic;

namespace Renko.Extensions
{
	public static class ExtensionListKeyValuePair {
		
		/// <summary>
		/// Returns whether this list contains a key value pair with given key.
		/// </summary>
		public static bool ContainsPair<K,V>(this List<KeyValuePair<K,V>> context, string key) {
			for(int i=0; i<context.Count; i++) {
				if(context[i].Key.Equals( key ))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns a key value pair from this list using the 'key'.
		/// Throws an exception if doesn't exist.
		/// </summary>
		public static KeyValuePair<K,V> GetPair<K,V>(this List<KeyValuePair<K,V>> context, string key) {
			for(int i=0; i<context.Count; i++) {
				if(context[i].Key.Equals( key ))
					return context[i];
			}
			throw new Exception("ExtensionListKeyValuePair.GetPair - Key was not present in the list.");
		}
	}
}

