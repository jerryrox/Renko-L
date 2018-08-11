using System;
using UnityEngine;

namespace Renko.Network
{
	/// <summary>
	/// Interface of a GraphQL request object.
	/// </summary>
	public interface IQLRequest {

		/// <summary>
		/// Returns a yield instruction instance for Coroutine yields.
		/// </summary>
		CustomYieldInstruction Yield { get; }

		/// <summary>
		/// Returns whether the request is on-going.
		/// </summary>
		bool IsRequesting { get; }

		/// <summary>
		/// The GraphQL request data object.
		/// </summary>
		QLData Data { get; set; }

		/// <summary>
		/// Returns a new response object from last request.
		/// * Will always create a new IQLResponse.
		/// </summary>
		IQLResponse Response { get; }


		/// <summary>
		/// Sends the request with no callback.
		/// </summary>
		void Send();

		/// <summary>
		/// Sends the request with specified callback.
		/// </summary>
		void Send(RenQL.ResponseHandler callback);

		/// <summary>
		/// Stops the request.
		/// </summary>
		void Stop();

		/// <summary>
		/// Disposes the request instance.
		/// The object becomes unusable after calling this method.
		/// </summary>
		void Dispose();
	}
}

