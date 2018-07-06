using System;

namespace Renko.Network
{
	/// <summary>
	/// A class that provides Netko delegate signatures and contains events in an instance.
	/// </summary>
	public class NetkoEvent {
		
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
		private NetkoItem item;


		/// <summary>
		/// Delegate for handling general callbacks.
		/// </summary>
		public delegate void GeneralHandler(NetkoItem item);

		/// <summary>
		/// Delegate for handling progress callbacks.
		/// </summary>
		public delegate void ProgressHandler(NetkoItem item, float progress);


		public NetkoEvent(NetkoItem item) {
			this.item = item;
		}

		/// <summary>
		/// Registers specified set of callbacks.
		/// Or you can directly access each event to add callbacks.
		/// </summary>
		public void Setup(GeneralHandler onRequested, ProgressHandler onProcessing,
			GeneralHandler onFinished, GeneralHandler onTerminated) {

			OnRequested += onRequested;
			OnProcessing += onProcessing;
			OnFinished += onFinished;
			OnTerminated += onTerminated;
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

