using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Store;

using Renko.Diagnostics;

namespace Renko.Services
{
	public class IAPProduct {

		/// <summary>
		/// The generic id for referring to all store-specific product ids.
		/// </summary>
		public string ProductID {
			get; private set;
		}

		/// <summary>
		/// The type of product.
		/// </summary>
		public ProductType ProductType {
			get; private set;
		}

		/// <summary>
		/// The object which holds all store-specific product ids linked to this product.
		/// </summary>
		public IDs StoreIDs {
			get; private set;
		}


		#region Properties
		/// <summary>
		/// Returns true if this product is purchased and is valid.
		/// Non-consumable product and Subscription only.
		/// </summary>
		public bool IsPurchased {
			get {
				Product product = IAPManager.FindRawProduct(ProductID, false);
				if(product == null) {
					IAP.LogMessage("IAPProduct.IsPurchased - Product with ID("+ProductID+") was not found.");
					return false;
				}
				return product.hasReceipt;
			}
		}

		/// <summary>
		/// Returns internal Product object which is wrapped by this class.
		/// </summary>
		public Product RawProduct {
			get { return IAPManager.FindRawProduct(ProductID, false); }
		}
		#endregion


		/// <summary>
		/// Constructor should be hidden.
		/// </summary>
		private IAPProduct() {}

		/// <summary>
		/// Returns a new IAPProduct instance with specified parameters.
		/// </summary>
		public static IAPProduct Create(string productID, ProductType productType, IDs storeIDs = null) {
			return new IAPProduct() {
				ProductID = productID,
				ProductType = productType,
				StoreIDs = storeIDs
			};
		}

		/// <summary>
		/// Adds a store-specific product id to link with this object.
		/// Returns this instance.
		/// </summary>
		public IAPProduct AddStoreID(string storeProductId, params AppStore[] stores) {
			if(StoreIDs == null)
				StoreIDs = new IDs();
			StoreIDs.Add(storeProductId, stores);
			return this;
		}


		/// <summary>
		/// A factory class that outputs an IAPProduct object.
		/// </summary>
		public class Builder {

			/// <summary>
			/// The list of products to output.
			/// </summary>
			public List<IAPProduct> Products {
				get; private set;
			}


			public Builder() {
				Products = new List<IAPProduct>();
			}

			/// <summary>
			/// Adds a new IAPProduct object with specified id, type, and store ids.
			/// </summary>
			public IAPProduct Add(string productID, ProductType productType, IDs storeIDs = null) {
				var product = IAPProduct.Create(productID, productType, storeIDs);
				Products.Add(product);
				return product;
			}
		}
	}
}