using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// Fate action preset for general rotating animation.
	/// </summary>
	public class RotatingAction : FateAction {

		private Transform myTransform;

		private Vector3 rotateFrom;
		private Vector3 rotateTo;
		private Vector3 rotateChange;


		public RotatingAction(Transform target, Vector3 from, Vector3 to, EaseType easeType = EaseType.Linear) {
			SetTransform(target);
			SetRotations(from, to);

			EaseMethod = easeType;
		}

		public void SetTransform(Transform target) {
			myTransform = target;
		}

		public void SetRotations(Vector3 from, Vector3 to) {
			rotateFrom = from;
			rotateChange = rotateTo = to;

			rotateChange.x -= from.x;
			rotateChange.y -= from.y;
			rotateChange.z -= from.z;
		}

		/// <summary>
		/// Animates with specified progress value (0~1).
		/// </summary>
		public override void Animate (float progress) {
			progress = GetEasedProgress(progress);
			myTransform.localEulerAngles = new Vector3(
				Easing.Linear(progress, rotateFrom.x, rotateChange.x, 0f),
				Easing.Linear(progress, rotateFrom.y, rotateChange.y, 0f),
				Easing.Linear(progress, rotateFrom.z, rotateChange.z, 0f)
			);
		}
	}
}

