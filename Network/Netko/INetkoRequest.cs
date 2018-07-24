using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Network
{
	/// <summary>
	/// Interface for netko item's request info.
	/// </summary>
	public interface INetkoRequest {

		/// <summary>
		/// Returns the request type being performed.
		/// </summary>
		Netko.RequestType Type { get; }

		/// <summary>
		/// Returns the url which this item is making a request to .
		/// </summary>
		string Url { get; }

		/// <summary>
		/// Returns the request method currently being used.
		/// </summary>
		string Method { get; }

		/// <summary>
		/// Returns a readable message of request errors.
		/// </summary>
		string Error { get; }

		/// <summary>
		/// Returns the current download progress from 0~1.
		/// </summary>
		float DownloadProgress { get; }

		/// <summary>
		/// Returns the current upload progress from 0~1.
		/// </summary>
		float UploadProgress { get; }

		/// <summary>
		/// Returns either DownloadProgress or UploadProgress based on current request.
		/// </summary>
		float Progress { get; }

		/// <summary>
		/// Time in seconds which makes this item end with a timeout error.
		/// Relative to Time.realtimeSinceStartup.
		/// </summary>
		float TimeOutTime { get; set; }

		/// <summary>
		/// Returns whether the owner NetkoItem is terminated.
		/// </summary>
		bool IsTerminated { get; }

		/// <summary>
		/// Returns whether this item is being processed.
		/// </summary>
		bool IsProcessing { get; }

		/// <summary>
		/// Returns whether the request is finished.
		/// May be "finished" earlier due to explicit errors.
		/// </summary>
		bool IsFinished { get; }

		/// <summary>
		/// Returns whether the request has timed out.
		/// </summary>
		bool IsTimeOut { get; }
	}
}