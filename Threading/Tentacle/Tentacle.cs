using System.Threading;

namespace Renko.Threading
{
	/// <summary>
	/// A simple class that allows you to implement multithreaded processes easily.
	/// </summary>
	public partial class Tentacle {


		/// <summary>
		/// Delegate that handles the work.
		/// </summary>
		public delegate object ProcessHandler(object argument);


		/// <summary>
		/// Returns a new Task that processes the specified handler in another thread.
		/// </summary>
		public static Task Create(ProcessHandler handler, object argument = null) {
			return new Task(handler, argument);
		}

		/// <summary>
		/// Executes the specified handler in main thread.
		/// </summary>
		public static object RunOnMainThread(UnityThread.ProcessHandler handler) {
			return UnityThread.Dispatch(handler);
		}
	}
}