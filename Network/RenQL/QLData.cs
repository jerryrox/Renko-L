using Renko.Data;

namespace Renko.Network
{
	/// <summary>
	/// Class that holds GraphQL query and variable data.
	/// </summary>
	public class QLData {

		private string url;
		[JsonAllowSerialize]
		private string query;
		[JsonAllowSerialize]
		private object variables;


		/// <summary>
		/// The url of target server.
		/// </summary>
		public string Url {
			get { return url; }
			set { url = value; }
		}

		/// <summary>
		/// The query string.
		/// </summary>
		public string Query {
			get { return query; }
			set { query = value; }
		}

		/// <summary>
		/// The AnonymousType object representing the variables for Query.
		/// </summary>
		public object Variables {
			get { return variables; }
			set { variables = value; }
		}


		public QLData (string url, string query, object variables)
		{
			Url = url;
			Query = query;
			Variables = variables;
		}

		/// <summary>
		/// Returns the JSON string representation of this object.
		/// </summary>
		public override string ToString()
		{
			return Json.ToString(this);
		}
	}
}

