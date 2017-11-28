#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
// #define RECEIPT_VALIDATION
#endif

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Store;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

using Renko.Debug;

namespace Renko.Services
{
	/// <summary>
	/// A class for handling in-app-purchase processes.
	/// </summary>
	public class IAPManager : MonoBehaviour, IStoreListener {

		#region Private fields
		/// <summary>
		/// The instance of IAP Manager for internal use.
		/// </summary>
		private static IAPManager _I;

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
		/// The current amount of time in seconds left before restoring purchase is considered finished.
		/// </summary>
		private static float restorePurchaseTimeout = -1;

		/// <summary>
		/// The maximum amount of time in seconds to wait before restoring purchase is considered finished.
		/// </summary>
		private const float RestorePurchaseDuration = 2.5f;
		#endregion


		#region Public properties
		/// <summary>
		/// Returns whether IAP manager is initialized and is ready for use.
		/// </summary>
		public static bool IsInitialized
		{
			get
			{
				//IAP is initialized if both purchasing references are set.
				return storeController != null && storeExtensionProvider != null;
			}
		}
		#endregion


		#region Initialize
		/// <summary>
		/// Use this method for initialization.
		/// For the products, use IAPProduct.Initialize method to create a new product.
		/// </summary>
		public static void Initialize(IAP.OnInitializedHandler callback, bool isTestMode, List<IAPProduct> products)
		{
			//Store callback
			onInitComplete = callback;

			//Store flag
			IAP.IsTestMode = isTestMode;

			//If instance not setup yet
			if(_I == null)
			{
				//Create new gameobject with this component.
				//Store reference too
				_I = new GameObject("_IAPManager").AddComponent<IAPManager>();
			}

			//If product list is empty or null, show warning and return
			if(products == null || products.Count == 0)
			{
				//Debug
				LogMessage("IAPManager.Initialize - Product list is null or empty!");
				//Return
				return;
			}

			//Initialize IAP config builder
			var builder = ConfigurationBuilder.Instance(
				StandardPurchasingModule.Instance()
			);

			//For each product given
			for(int i=0; i<products.Count; i++)
			{
				//Add this product to the builder
				builder.AddProduct(products[i]);
			}

			//Initialize unity purchasing service
			UnityPurchasing.Initialize(_I, builder);
		}
		#endregion

		#region Purchase Product
		/// <summary>
		/// Purchases the product with given id.
		/// You need to specify whether your 'id' is a store-specific product id or a generic one.
		/// </summary>
		public static void PurchaseProduct(IAP.OnPurchasedHandler callback, string id, bool isStoreSpecific)
		{
			//Store callback
			onPurchaseComplete = callback;

			//If purchasing is initialized
			if(IsInitialized)
			{
				//Find the product with matching id
				Product product = (
					isStoreSpecific ?
					storeController.products.WithStoreSpecificID(id) :
					storeController.products.WithID(id)
				);

				//If product is null
				if(product == null)
				{
					//Product not found
					DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.ProductNotFound));
					return;
				}

