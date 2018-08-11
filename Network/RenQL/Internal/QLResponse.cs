using System;
using Renko.Data;

namespace Renko.Network.Internal
{
	/// <summary>
	/// Response object of GraphQL request.
	/// </summary>
	public class QLResponse : IQLResponse {
		
		/// <summary>
		/// Returns the error message (if there was any).
		/// </summary>
		public string ErrorMessage {
			get; private set;
		}

		/// <summary>
		/// Returns whether this response was an error.
		/// </summary>
		public bool IsError {
			get { return !string.IsNullOrEmpty(ErrorMessage); }
		}

		/// <summary>
		/// Returns the raw text data response.
		/// </summary>
		public string TextData {
			get; private set;
		}

		/// <summary>
		/// Returns the JsonObject representation of response text.
		/// * Will always create a new JsonObject.
		/// </summary>
		public JsonObject JsonData {
			get { return Json.Parse(TextData); }
		}


		public QLResponse (QLWebRequest webRequest)
		{
			// Parse out response data
			var req = webRequest.WebRequest;

			// Make sure it's not an error first.
			if(req.isError) {
				// Set error message
				ErrorMessage = req.error;
			}
			else {
				// Set text data.
				TextData = req.downloadHandler.text;
			}
		}
	}
}

