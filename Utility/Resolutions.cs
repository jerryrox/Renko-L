using UnityEngine;
using System.Collections;

namespace Renko.Utility
{
	/// <summary>
	/// Class used for setting a virtual screen size which scales to fit on the physical screen resolution.
	/// </summary>
	public static class Resolutions {

		/// <summary>
		/// The current (preferred) width of the screen.
		/// </summary>
		public static float Width;

		/// <summary>
		/// The current (preferred) height of the screen.
		/// </summary>
		public static float Height;

		/// <summary>
		/// The screen ratio (Matching side) / (Non-matching side)
		/// </summary>
		public static float Ratio;

		/// <summary>
		/// The base screen ratio you prefer (Matching side) / (Non-matching side)
		/// </summary>
		public static float PreferredRatio;

		/// <summary>
		/// The scale of preferred ratio to current Ratio.
		/// </summary>
		public static float ScaleFactor;


		/// <summary>
		/// Initializes virtual screen resolution.
		/// </summary>
		public static void Initialize(bool matchToWidth, float baseSizeOfMatchingSide, float preferredScreenRatio) {
			PreferredRatio = preferredScreenRatio;

			if(matchToWidth) {
				Width = baseSizeOfMatchingSide;
				Height = Width * (float)Screen.height / (float)Screen.width;

				Ratio = Width / Height;
				ScaleFactor = preferredScreenRatio / Ratio;
			}
			else {
				Height = baseSizeOfMatchingSide;
				Width = Height * (float)Screen.width / (float)Screen.height;
				
				Ratio = Height / Width;
				ScaleFactor = preferredScreenRatio / Ratio;
			}
		}

		/// <summary>
		/// Returns a 2D position in virtual space which is positioned (xOffset, yOffset) away from given "side".
		/// </summary>
		public static Vector2 AnchoredPosition(Side side, float xOffset, float yOffset) {
			Vector2 pos = new Vector2();
			float width = Width;
			float height = Height;

			switch(side) {
			case Side.BottomLeft:
				pos.x = width*-0.5f + xOffset;
				pos.y = height*-0.5f + yOffset;
				break;
			case Side.Left:
				pos.x = width*-0.5f + xOffset;
				pos.y = yOffset;
				break;
			case Side.TopLeft:
				pos.x = width*-0.5f + xOffset;
				pos.y = height*0.5f + yOffset;
				break;
				
			case Side.BottomRight:
				pos.x = width*0.5f + xOffset;
				pos.y = height*-0.5f + yOffset;
				break;
			case Side.Right:
				pos.x = width*0.5f + xOffset;
				pos.y = yOffset;
				break;
			case Side.TopRight:
				pos.x = width*0.5f + xOffset;
				pos.y = height*0.5f + yOffset;
				break;
				
			case Side.Bottom:
				pos.x = xOffset;
				pos.y = height*-0.5f + yOffset;
				break;
			case Side.Center:
				pos.x = xOffset;
				pos.y = yOffset;
				break;
			case Side.Top:
				pos.x = xOffset;
				pos.y = height*0.5f + yOffset;
				break;
			}
			return pos;
		}

		/// <summary>
		/// Moves the given transform to position calaulated from AnchoredPosition method.
		/// </summary>
		public static void ApplyAnchor(Transform tm, Side side, float xOffset, float yOffset) {
			Vector3 targetPos = AnchoredPosition(side, xOffset, yOffset);
			targetPos.z = tm.localPosition.z;
			tm.localPosition = targetPos;
		}

		#if NGUI
		/// <summary>
		/// Converts the given world position to NGUI's local position.
		/// </summary>
		public static Vector2 WorldToLocalNGUI(Vector2 world) {
			return new Vector2(
				world.x * 0.5f * Height,
				world.y * 0.5f * Height
			);
		}
		#endif

		#region Enums
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
		#endregion
	}
}