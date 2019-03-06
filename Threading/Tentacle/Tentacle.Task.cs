using System;
using System.Threading;
using UnityEngine;
using Renko.Diagnostics;

using ThreadPriority = System.Threading.ThreadPriority;

namespace Renko.Threading
{
	public partial class Tentacle {

		/// <summary>
		/// A class that represents a task to process.
		/// </summary>
		public class Task : CustomYieldInstruction {

			/// <summary>
			/// An object for lock syntax.
			/// </summary>
			private object locker;

			/// <summary>
			/// An argument which will be passed to the task.
			/// </summary>
			private object argument;

			/// <summary>
			/// A flag to ensure the Start() method is only called once.
			/// </summary>
			private bool alreadyStarted;

			/// <summary>
			/// A reference to this task's worker thread.
			/// </summary>
			private Thread workerThread;

			/// <summary>
			/// The process to handler in the new thread.
			/// </summary>
			private ProcessHandler process;


			/// <summary>
			/// Whether this task has finished its process.
			/// </summary>
			public bool IsFinished {
				get; private set;
			}

			/// <summary>
			/// The priority of this task's thread.
			/// </summary>
			public ThreadPriority Priority {
				get {
					return (ThreadPriority)DoLockedAction(() => {
						if(workerThread == null)
							return ThreadPriority.Normal;
						return workerThread.Priority;
					});
				}
				set {
					DoLockedAction(() => {
						if(workerThread != null)
							workerThread.Priority = value;
					});
				}
			}

			/// <summary>
			/// Whether this task had an error.
			/// </summary>
			public bool IsError {
				get { return ErrorData != null; }
			}

			/// <summary>
			/// An Exception object that contains the error's details.
			/// </summary>
			public Exception ErrorData {
				get; private set;
			}

			/// <summary>
			/// The output from this task.
			/// </summary>
			public object ReturnData {
				get; private set;
			}

			/// <summary>
			/// Abstract property of CustomYieldInstruction for coroutine yields.
			/// </summary>
			public override bool keepWaiting {
				get { return !IsFinished; }
			}


			public Task(ProcessHandler handler, object argument) {
				alreadyStarted = false;
				locker = new object();
				workerThread = new Thread(DoProcess);
				process = handler;
				IsFinished = false;
			}

			/// <summary>
			/// Starts the task and returns this instance.
			/// May return null if this task was already invoked once.
			/// </summary>
			public Task Start() {
				if(alreadyStarted) {
					RenLog.Log(LogLevel.Warning, "Tentacle.Task.Start - A task can't be called more than once on the same instance!");
					return null;
				}

				alreadyStarted = true;
				workerThread.Start();
				return this;
			}

			/// <summary>
			/// Force-stops the task.
			/// </summary>
			public void Stop() {
				DoLockedAction(() => {
					if(workerThread != null)
						workerThread.Interrupt();
				});
			}

			/// <summary>
			/// Handles threaded processes.
			/// </summary>
			void DoProcess() {
				try {
					ReturnData = process(argument);
				}
				catch(Exception e) {
					ErrorData = e;
				}
				finally {
					DoLockedAction(() => {
						IsFinished = true;
						workerThread = null;
					});
				}
			}

			/// <summary>
			/// Invokes the specified action with a lock for thread-safety.
			/// </summary>
			void DoLockedAction(Action handler) {
				lock(locker) {
					handler();
				}
			}

			/// <summary>
			/// Invokes the specified action with a lock for thread-safety.
			/// </summary>
			object DoLockedAction(Func<object> handler) {
				lock(locker) {
					return handler();
				}
			}
		}
	}
}

