using System.Collections;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Renko.Network
{
	/// <summary>
	/// A class that contains data required to make a web request.
	/// </summary>
	public class WebRequestInfo {

		/// <summary>
		/// The base url to make a request to.
		/// </summary>
		protected string baseUrl;

		/// <summary>
		/// Method used for making a web request.
		/// </summary>
		protected string method;

		/// <summary>
		/// (Optional) Query param string builder for convenience.
		/// </summary>
		protected UrlParamBuilder urlParamBuilder;

		/// <summary>
		/// (Optional) Form to submit during a POST request.
		/// </summary>
		protected WWWForm form;


		/// <summary>
		/// Base url to make a request on.
		/// If you wish to retrieve a url with the query param applied, use the Url property instead.
		/// </summary>
		public string BaseUrl {
			get { return baseUrl; }
			set {
				baseUrl = value;
				if(baseUrl == null)
					baseUrl = string.Empty;
			}
		}

		/// <summary>
		/// Returns the target url to make a request on.
		/// Will return the base url + UrlParam value.
		/// </summary>
		public string Url {
			get { return baseUrl + urlParamBuilder.Output(); }
		}

		/// <summary>
		/// Request method to use.
		/// </summary>
		public string Method {
			get { return method; }
			set {
				switch(value) {
				case HttpMethods.CREATE:
				case HttpMethods.DELETE:
				case HttpMethods.GET:
				case HttpMethods.HEAD:
				case HttpMethods.POST:
				case HttpMethods.PUT:
					method = value;
					break;
				default:
					method = HttpMethods.GET;
					break;
				}
			}
		}

		/// <summary>
		/// Query parameter string to add on the base url.
		/// </summary>
		public string UrlParam {
			get { return urlParamBuilder.Output(); }
			set { urlParamBuilder = UrlParamBuilder.Create(value); }
		}

		/// <summary>
		/// The form object to use during post requests.
		/// </summary>
		public WWWForm Form {
			get { return form; }
			set {
				form = value;
				if(form == null)
					form = new WWWForm();
			}
		}

		/// <summary>
		/// The object used for transferring data while POST request.
		/// </summary>
		public UploadHandlerRaw UploadHandler {
			get; set;
		}


		public WebRequestInfo(string url, string method) {
			baseUrl = url;
			this.method = method;
			urlParamBuilder = UrlParamBuilder.Create();
			form = new WWWForm();
		}

		/// <summary>
		/// Adds specified key and value to url query param.
		/// </summary>
		public void AddUrlParam(string key, object value) {
			urlParamBuilder.Add(key, value);
		}

		/// <summary>
		/// Adds specified key and value to post form.
		/// </summary>
		public void AddFormParam(string key, object value, Encoding encoding = null) {
			if(encoding == null)
				encoding = Encoding.UTF8;
			form.AddField(key, value.ToString(), encoding);
		}

		/// <summary>
		/// Adds specified binary data to post form.
		/// </summary>
		public void AddFormBinary(string key, byte[] data, string fileName = null, string mimeType = null) {
			form.AddBinaryData(key, data, fileName, mimeType);
		}
	}
}

