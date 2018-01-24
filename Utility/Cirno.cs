using System;

namespace Renko.Utility
{
	/// <summary>
	/// A math helper class that provides methods not included in Unity's Mathf class.
	/// </summary>
	public static class Cirno {
		
		/// <summary>
		/// Returns the average value of specified values.
		/// </summary>
		public static float Average(params float[] values) {
			float val = 0f;
			for(int i=0; i<values.Length; i++)
				val += values[i];
			return val / values.Length;
		}

		/// <summary>
		/// Returns the InverseLerp value without clamping.
		/// </summary>
		public static float InverseLerpUnclamped(float a, float b, float value) {
			if(a == b)
				return 0f;
			return (value - a) / (b - a);
		}
	}
}

