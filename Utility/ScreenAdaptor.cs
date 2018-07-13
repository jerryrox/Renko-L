using System.Collections;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A class that provides useful functions for scalable 2D UI.
	/// </summary>
	public class ScreenAdaptor {
		
		/// <summary>
		/// The base resolution to work on.
		/// </summary>
		public readonly Vector2 BaseResolution;


		/// <summary>
		/// The actual resolution of device screen.
		/// </summary>
		public static Vector2 AbsoluteResolution {
			get { return new Vector2(Screen.width, Screen.height); }
		}

		/// <summary>
		/// The actual ratio of device screen (width / height).
		/// </summary>
		public static float AbsoluteRatio {
			get { return (float)Screen.width / (float)Screen.height; }
		}

		/// <summary>
		/// The screen ratio of BaseResolution (width / height).
		/// </summary>
		public float BaseRatio {
			get { return BaseResolution.x / BaseResolution.y; }
		}


		public ScreenAdaptor(float width, float height) : this(new Vector2(width, height)) {}

		public ScreenAdaptor(Vector2 resolution)
		{
			this.BaseResolution = resolution;
		}

		/// <summary>
		/// Returns a 2D position in virtual space which is positioned (xOffset, yOffset) away from given "side".
		/// </summary>
		public static Vector2 GetAnchoredPosition(Vector2 resolution, Side side, float xOffset, float yOffset) {
			Vector2 pos = resolution;

			switch(side) {
			case Side.BottomLeft:
				pos.x = pos.x*-0.5f + xOffset;
				pos.y = pos.y*-0.5f + yOffset;
				break;
			case Side.Left:
				pos.x = pos.x*-0.5f + xOffset;
				pos.y = yOffset;
				break;
			case Side.TopLeft:
				pos.x = pos.x*-0.5f + xOffset;
				pos.y = pos.y*0.5f - yOffset;
				break;

			case Side.BottomRight:
				pos.x = pos.x*0.5f - xOffset;
				pos.y = pos.y*-0.5f + yOffset;
				break;
			case Side.Right:
				pos.x = pos.x*0.5f - xOffset;
				pos.y = yOffset;
				break;
			case Side.TopRight:
				pos.x = pos.x*0.5f - xOffset;
				pos.y = pos.y*0.5f - yOffset;
				break;

			case Side.Bottom:
				pos.x = xOffset;
				pos.y = pos.y*-0.5f + yOffset;
				break;
			case Side.Center:
				pos.x = xOffset;
				pos.y = yOffset;
				break;
			case Side.Top:
				pos.x = xOffset;
				pos.y = pos.y*0.5f - yOffset;
				break;
			}
			return pos;
		}

		/// <summary>
		/// Moves the specified transform to position calaulated from GetAnchoredPosition method.
		/// </summary>
		public static void ApplyAnchor(Transform tm, Vector2 resolution, Side side, float xOffset, float yOffset) {
			Vector3 targetPos = GetAnchoredPosition(resolution, side, xOffset, yOffset);
			targetPos.z = tm.localPosition.z;
			tm.localPosition = targetPos;
		}

		/// <summary>
		/// Returns the scale factor from BaseResolution to AbsoluteResolution using specified scale mode.
		/// </summary>
		public Vector2 GetScaleFactor(ScaleMode mode)
		{
			switch(mode) {
			case ScaleMode.FitToWidth:
				return new Vector2(
					1f,
					(Screen.width * BaseResolution.y) / (BaseResolution.x * Screen.height)
				);
			case ScaleMode.FitToHeight:
				return new Vector2(
					(Screen.width * BaseResolution.y) / (BaseResolution.x * Screen.height),
					1f
				);
			case ScaleMode.Contain:
				// Fit to height, so width can grow
				if(BaseRatio < AbsoluteRatio) {
					return new Vector2(
						(Screen.width * BaseResolution.y) / (BaseResolution.x * Screen.height),
						1f
					);
				}
				// Fit to width, so height can grow
				else {
					return new Vector2(
						1f,
						(Screen.width * BaseResolution.y) / (BaseResolution.x * Screen.height)
					);
				}
			}
			return Vector2.one;
		}

		/// <summary>
		/// Returns the BaseResolution applied with scale factor calculated using specified scale mode.
		/// </summary>
		public Vector2 GetScaledResolution(ScaleMode mode)
		{
			var scale = GetScaleFactor(mode);
			return new Vector2(
				BaseResolution.x * scale.x,
				BaseResolution.y * scale.y
			);
		}


		/// <summary>
		/// Types of screen side.
		/// </summary>
		public enum Side {
			BottomLeft,
			Left,
			TopLeft,
			Top,
			TopRight,
			Right,
			BottomRight,
			Bottom,
			Center,
		}

		/// <summary>
		/// Types of scaling mode.
		/// </summary>
		public enum ScaleMode {
			/// <summary>
			/// Fits base resolution to width.
			/// Growing / Shrinking height.
			/// </summary>
			FitToWidth = 0,

			/// <summary>
			/// Fits base resolution to height.
			/// Growing / Shrinking width.
			/// </summary>
			FitToHeight = 1,

			/// <summary>
			/// Base resolution is guaranteed to fully display in all situations.
			/// Growing height / width.
			/// </summary>
			Contain = 2
		}
	}
}