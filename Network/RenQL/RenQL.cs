using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Data;
using Renko.Network.Internal;

namespace Renko.Network
{
	/// <summary>
	/// GraphQL client for Unity.
	/// </summary>
	public partial class RenQL : MonoBehaviour {

		private static RenQL I;

		private List<QLRequest> requests;


		/// <summary>
		/// Delegate for handling GraphQL response.
		/// </summary>
		public delegate void ResponseHandler(IQLRequest request, IQLResponse response);


		void Awake()
		{
			requests = new List<QLRequest>(4);
		}

		/// <summary>
		/// Initializes RenQL module.
		/// </summary>
		public static void Initialize()
		{
			I = RenkoLibrary.CreateModule<RenQL>();
		}

		/// <summary>
		/// Registers specified request object in the update queue.
		/// </summary>
		public void RegisterUpdate(QLRequest request)
		{
			if(requests.Contains(request))
				return;

			// Add request and enable update.
			requests.Add(request);
			enabled = true;
		}

		/// <summary>
		/// Unregisters specified request object from the update queue.
		/// </summary>
		public void UnregisterUpdate(QLRequest request)
		{
			// Remove request
			requests.Remove(request);
			// Disable update if no more requests remaining.
			if(requests.Count == 0)
				enabled = false;
		}

		void Update()
		{
			for(int i=requests.Count-1; i>=0; i--) {
				QLRequest req = requests[i];
				req.Update();
			}
		}
	}
}