using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Network.Internal;

namespace Renko.Network
{
	/// <summary>
	/// Interface for netko item's event manager.
	/// </summary>
	public interface INetkoEvent {

		/// <summary>
		/// Event to invoke when the request is made.
		/// </summary>
		event NetkoEvent.GeneralHandler OnRequested;

		/// <summary>
		/// Event to invoke while request is being processed.
		/// </summary>
		event NetkoEvent.ProgressHandler OnProcessing;

		/// <summary>
		/// Event to invoke when the request is finished.
		/// </summary>
		event NetkoEvent.GeneralHandler OnFinished;

		/// <summary>
		/// Event to invoke when this item is terminated.
		/// </summary>
		event NetkoEvent.GeneralHandler OnTerminated;


		/// <summary>
		/// Clears all registered events.
		/// </summary>
		void Clear();
	}
}