using System;
using Renko.Data;

namespace Renko.Network
{
	/// <summary>
	/// Interface of a GraphQL response object.
	/// </summary>
	public interface IQLResponse {

		/// <summary>
		/// Returns the error message (if there was any).
		/// </summary>
		string ErrorMessage { get; }

		/// <summary>
		/// Returns whether this response was an error.
		/// </summary>
		bool IsError { get; }

		/// <summary>
		/// Returns the raw text data response.
		/// </summary>
		string TextData { get; }

		/// <summary>
		/// Returns the JsonObject representation of response text.
		/// * Will always create a new JsonObject.
		/// </summary>
		JsonObject JsonData { get; }
	}
}

