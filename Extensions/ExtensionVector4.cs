using System;
using UnityEngine;

namespace Renko.Extensions
{
	public static class ExtensionVector4 {

		/// <summary>
		/// Returns a new float array[x, y, z, w] from this vector.
		/// </summary>
		public static float[] ToArray(this Vector4 context) {
			return new float[] {
				context.x,
				context.y,
				context.z,
				context.w
			};
		}
	}
}

