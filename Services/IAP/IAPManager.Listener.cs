using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Renko.Services
{
	public partial class IAPManager {

		/// <summary>
		/// A class that listens to Unity IAP events.
		/// </summary>
		private class Listener : IStoreListener {
			
			public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
				IAP.LogMessage("IAPManager.OnInitialized");

				storeController = controller;
				storeExtensionProvider = extensions;
				DoOnInitComplete(true, null);
			}

			public void OnInitializeFailed (InitializationFailureReason error) {
				IAP.LogMessage("IAPManager.OnInitializeFailed - Reason: " + error);

				IAPFail.Reason reason = GetInitFailReason(error);
				DoOnInitComplete(false, new IAPFail(reason));
			}

			public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
				IAP.LogMessage("IAPManager.ProcessPurchase");

				DoOnIAPComplete(new IAPSuccess(args.purchasedProduct), null);
				return PurchaseProcessingResult.Complete;
			}

			public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason) {
				IAP.LogMessage(string.Format(
					"IAPManager.OnPurchaseFailed - Unity Product id: {0}, Store-Specific id: {1}, Failure reason: {2}",
					product.definition.id,
					product.definition.storeSpecificId,
					failureReason
				));

				DoOnIAPComplete(null, new IAPFail(failureReason));
			}

			IAPFail.Reason GetInitFailReason(InitializationFailureReason error) {
				if(error == InitializationFailureReason.AppNotKnown)
					return IAPFail.Reason.AppNotKnown;
				else if(error == InitializationFailureReason.NoProductsAvailable)
					return IAPFail.Reason.ProductNotAvailable;
				else
					return IAPFail.Reason.PurchaseNotAvailable;
				
				return IAPFail.Reason.Unknown;
			}
		}
	}
}

