using System.Collections;
using System.Text;
using System;
using UnityEngine;

namespace Renko.Network
{
	/// <summary>
	/// A class that contains data required to make a simple web request.
	/// </summary>
	public class WebRequestInfo {

		protected string url;
		protected string method;
		protected UrlParamBuilder urlParamBuilder;
		protected WWWForm form;


		/// <summary>
		/// The target url to make a request on.
		/// Any url parameters registered on this info will be added automatically.
		/// </summary>
		public string Url {
			get {
				return url + urlParamBuilder.Output();
			}
			set {
				url = value;
				if(url == null)
					url = string.Empty;
			}
		}
		public string Method {
			get {
				return method;
			}
			set {
				switch(value) {
				case HttpVerb.CREATE:
				case HttpVerb.DELETE:
				case HttpVerb.GET:
				case HttpVerb.HEAD:
				case HttpVerb.POST:
				case HttpVerb.PUT:
					method = value;
					break;

				default:
					method = HttpVerb.GET;
					break;
				}
			}
		}
		public string UrlParam {
			get {
				return urlParamBuilder.Output();
			}
			set {
				urlParamBuilder = UrlParamBuilder.Create(value);
			}
		}
		public WWWForm Form {
			get {
				return form;
			}
			set {
				form = value;
				if(form == null)
					form = new WWWForm();
			}
		}


		public WebRequestInfo(string _url, string _method) {
			url = _url;
			method = _method;
			urlParamBuilder = UrlParamBuilder.Create();
			form = new WWWForm();
		}

		/// <summary>
		/// Adds specified key and value to url param.
		/// </summary>
		public void AddUrlParam(string key, object value) {
			urlParamBuilder.Add(key, value);
		}

		/// <summary>
		/// Adds specified key and value to form.
		/// </summary>
		public void AddFormParam(string key, object value, Encoding encoding = null) {
			if(encoding == null)
				encoding = Encoding.UTF8;
			form.AddField(key, value.ToString(), encoding);
		}

		/// <summary>
		/// Adds specified binary data to form.
		/// </summary>
		public void AddFormBinary(string key, byte[] data, string fileName = null, string mimeType = null) {
			form.AddBinaryData(key, data, fileName, mimeType);
		}
	}
}

