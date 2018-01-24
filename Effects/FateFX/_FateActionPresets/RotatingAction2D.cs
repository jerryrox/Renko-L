using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// Fate action preset for rotating animation around z-axis only.
	/// </summary>
	public class RotatingAction2D : FateAction {

		private Transform myTransform;

		private float rotateFrom;
		private float rotateTo;
		private float rotateChange;

		private Vector3 cachedVector;


		public RotatingAction2D(Transform target, float from, float to, EaseType easeType) : base(easeType) {
			SetTransform(target);
			SetRotations(from, to);
		}

		public void SetTransform(Transform target) {
			myTransform = target;
			cachedVector = myTransform.localEulerAngles;
		}

		public void SetRotations(float from, float to) {
			rotateFrom = from;
			rotateTo = to;
			rotateChange = to - from;
		}

		/// <summary>
		/// Animates with specified progress value (0~1).
		/// </summary>
		public override void Animate (float progress) {
			cachedVector.z = Easing.Handlers[EaseMethod](progress, rotateFrom, rotateChange, Duration);
			myTransform.localEulerAngles = cachedVector;
		}
	}
}

