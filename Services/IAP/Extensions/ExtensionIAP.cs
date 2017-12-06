using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Store;

namespace Renko.Services
{
	/// <summary>
	/// A static class for extending new functionalities related to Unity IAP service.
	/// </summary>
	public static class ExtensionIAP {

		/// <summary>
		/// Adds product (type of IAPProduct) to the ConfigurationBuilder object.
		/// </summary>
		public static ConfigurationBuilder AddProduct(this ConfigurationBuilder context, IAPProduct product)
		{
			//If product's storeIDs object is null
			if(product.StoreIDs == null)
			{
				return context.AddProduct(
					product.ProductID,
					product.ProductType
				);
			}

			//If not null, include it
			return context.AddProduct(
				product.ProductID,
				product.ProductType,
				product.StoreIDs
			);
		}
	}
}