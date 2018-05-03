using System;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// The item which represents a single FateFX animation.
	/// </summary>
	public class FateItem {

		/// <summary>
		/// Callback for resetting process.
		/// You can use this to reset things back to their initial state for another loop or
		/// use it as an on-end callback.
		/// Will be called upon: Loop, Stop
		/// </summary>
		public ResetHandler OnReset;

		/// <summary>
		/// Time sections of this item.
		/// </summary>
		private List<FateSection> sections;

		/// <summary>
		/// Speed of this item.
		/// </summary>
		private float speed;

		/// <summary>
		/// Duration of this item, excluding delays.
		/// </summary>
		private float duration;

		/// <summary>
		/// Total duration of this item, including delays.
		/// </summary>
		private float totalDuration;

		/// <summary>
		/// Currently passed time.
		/// </summary>
		private float curTime;

		/// <summary>
		/// Amount of delay to apply on start.
		/// </summary>
		private float startDelay;

		/// <summary>
		/// Amount of delay to apply on end.
		/// </summary>
		private float endDelay;

		/// <summary>
		/// Whether to loop on animation end.
		/// </summary>
		private bool isLoop;

		/// <summary>
		/// Backing field of IsPlaying property.
		/// </summary>
		private bool isPlaying;


		/// <summary>
		/// The speed of this item.
		/// </summary>
		public float Speed {
			get { return speed; }
			set { speed = Mathf.Clamp(value, FateFX.MinimumSpeed, float.MaxValue); }
		}

		/// <summary>
		/// Whether the animation should loop on end.
		/// </summary>
		public bool IsLoop {
			get { return isLoop; }
			set { isLoop = value; }
		}

		/// <summary>
		/// Returns whether animation is playing.
		/// </summary>
		public bool IsPlaying {
			get { return isPlaying; }
		}

		/// <summary>
		/// Returns the duration of this item, excluding delays.
		/// </summary>
		public float Duration {
			get { return duration; }
		}

		/// <summary>
		/// Returns the total duration of this item, including delays.
		/// </summary>
		public float TotalDuration {
			get { return totalDuration; }
		}

		/// <summary>
		/// Returns the current time of this item.
		/// </summary>
		public float CurrentTime {
			get { return Mathf.Clamp(curTime, 0f, totalDuration); }
		}

		/// <summary>
		/// Returns current progress (0~1) of this item.
		/// </summary>
		public float Progress {
			get { return (duration - curTime) / duration; }
		}


		/// <summary>
		/// Delegate for handling reset events.
		/// </summary>
		public delegate void ResetHandler(FateItem item);


		public FateItem(int listCapacity = 0) {
			speed = 1f;
			sections = new List<FateSection>(listCapacity);
		}

		/// <summary>
		/// Updates the item for animation.
		/// </summary>
		public void Update() {
			float lastTime = curTime;
			curTime += FateFX.DeltaTime * speed;

			UpdateSections(curTime, lastTime);

			// On update finished
			if(curTime > totalDuration) {
				HandleUpdateFinished();
			}
		}

		/// <summary>
		/// Plays the animation.
		/// </summary>
		public void Play() {
			isPlaying = true;

			SeekTo(curTime, false);
			FateFX.RegisterItem(this);
		}

		/// <summary>
		/// Pauses the animation.
		/// </summary>
		public void Pause() {
			isPlaying = false;

			FateFX.RemoveItem(this);
		}

		/// <summary>
		/// Stops the animation and resets time to 0.
		/// If update is true, this method will call SeekTo with time 0.
		/// Else, only the time is set to 0 and the view state will stay.
		/// </summary>
		public void Stop(bool update = false) {
			isPlaying = false;

			if(update)
				SeekTo(0f, false);
			else
				curTime = 0f;
			
			FateFX.RemoveItem(this);
			InvokeResetter();
		}

		/// <summary>
		/// Sets current time to specified value.
		/// If isRatio is true, the specified 'time' will be interpreted as an interpolant value (0~1).
		/// </summary>
		public void SeekTo(float time, bool isRatio) {
			if(isRatio)
				curTime = Mathf.Lerp(0f, totalDuration, time);
			else
				curTime = Mathf.Clamp(time, 0f, totalDuration);
			
			UpdateSections(curTime, curTime - FateFX.DeltaTime * speed);
		}

		/// <summary>
		/// Clears all sections in this item.
		/// </summary>
		public void Clear() {
			sections.Clear();
		}

		/// <summary>
		/// Sets the amount of delay to apply on animation start/end.
		/// </summary>
		public void SetDelays(float startDelay, float endDelay) {
			this.startDelay = startDelay;
			this.endDelay = endDelay;

			RefreshTimeValues();
		}

		/// <summary>
		/// Creates a new section, adds it, and returns it.
		/// </summary>
		public FateSection CreateSection(float startTime, float endTime, int listCapacity = 0) {
			return AddSection(new FateSection(startTime, endTime, listCapacity));
		}

		/// <summary>
		/// Creates a fate section for callback event when current time reaches the specified value.
		/// </summary>
		public FateSection CreateEvent(float time, FateSection.CallbackHandler callback) {
			FateSection section = new FateSection(time, time, 1);
			section.OnStart = callback;
			return AddSection(section);
		}

		/// <summary>
		/// Adds the specified section and returns it.
		/// </summary>
		public FateSection AddSection(FateSection section) {
			sections.Add(section);
			RefreshTimeValues();
			return section;
		}

		/// <summary>
		/// Invokes the OnReset handler.
		/// </summary>
		void InvokeResetter() {
			if(OnReset != null)
				OnReset(this);
		}

		/// <summary>
		/// Refreshes local variables related to timing.
		/// </summary>
		void RefreshTimeValues() {
			ApplyDelay();
			FindDuration();

			// startDelay is already included in duration.
			totalDuration = duration + endDelay;
		}

		/// <summary>
		/// Applies delay for each fate section.
		/// </summary>
		void ApplyDelay() {
			for(int i=0; i<sections.Count; i++)
				sections[i].SetDelay(startDelay);
		}

		/// <summary>
		/// Iterates through each section to find the duration of this item.
		/// </summary>
		void FindDuration() {
			duration = 0f;
			for(int i=0; i<sections.Count; i++) {
				float sectionEnd = sections[i].EndTime;
				if(sectionEnd > duration)
					duration = sectionEnd;
			}
		}

		/// <summary>
		/// Updates all sections with specified time values.
		/// </summary>
		void UpdateSections(float curTime, float lastTime) {
			for(int i=0; i<sections.Count; i++) {
				sections[i].Update(curTime, lastTime);
			}
		}

		/// <summary>
		/// Handles update finish event.
		/// </summary>
		void HandleUpdateFinished() {
			if(isLoop)
				curTime = 0f;
			else
				Pause();

			InvokeResetter();
		}
	}
}

