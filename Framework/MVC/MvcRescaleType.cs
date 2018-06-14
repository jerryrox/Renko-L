#if NGUI
using System;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Defines types of MVC view rescale mode.
	/// </summary>
	public enum MvcRescaleType {

		/// <summary>
		/// Constant width, rescaling height.
		/// </summary>
		MatchToWidth = 0,

		/// <summary>
		/// Constant height, rescaling width.
		/// </summary>
		MatchToHeight = 1,

		/// <summary>
		/// Rescaling either side to guarantee that no sides will shrink on any screen resolutions.
		/// </summary>
		MatchToBoth = 2,
	}
}
#endif