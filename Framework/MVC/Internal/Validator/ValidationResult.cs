#if NGUI
using System;

namespace Renko.MVCFramework
{
	public enum ValidationResult {
		
		Success = 0,

		TypeDoesntExist,
		TypeDoesntImplementViewInterface,

		ViewNameConflicts,
		ViewNameInvalid,
	}
}
#endif