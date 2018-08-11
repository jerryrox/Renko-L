using System;
using UnityEngine;
using Renko.Network.Internal;

namespace Renko.Network
{
	public partial class RenQL : MonoBehaviour {
		
		/// <summary>
		/// Creates and returns a GraphQL request.
		/// </summary>
		public static IQLRequest Request(string url, string query)
		{
			return Request(url, query, null);
		}

		/// <summary>
		/// Creates and returns a GraphQL request.
		/// </summary>
		public static IQLRequest Request(string url, string query, object parameters)
		{
			QLRequest request = new QLRequest(I, url, query, parameters);
			return request;
		}
	}
}

