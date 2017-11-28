using System.Collections;
using System.Text;
using System;

namespace Renko.Extensions
{
	public static class ExtensionString {

		/// <summary>
		/// Returns a random index position of this string.
		/// </summary>
		public static int GetRandomIndex(this string context) {
			return UnityEngine.Random.Range(0, context.Length);
		}

		/// <summary>
		/// Returns a random character from this string.
		/// </summary>
		public static char GetRandomChar(this string context) {
			return context[UnityEngine.Random.Range(0, context.Length)];
		}

		/// <summary>
		/// Splits this string with the separator string.
		/// </summary>
		public static string[] Split(this string context, string separator) {
			return context.Split(new string[]{separator}, StringSplitOptions.None);
		}

		/// <summary>
		/// Splits this string with the separator string and split options.
		/// </summary>
		public static string[] Split(this string context, string separator, StringSplitOptions option) {
			return context.Split(new string[]{separator}, option);
		}

		/// <summary>
		/// Returns the first index of string s from this string.
		/// Returns -1 if no occurrence.
		/// </summary>
		public static int FirstIndexOf(this string context, string s) {
			int targetLength = s.Length;
			int loopCount = context.Length - targetLength;
			if(loopCount < 0)
				return -1;
			for(int i=0; i<=loopCount; i++) {
				bool success = true;
				for(int c=0; c<targetLength; c++) {
					if(context[i+c] != s[c]) {
						success = false;
						break;
					}
				}
				if(success)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Returns the first index of character c from this string.
		/// </summary>
		public static int FirstIndexOf(this string context, char c) {
			int length = context.Length;
			for(int i=0; i<length; i++) {
				if(context[i].Equals( c ))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Returns the n'th index of character c from this string.
		/// </summary>
		public static int NthIndexOf(this string context, int n, char c) {
			int length = context.Length;
			for(int i=0; i<length; i++) {
				if(context[i].Equals( c )) {
					n--;
					if(n <= 0)
						return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Returns the n'th index of character c from this string in backwards.
		/// </summary>
		public static int NthIndexFromLast(this string context, int n, char c) {
			int length = context.Length;
			for(int i=length-1; i>=0; i--) {
				if(context[i].Equals( c )) {
					n--;
					if(n <= 0)
						return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Returns the number of character c included in this string.
		/// </summary>
		public static int EntryCountOf(this string context, char c) {
			int count = 0;
			for(int i=0; i<context.Length; i++) {
				if(context[i].Equals(c))
					count++;
			}
			return count;
		}

		/// <summary>
		/// Returns the last character from this string.
		/// </summary>
		public static char LastCharacter(this string context) {
			return context[ context.Length-1 ];
		}

		/// <summary>
		/// Returns whether this string matches the target string, regardless of case-sensitivity.
		/// </summary>
		public static bool EqualsIgnoreCase(this string context, string target) {
			return context.ToLower().Equals( target.ToLower() );
		}

		/// <summary>
		/// Returns whether this string contains the target string, regardless of case-sensitivity.
		/// </summary>
		public static bool ContainsIgnoreCase(this string context, string target) {
			return context.ToLower().Contains( target.ToLower() );
		}

		/// <summary>
		/// Returns a base64 representation of this string.
		/// </summary>
		public static string ToBase64String(this string context, Encoding encoding) {
			return encoding.GetBytes(context).ToBase64String();
		}

		/// <summary>
		/// Returns a decoded string from this base64 string.
		/// </summary>
		public static string FromBase64String(this string context, Encoding encoding) {
			return encoding.GetString(Convert.FromBase64String(context));
		}
	}
}

