using System.Collections;
using UnityEngine;
using Renko.Debug;

namespace Renko.Utility
{
	/// <summary>
	/// Just a convenience class for converting hexadecimal value (byte or string) to UnityEngine.Color type.
	/// </summary>
	public static class HexColor {

		private const float ByteReciprocal = 1f / 255f;


		/// <summary>
		/// Creates a new Color with byte (0~255) values.
		/// </summary>
		public static Color Create(byte r = 0xff, byte g = 0xff, byte b = 0xff, byte a = 0xff) {
			return new Color(
				r * ByteReciprocal,
				g * ByteReciprocal,
				b * ByteReciprocal,
				a * ByteReciprocal
			);
		}

		/// <summary>
		/// Creates a new Color with string values.
		/// Supported formats:
		/// #{rr}{gg}{bb}{aa}
		/// #{rr}{gg}{bb}
		/// ** Cases are non-sensitive.
		/// ** Hashtag (#) not required.
		/// </summary>
		public static Color Create(string str) {
			int r = 255, g = 255, b = 255, a = 255;

			if(str[0] == '#')
				str = str.Substring(1);

			switch(str.Length)
			{
			case 3:
				{
					r = ParseHex(str[0]) * 16 + ParseHex(str[0]);
					g = ParseHex(str[1]) * 16 + ParseHex(str[1]);
					b = ParseHex(str[2]) * 16 + ParseHex(str[2]);
					break;
				}
			case 4:
				{
					r = ParseHex(str[0]) * 16 + ParseHex(str[0]);
					g = ParseHex(str[1]) * 16 + ParseHex(str[1]);
					b = ParseHex(str[2]) * 16 + ParseHex(str[2]);
					a = ParseHex(str[3]) * 16 + ParseHex(str[3]);
					break;
				}
			case 6:
				{
					r = ParseHex(str[0]) * 16 + ParseHex(str[1]);
					g = ParseHex(str[2]) * 16 + ParseHex(str[3]);
					b = ParseHex(str[4]) * 16 + ParseHex(str[5]);
					break;
				}

			case 8:
				{
					r = ParseHex(str[0]) * 16 + ParseHex(str[1]);
					g = ParseHex(str[2]) * 16 + ParseHex(str[3]);
					b = ParseHex(str[4]) * 16 + ParseHex(str[5]);
					a = ParseHex(str[6]) * 16 + ParseHex(str[7]);
					break;
				}
			default:
				RenLog.Log(LogLevel.Warning, "HexColor.Create - Invalid string given: " + str);
				break;
			}

			return new Color(
				r * ByteReciprocal,
				g * ByteReciprocal,
				b * ByteReciprocal,
				a * ByteReciprocal
			);
		}

		private static int ParseHex(char ch) {
			if(ch < 58)
				return ch - 48;
			else if(ch < 71)
				return ch - 55; //+10'd
			else if(ch < 103)
				return ch - 87; //+10'd
			return 0;
		}
	}

	public static class HexColorExtensions {

		/// <summary>
		/// Converts this color to a hexadecimal format string.
		/// </summary>
		public static string ToHexString(this Color color) {
			byte r = (byte)(color.r * 255f);
			byte g = (byte)(color.g * 255f);
			byte b = (byte)(color.b * 255f);
			byte a = (byte)(color.a * 255f);

			return string.Format(
				"{0:X2}{1:X2}{2:X2}{3:X2}",
				r,
				g,
				b,
				a
			);
		}
	}
}