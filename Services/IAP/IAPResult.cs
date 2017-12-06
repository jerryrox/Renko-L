using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Renko.Services
{
	/// <summary>
	/// Class which holds information related to IAP success.
	/// </summary>
	public class IAPSuccess {

		/// <summary>
		/// Product which is purchased by the user.
		/// </summary>
		public Product product {
			get; private set;
		}


		/// <summary>
		/// Default constructor for success info.
		/// </summary>
		public IAPSuccess(Product @product) {
			//Store info
			product = @product;
		}

		/// <summary>
		/// Implicit boolean operator for short-typing null checks.
		/// </summary>
		public static implicit operator bool(IAPSuccess success) {
			return success != null;
		}
	}

	/// <summary>
	/// Class which holds information related to IAP failure.
	/// </summary>
	public class IAPFail {

		/// <summary>
		/// Reason for failure.
		/// </summary>
		public Reason reason {
			get; private set;
		}


		/// <summary>
		/// Default constructor for fail info
		/// </summary>
		public IAPFail(Reason _reason) {
			reason = _reason;
		}

		/// <summary>
		/// Overloaded constructor for new fail info using PurchaseFailureReason enum.
		/// </summary>
		public IAPFail(PurchaseFailureReason _reason) {
			SetReasonFromPurchaseFailureReason(_reason);
		}

		/// <summary>
		/// Convenience method for mapping PurchaseFailureReason to IAPFail.Reason enum.
		/// </summary>
		public void SetReasonFromPurchaseFailureReason(PurchaseFailureReason failReason) {
			switch(failReason) {
			case PurchaseFailureReason.PurchasingUnavailable:
				reason = Reason.PurchaseNotAvailable;
				break;
			case PurchaseFailureReason.ExistingPurchasePending:
				reason = Reason.ExistingPurchasePending;
				break;
			case PurchaseFailureReason.ProductUnavailable:
				reason = Reason.ProductNotAvailable;
				break;
			case PurchaseFailureReason.SignatureInvalid:
				reason = Reason.SignatureInvalid;
				break;
			case PurchaseFailureReason.UserCancelled:
				reason = Reason.UserCancel;
				break;
			case PurchaseFailureReason.PaymentDeclined:
				reason = Reason.PaymentDeclined;
				break;
			case PurchaseFailureReason.DuplicateTransaction:
				reason = Reason.DuplicateTransaction;
				break;
			case PurchaseFailureReason.Unknown:
				reason = Reason.Unknown;
				break;
			}
		}

		/// <summary>
		/// Implicit boolean operator for short-typing null checks.
		/// </summary>
		public static implicit operator bool(IAPFail fail) {
			return fail != null;
		}


		/// <summary>
		/// Defines the type of failures.
		/// </summary>
		public enum Reason {
			NotInitialized = 0,
			PlatformNotSupported,
			ProductNotFound,
			ProductNotAvailable,
			PurchaseNotAvailable,
			ExistingPurchasePending,
			SignatureInvalid,
			PaymentDeclined,
			DuplicateTransaction,
			UserCancel,
			AppNotKnown,
			Unknown
		}
	}
}