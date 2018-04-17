#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
// #define RECEIPT_VALIDATION
#endif

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Purchasing;

using Renko.Diagnostics;

namespace Renko.Services
{
	/// <summary>
	/// A class for handling in-app-purchase processes.
	/// </summary>
	public partial class IAPManager {
		
		/// <summary>
		/// The instance of IAP Manager for internal use.
		/// </summary>
		private static IAPManager I;

		/// <summary>
		/// Unity's store controller interface.
		/// </summary>
		private static IStoreController storeController;

		/// <summary>
		/// Store-specific purchase subsystem.
		/// </summary>
		private static IExtensionProvider storeExtensionProvider;

		/// <summary>
		/// Callback to invoke when initialization is finished.
		/// </summary>
		private static IAP.OnInitializedHandler onInitComplete;

		/// <summary>
		/// Callback to invoke when a purchase is finished (whether successful or not).
		/// </summary>
		private static IAP.OnPurchasedHandler onPurchaseComplete;

		/// <summary>
		/// Callback to invoke when restoring products is complete
		/// </summary>
		private static IAP.OnRestoredHandler onRestoreComplete;

		/// <summary>
		/// A nested object inside IAPManager for IStoreListener implementation.
		/// </summary>
		private static Listener StoreListener;

		/// <summary>
		/// An object that handles product restoration process.
		/// </summary>
		private static Restorer ProductRestorer;


		/// <summary>
		/// Returns whether IAP manager is initialized and is ready for use.
		/// </summary>
		public static bool IsInitialized {
			get { return storeController != null && storeExtensionProvider != null; }
		}


		/// <summary>
		/// Use this method for initialization.
		/// For the products, use IAPProduct.Initialize method to create a new product.
		/// </summary>
		public static void Initialize(IAP.OnInitializedHandler callback, bool isTestMode, List<IAPProduct> products) {
			onInitComplete = callback;
			IAP.IsTestMode = isTestMode;

			SetupInstance();

			if(products == null || products.Count == 0) {
				IAP.LogMessage("IAPManager.Initialize - Product list can't be null or empty!");
				return;
			}

			var builder = ConfigurationBuilder.Instance(
				StandardPurchasingModule.Instance()
			);
			for(int i=0; i<products.Count; i++)
				builder.AddProduct(products[i]);
			
			UnityPurchasing.Initialize(
				StoreListener = new Listener(),
				builder
			);
		}


		/// <summary>
		/// Purchases the product with given id.
		/// You need to specify whether your 'id' is store-specific or generic.
		/// </summary>
		public static void PurchaseProduct(IAP.OnPurchasedHandler callback, string id, bool isStoreSpecific) {
			onPurchaseComplete = callback;

			if(IsInitialized) {
				Product product = (
					isStoreSpecific ?
					storeController.products.WithStoreSpecificID(id) :
					storeController.products.WithID(id)
				);
				if(product == null) {
					DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.ProductNotFound));
					return;
				}
				if(!product.availableToPurchase){
					DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.ProductNotAvailable));
					return;	
				}

				storeController.InitiatePurchase(product);
			}
			else {
				DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.NotInitialized));
			}
		}

		/// <summary>
		/// Restores non-consumable / subscription purchases.
		/// </summary>
		public static void RestorePurchases(IAP.OnRestoredHandler callback) {
			onRestoreComplete = callback;

			if(!IsInitialized) {
				DoOnRestoreComplete(false, new IAPFail(IAPFail.Reason.NotInitialized));
				return;
			}
			if(!IAP.IsRestoreSupported) {
				DoOnRestoreComplete(false, new IAPFail(IAPFail.Reason.PlatformNotSupported));
				return;
			}

			ProductRestorer = new Restorer(() => {
				IAP.LogMessage("IAPManager.OnTimerFinished - Restore completed.");
				DoOnRestoreComplete(true, null);
			});
			ProductRestorer.Start();
		}

		/// <summary>
		/// Returns a Product object from specified id.
		/// </summary>
		public static Product FindRawProduct(string productID, bool isStoreSpecific) {
			if(storeController == null || storeController.products == null)
				return null;
			return (
				isStoreSpecific ?
				storeController.products.WithStoreSpecificID(productID) :
				storeController.products.WithID(productID)
			);
		}

		/// <summary>
		/// Prepares a new instance of IAPManager if it doesn't already exist.
		/// </summary>
		private static void SetupInstance() {
			if(I == null)
				I = new IAPManager();
		}

		/// <summary>
		/// Do the callback for delegate: IAP.OnInitializedHandler
		/// </summary>
		private static void DoOnInitComplete(bool result, IAPFail fail) {
			if(onInitComplete != null)
				onInitComplete(result, fail);
			
			onInitComplete = null;
		}

		/// <summary>
		/// Do the callback for delegate: IAP.OnPurchasedHandler
		/// </summary>
		private static void DoOnIAPComplete(IAPSuccess success, IAPFail fail) {
			if(onPurchaseComplete != null)
				onPurchaseComplete(success, fail);
			
			onPurchaseComplete = null;
		}

		/// <summary>
		/// Do the callback for delegate: IAP.OnRestoredHandler
		/// </summary>
		private static void DoOnRestoreComplete(bool result, IAPFail fail) {
			if(onRestoreComplete != null)
				onRestoreComplete(result, fail);
			
			onRestoreComplete = null;
		}
	}
}