				//If not available to purchase
				if(!product.availableToPurchase)
				{
					//Product not available
					DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.ProductNotAvailable));
					return;	
				}

				//Initiate purchasing process
				storeController.InitiatePurchase(product);
			}
			else
			{
				//Not initialized
				DoOnIAPComplete(null, new IAPFail(IAPFail.Reason.NotInitialized));
			}
		}
		#endregion

		#region Restore Purchases
		/// <summary>
		/// Restores all purchases made previously.
		/// </summary>
		public static void RestorePurchases(IAP.OnRestoredHandler callback)
		{
			//Store callback
			onRestoreComplete = callback;

			//If not initialized
			if(!IsInitialized)
			{
				//Not initialized
				DoOnRestoreComplete(false, new IAPFail(IAPFail.Reason.NotInitialized));
				//Just return
				return;
			}

			//If not supported
			if(!IAP.IsRestoreSupported)
			{
				//Platform not supported
				DoOnRestoreComplete(false, new IAPFail(IAPFail.Reason.PlatformNotSupported));
				return;
			}

			//Get apple subsystem
			var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();

			//Restore the transactions
			apple.RestoreTransactions((bool result) => {

				//Debug message
				LogMessage("IAPManager.RestorePurchases - Result: " + result);

				//Set timeout time
				restorePurchaseTimeout = RestorePurchaseDuration;
			});
		}

		/// <summary>
		/// Updates restore purcahse timeout time for detecting completion
		/// </summary>
		void Update_RestorePurchases()
		{
			//Store current timeout time
			float timeOut = restorePurchaseTimeout;

			//Decrease timeout time
			restorePurchaseTimeout -= fDeltaTime;

			//If timeout
			if(timeOut >= 0f && restorePurchaseTimeout < 0f)
			{
				//Debug
				LogMessage("IAPManager.Update_RestorePurchases - Restore complete");

				//Do callback
				DoOnRestoreComplete(true, null);
			}
		}
		#endregion

		#region Public helpers
		/// <summary>
		/// Returns a Product object from specified id.
		/// </summary>
		public static Product FindRawProduct(string productID, bool isStoreSpecific)
		{
			//If controller or products is null, return null
			if(storeController == null || storeController.products == null)
				return null;

			//Return product
			return (
				isStoreSpecific ?
				storeController.products.WithStoreSpecificID(productID) :
				storeController.products.WithID(productID)
			);
		}
		#endregion

		#region Callback helpers
		/// <summary>
		/// Do the callback for delegate: IAP.OnInitializedHandler
		/// </summary>
		private static void DoOnInitComplete(bool result, IAPFail fail)
		{
			//Do callback only if not null
			if(onInitComplete != null)
				onInitComplete(result, fail);

			//Remove callback
			onInitComplete = null;
		}

		/// <summary>
		/// Do the callback for delegate: IAP.OnPurchasedHandler
		/// </summary>
		private static void DoOnIAPComplete(IAPSuccess success, IAPFail fail)
		{
			//Do callback only if not null
			if(onPurchaseComplete != null)
				onPurchaseComplete(success, fail);

			//Remove callback
			onPurchaseComplete = null;
		}

		/// <summary>
		/// Do the callback for delegate: IAP.OnRestoredHandler
		/// </summary>
		private static void DoOnRestoreComplete(bool result, IAPFail fail)
		{
			//Do callback only if not null
			if(onRestoreComplete != null)
				onRestoreComplete(result, fail);

			//Remove callback
			onRestoreComplete = null;
		}

		/// <summary>
		/// Method for outputting debug messages.
		/// </summary>
		public static void LogMessage(object message)
		{
			//Only do logging if specified
			if(IAP.DebugMessage)
				RenLog.Log(LogLevel.Info, message);
		}
		#endregion

		#region Update
		private float fDeltaTime;
		void Update()
		{
			//Cache deltatime
			fDeltaTime = Time.deltaTime;

			//Do updates
			Update_RestorePurchases();
		}
		#endregion

		#region IStoreListener events
		/// <summary>
		/// This method is called once the Unity IAP has loaded.
		/// </summary>
		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			//Debug
			LogMessage("IAPManager.OnInitialized");

			//Store parameter values
			storeController = controller;
			storeExtensionProvider = extensions;

			//Do callback
			DoOnInitComplete(true, null);
		}

		/// <summary>
		/// This method is called when the Unity IAP has failed to load.
		/// </summary>
		public void OnInitializeFailed (InitializationFailureReason error)
		{
			//Debug
			LogMessage("IAPManager.OnInitializeFailed - Reason: " + error);

			//Determine reason
			IAPFail.Reason reason = IAPFail.Reason.Unknown;
			if(error == InitializationFailureReason.AppNotKnown)
				reason = IAPFail.Reason.AppNotKnown;
			else if(error == InitializationFailureReason.NoProductsAvailable)
				reason = IAPFail.Reason.ProductNotAvailable;
			else
				reason = IAPFail.Reason.PurchaseNotAvailable;

			//Do callback
			DoOnInitComplete(false, new IAPFail(reason));
		}

		/// <summary>
		/// This method is called when the purchase is completed.
		/// </summary>
		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			//Debug
			LogMessage("IAPManager.ProcessPurchase");

			//Do callback
			DoOnIAPComplete(new IAPSuccess(args.purchasedProduct), null);

			//Return complete
			return PurchaseProcessingResult.Complete;
		}

		/// <summary>
		/// This method is called when the purchase is failed for some reason.
		/// </summary>
		public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
		{
			//Debug
			LogMessage(string.Format(
				"IAPManager.OnPurchaseFailed - Unity Product id: {0}, Store-Specific id: {1}, Failure reason: {2}",
				product.definition.id,
				product.definition.storeSpecificId,
				failureReason
			));

			//Do callback
			DoOnIAPComplete(null, new IAPFail(failureReason));
		}
		#endregion
	}
}