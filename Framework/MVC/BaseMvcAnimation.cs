using System;
using UnityEngine;
using Renko.Effects;
using Renko.Utility;

namespace Renko.MVCFramework
{
	/// <summary>
	/// Base class for MVC View show/hide animations.
	/// </summary>
	public abstract class BaseMvcAnimation : MonoBehaviour {

		/// <summary>
		/// Whether the animation should play automatically on view show or hide event.
		/// </summary>
		public bool IsAutomatic = true;

		/// <summary>
		/// The target event which triggers this animation.
		/// </summary>
		public MvcAnimationEvent TargetEvent = MvcAnimationEvent.OnViewShow;


		/// <summary>
		/// Returns the FateItem being used for playing animaion.
		/// </summary>
		public abstract FateItem FateAni {
			get;
		}

		/// <summary>
		/// Returns the total duration of the animation.
		/// </summary>
		public abstract float Duration {
			get;
		}


		/// <summary>
		/// Plays the animation with no condition checks.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Plays the animation if this animation's listening to specified event type and
		/// IsAutomatic flag is true.
		/// Returns whether play was successful.
		/// </summary>
		public bool Play(MvcAnimationEvent eventType) {
			if(TargetEvent == eventType && IsAutomatic) {
				Play();
				return true;
			}
			return false;
		}
	}

	public enum MvcAnimationEvent {
		OnViewShow,
		OnViewHide
	}
}

