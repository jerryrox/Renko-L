using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Store;

using Renko.Debug;

namespace Renko.Services
{
	public class IAPProduct {

		/// <summary>
		/// The generic id for referring to all store-specific product ids.
		/// </summary>
		public string productID;

		/// <summary>
		/// The type of product.
		/// </summary>
		public ProductType productType;

		/// <summary>
		/// The object which holds all store-specific product ids linked to this product.
		/// </summary>
		public IDs storeIDs;


		#region Properties
		/// <summary>
		/// Non-consume product and Subscription only.
		/// Returns true if this product is purchased and is valid.
		/// </summary>
		public bool IsPurchased
		{
			get
			{
				//Get product
				Product product = IAPManager.FindRawProduct(productID, false);
				//If somehow null
				if(product == null)
				{
					//Return false
					IAPManager.LogMessage("IAPProduct.IsPurchased - Product with ID("+productID+") was not found.");
					return false;
				}
				//Having a receipt means it's purchased.
				return product.hasReceipt;
			}
		}

		/// <summary>
		/// Unity's internal Product object which is wrapped by this class.
		/// </summary>
		public Product RawProduct { get { return IAPManager.FindRawProduct(productID, false); } }
		#endregion


		/// <summary>
		/// Just for hiding the constructor
		/// </summary>
		private IAPProduct() { }

		/// <summary>
		/// Adds a store-specific product id to link with this object.
		/// </summary>
		public IAPProduct AddStoreID(string storeProductId, params AppStore[] stores)
		{
			//If storeIDs is null, create new
			if(storeIDs == null)
				storeIDs = new IDs();

			//Add data to store ids object
			storeIDs.Add(storeProductId, stores);

			//Return this IAPProduct
			return this;
		}

		#region Static
		/// <summary>
		/// Helper method which creates a new IAPProduct object with given parameters.
		/// </summary>
		public static IAPProduct Create(string @productID, ProductType @productType, IDs @storeIDs = null)
		{
			//Create new product
			IAPProduct product = new IAPProduct();

			//Store values
			product.productID = @productID;
			product.productType = @productType;
			product.storeIDs = @storeIDs;

			//Return product
			return product;
		}
		#endregion


		#region Classes
		public class Builder {

			/// <summary>
			/// The list of products to output.
			/// </summary>
			public List<IAPProduct> Products { get; private set; }


			/// <summary>
			/// Initializes a new instance of the <see cref="Renko.Services.IAPProduct+Builder"/> class.
			/// </summary>
			public Builder() { Products = new List<IAPProduct>(); }


			/// <summary>
			/// Adds a new IAPProduct object with specified id, type, and store ids.
			/// </summary>
			public IAPProduct Add(string productID, ProductType productType, IDs storeIDs = null)
			{
				//Create a new product
				var product = IAPProduct.Create(productID, productType, storeIDs);

				//Add the product to list
				Products.Add(product);

				//Return product
				return product;
			}
		}
		#endregion
	}
}