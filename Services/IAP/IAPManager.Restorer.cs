using System;
using UnityEngine.Purchasing;
using Renko.Utility;

namespace Renko.Services
{
	public partial class IAPManager {

		/// <summary>
		/// An encapsulated class for handling IAP restoration process.
		/// </summary>
		private class Restorer {

			/// <summary>
			/// The maximum amount of time (in seconds) allowed between restorations before the restoring process is considered finished.
			/// </summary>
			private const float RestorePurchaseDuration = 2.5f;

			/// <summary>
			/// Callback action to invoke when restore is completed.
			/// </summary>
			private Action restoreCompleted;

			/// <summary>
			/// Timer for detecting restoration end.
			/// </summary>
			private Timer.Item timer;


			public Restorer(Action onComplete) {
				restoreCompleted = onComplete;
				timer = Timer.CreateDelay(RestorePurchaseDuration, 0, OnTimerFinished);
			}

			/// <summary>
			/// Starts the the restoration process.
			/// </summary>
			public void Start() {
				var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
				apple.RestoreTransactions((bool result) => {
					IAP.LogMessage("IAPManager.Restorer.RestorePurchases - Item result: " + result);
					timer.Progress = 0f;
				});
			}

			/// <summary>
			/// OnFinished callback for the timer.
			/// Having this method called means the restoration is finished.
			/// </summary>
			void OnTimerFinished(Timer.Item timer) {
				if(restoreCompleted != null)
					restoreCompleted();
			}
		}
	}
}

