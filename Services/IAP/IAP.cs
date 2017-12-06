using UnityEngine;
using Renko.Debug;

namespace Renko.Services
{
	/// <summary>
	/// A static class for defining delegates, flags, etc.
	/// Use IAPManager class for the actual funtions.
	/// </summary>
	public static class IAP
	{
		/// <summary>
		/// Whether the debug message will be displayed.
		/// On production build, this must be set to false.
		/// </summary>
		public static bool DebugMessage = false;

		/// <summary>
		/// Whether IAP service is in test mode or not.
		/// On production build, this must be set to false.
		/// </summary>
		public static bool IsTestMode = false;

		/// <summary>
		/// Returns whether restoring purchases is available for current environment.
		/// </summary>
		public static bool IsRestoreSupported {
			get {
				return Application.platform == RuntimePlatform.IPhonePlayer ||
					Application.platform == RuntimePlatform.OSXPlayer;
			}
		}

		/// <summary>
		/// Delegate for init'd event.
		/// </summary>
		public delegate void OnInitializedHandler(bool result, IAPFail fail);

		/// <summary>
		/// Delegate for purchased event.
		/// </summary>
		public delegate void OnPurchasedHandler(IAPSuccess success, IAPFail fail);

		/// <summary>
		/// Delegate for restored event.
		/// </summary>
		public delegate void OnRestoredHandler(bool result, IAPFail fail);


		/// <summary>
		/// Method for outputting debug messages.
		/// </summary>
		public static void LogMessage(object message) {
			if(IAP.DebugMessage)
				RenLog.Log(LogLevel.Info, message);
		}
	}
}

