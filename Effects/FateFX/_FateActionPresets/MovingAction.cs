using System;
using UnityEngine;
using Renko.Utility;

namespace Renko.Effects
{
	/// <summary>
	/// Fate action preset for general moving animation.
	/// </summary>
	public class MovingAction : FateAction {

		private Transform myTransform;

		private Vector3 moveFrom;
		private Vector3 moveTo;
		private Vector3 moveChange;


		public MovingAction(Transform target, Vector3 from, Vector3 to, EaseType easeType = EaseType.Linear) {
			SetTransform(target);
			SetPositions(from, to);

			EaseMethod = easeType;
		}

		public void SetTransform(Transform target) {
			myTransform = target;
		}

		public void SetPositions(Vector3 from, Vector3 to) {
			moveFrom = from;
			moveChange = moveTo = to;

			moveChange.x -= from.x;
			moveChange.y -= from.y;
			moveChange.z -= from.z;
		}

		/// <summary>
		/// Animates with specified progress value (0~1).
		/// </summary>
		public override void Animate (float progress) {
			progress = GetEasedProgress(progress);
			myTransform.localPosition = new Vector3(
				Easing.Linear(progress, moveFrom.x, moveChange.x, 0f),
				Easing.Linear(progress, moveFrom.y, moveChange.y, 0f),
				Easing.Linear(progress, moveFrom.z, moveChange.z, 0f)
			);
		}
	}
}

