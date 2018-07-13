#if NGUI
using UnityEngine;
using System;

namespace Renko.Extensions
{
	public static class NGUIExtensionVector2 {
		
		/// <summary>
		/// Converts the given world position to NGUI's screen position.
		/// Assumes the NGUI is positioned at Vector2.zero.
		/// Multiply the returned value with UIRoot resolution sizes to get local coordinates.
		/// </summary>
		public static Vector2 WorldToScreenNGUI(this Vector2 context) {
			return new Vector2(
				context.x * 0.5f,
				context.y * 0.5f
			);
		}
	}
}
#endif