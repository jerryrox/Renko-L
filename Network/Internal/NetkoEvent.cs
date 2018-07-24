using System;

namespace Renko.Network.Internal
{
	/// <summary>
	/// A class that provides Netko delegate signatures and contains events in an instance.
	/// </summary>
	public class NetkoEvent : INetkoEvent {
		
		/// <summary>
		/// Event to invoke when the request is made.
		/// </summary>
		public event GeneralHandler OnRequested;

		/// <summary>
		/// Event to invoke while request is being processed.
		/// </summary>
		public event ProgressHandler OnProcessing;

		/// <summary>
		/// Event to invoke when the request is finished.
		/// </summary>
		public event GeneralHandler OnFinished;

		/// <summary>
		/// Event to invoke when this item is terminated.
		/// </summary>
		public event GeneralHandler OnTerminated;

		/// <summary>
		/// The item that contains this instance.
		/// </summary>
		private INetkoItem item;


		/// <summary>
		/// Delegate for handling general callbacks.
		/// </summary>
		public delegate void GeneralHandler(INetkoItem item);

		/// <summary>
		/// Delegate for handling progress callbacks.
		/// </summary>
		public delegate void ProgressHandler(INetkoItem item, float progress);


		public NetkoEvent(INetkoItem item) {
			this.item = item;
		}

		/// <summary>
		/// Clears all registered events.
		/// </summary>
		public void Clear() {
			OnRequested = null;
			OnProcessing = null;
			OnFinished = null;
			OnTerminated = null;
		}

		/// <summary>
		/// Invokes OnRequest event.
		/// </summary>
		public void InvokeOnRequested() {
			if(OnRequested != null)
				OnRequested(item);
		}

		/// <summary>
		/// Invokes OnProcessing event.
		/// </summary>
		public void InvokeOnProcessing(float progress) {
			if(OnProcessing != null)
				OnProcessing(item, progress);
		}

		/// <summary>
		/// Invokes OnFinished event.
		/// </summary>
		public void InvokeOnFinished() {
			if(OnFinished != null)
				OnFinished(item);
		}

		/// <summary>
		/// Invokes OnTerminated event.
		/// </summary>
		public void InvokeOnTerminated() {
			if(OnTerminated != null)
				OnTerminated(item);
		}
	}
}

