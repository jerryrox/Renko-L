using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// A class that represents a time section within FateFX.
	/// </summary>
	public class FateSection : IDisposable {

		/// <summary>
		/// Callback for on start event.
		/// </summary>
		public CallbackHandler OnStart;

		/// <summary>
		/// Callback for on end event.
		/// </summary>
		public CallbackHandler OnEnd;

		/// <summary>
		/// List of actions to perform during this section.
		/// </summary>
		private List<FateAction> actions;

		/// <summary>
		/// The starting time of this section.
		/// </summary>
		private float startTime;

		/// <summary>
		/// The ending time of this section.
		/// </summary>
		private float endTime;

		/// <summary>
		/// The actual start time of this section after applying delay.
		/// </summary>
		private float delayedStartTime;

		/// <summary>
		/// The actual end time of this section after applying delay.
		/// </summary>
		private float delayedEndTime;


		/// <summary>
		/// Returns the duration of this section.
		/// </summary>
		public float Duration {
			get { return endTime - startTime; }
		}

		/// <summary>
		/// Returns the actual starting time of this section, including delay.
		/// </summary>
		public float StartTime {
			get { return delayedStartTime; }
		}

		/// <summary>
		/// Returns the actual ending time of this section, including delay.
		/// </summary>
		public float EndTime {
			get { return delayedEndTime; }
		}

		/// <summary>
		/// Returns the amount of delay to apply on startTime and endTime.
		/// </summary>
		public float Delay {
			get { return delayedStartTime - startTime; }
		}


		/// <summary>
		/// Delegate for handling callback events on section start/end.
		/// </summary>
		public delegate void CallbackHandler(FateSection section);


		public FateSection(float startTime, float endTime) {
			actions = new List<FateAction>();
			this.startTime = startTime;
			this.endTime = endTime;
		}

		/// <summary>
		/// Updates fate actions.
		/// </summary>
		public void Update(float curTime, float lastTime) {
			if(curTime <= startTime || lastTime >= endTime)
				return;

			// On start event
			if(lastTime <= startTime) {
				InvokeEvent(OnStart);
			}

			// On end event.
			// Here, we manually call AnimateActions with progress 1 so it should just return after event
			if(curTime >= endTime) {
				AnimateActions(1f);
				InvokeEvent(OnEnd);
				return;
			}

			// Update progress
			AnimateActions(Cirno.InverseLerpUnclamped(
				startTime,
				endTime,
				curTime
			));
		}

		/// <summary>
		/// Disposes the section.
		/// </summary>
		public void Dispose() {
			actions.Clear();
		}

		/// <summary>
		/// Sets the amount of delay to apply on start.
		/// </summary>
		public void SetDelay(float delay) {
			delayedStartTime = startTime + delay;
			delayedEndTime = endTime + delay;
		}

		/// <summary>
		/// Adds the specified action to this section.
		/// </summary>
		public void AddAction(FateAction action) {
			action.Duration = Duration;
			actions.Add(action);
		}

		/// <summary>
		/// Invokes the specified event if not null.
		/// </summary>
		void InvokeEvent(CallbackHandler handler) {
			if(handler != null)
				handler(this);
		}

		/// <summary>
		/// Animates all actions in this section.
		/// </summary>
		void AnimateActions(float progress) {
			for(int i=0; i<actions.Count; i++)
				actions[i].Animate(progress);
		}
	}
}

