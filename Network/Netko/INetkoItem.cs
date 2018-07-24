using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Network.Internal;

namespace Renko.Network
{
	/// <summary>
	/// Interface of a netko item.
	/// </summary>
	public interface INetkoItem {

		/// <summary>
		/// Returns the event handler of this item.
		/// </summary>
		INetkoEvent Events { get; }

		/// <summary>
		/// Returns the request info of this item.
		/// </summary>
		INetkoRequest Request { get; }

		/// <summary>
		/// Returns the response info of this item.
		/// </summary>
		INetkoResponse Response { get; }

		/// <summary>
		/// An option data associated with this item.
		/// You can assign any value here to store value and use it later when OnFinished is called.
		/// </summary>
		object ExtraData { get; set; }

		/// <summary>
		/// The group id which this item is associated with.
		/// </summary>
		int GroupId { get; set; }


		/// <summary>
		/// Reinitializes this item's state for retry.
		/// </summary>
		void Retry();

		/// <summary>
		/// Forcefully stops request if on-going.
		/// </summary>
		void ForceStop();
	}
}