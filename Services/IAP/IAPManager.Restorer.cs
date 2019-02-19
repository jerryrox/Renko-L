using System;
using UnityEngine.Purchasing;
using Renko.Utility;
using Renko.LapseFramework;

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
			private ITimerDelay timer;


			public Restorer(Action onComplete) {
				restoreCompleted = onComplete;
				timer = Timer.CreateDelay(OnTimerFinished, RestorePurchaseDuration);
			}

			/// <summary>
			/// Starts the the restoration process.
			/// </summary>
			public void Start() {
				var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
				timer.Start();
				apple.RestoreTransactions((bool result) => {
					IAP.LogMessage("IAPManager.Restorer.RestorePurchases - Item result: " + result);

					if(timer.IsValid)
					{
						timer.Stop();
						timer.Start();
					}
				});
			}

			/// <summary>
			/// OnFinished callback for the timer.
			/// Having this method called means the restoration is finished.
			/// </summary>
			void OnTimerFinished() {
				if(restoreCompleted != null)
					restoreCompleted();
			}
		}
	}
}

