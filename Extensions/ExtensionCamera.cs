using UnityEngine;

namespace Renko.Extensions
{
	public static class ExtensionCamera {
		
		/// <summary>
		/// Returns a point within scale width and height using screen position.
		/// So, the output X = (-scaleWidth/2 ~ scaleWidth/2) and Y = (-scaleHeight/2 ~ scaleHeight/2)
		/// </summary>
		public static Vector3 ScreenToScaledPoint(this Camera context, Vector3 screenPosition, float scaleWidth, float scaleHeight){
			screenPosition = context.ScreenToViewportPoint(screenPosition);
			screenPosition.x = (screenPosition.x - 0.5f) * scaleWidth;
			screenPosition.y = (screenPosition.y - 0.5f) * scaleHeight;
			return screenPosition;
		}
	}
}

