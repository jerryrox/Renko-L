using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Network
{
	/// <summary>
	/// A simple class for building url parameters.
	/// </summary>
	public class UrlParamBuilder {

		/// <summary>
		/// Current parameter string being manipulated.
		/// </summary>
		private string curParam;


		private UrlParamBuilder(string param) {
			if(param == null)
				param = string.Empty;
			if(param.Length > 0 && param[0] == '?')
				param = param.Remove(0, 1);
			curParam = param;
		}

		/// <summary>
		/// Adds the specified key and value to param.
		/// Value will be escaped automatically.
		/// </summary>
		public UrlParamBuilder Add(string key, object value) {
			if(curParam.Length > 0)
				curParam += '&';
			curParam += key + '=' + WWW.EscapeURL(value.ToString());
			return this;
		}

		/// <summary>
		/// Outputs current parameter.
		/// </summary>
		public string Output() {
			if(curParam.Length > 0) {
				if(curParam[0] != '?')
					return "?" + curParam;
			}
			return curParam;
		}

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		public static UrlParamBuilder Create(string param = null) {
			return new UrlParamBuilder(param);
		}
	}
}

