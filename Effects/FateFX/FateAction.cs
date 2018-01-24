using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// An abstract fate animation handler.
	/// You should derive from this class to 
	/// </summary>
	public abstract class FateAction {

		/// <summary>
		/// Ease method to use.
		/// </summary>
		public EaseType EaseMethod = EaseType.Linear;

		/// <summary>
		/// The duration of this action. Will automatially be set from FateSection so try not to
		/// change this value unless you understand the whole FateFX process.
		/// </summary>
		public float Duration;


		public FateAction(EaseType easeType) {
			EaseMethod = easeType;
		}

		/// <summary>
		/// Animates with specified progress value (0~1).
		/// Note that progress value of 1 is guaranteed.
		/// </summary>
		public abstract void Animate(float progress);

		/// <summary>
		/// Processes the ease calculation and return a new progress value ranging from 0 to 1.
		/// You can save performance by using Easing.Linear with this value for parameter 't'.
		/// Ease type is based on the EaseMethod field.
		/// </summary>
		protected float GetEasedProgress(float progress) {
			return Easing.Handlers[EaseMethod](progress, 0f, 1f, Duration);
		}
	}
}

