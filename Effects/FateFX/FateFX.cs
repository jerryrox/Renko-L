using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;
using Renko.Diagnostics;
using Renko;

namespace Renko.Effects
{
	/// <summary>
	/// A simple library that allows you to create animations programmatically.
	/// </summary>
	public partial class FateFX : MonoBehaviour {

		/// <summary>
		/// The smallest speed value allowed for any Fate items.
		/// </summary>
		public const float MinimumSpeed = 0.001f;

		/// <summary>
		/// Just a static field for caching deltatime (scaled).
		/// </summary>
		public static float DeltaTime;

		/// <summary>
		/// Direct instance to this class.
		/// </summary>
		private static FateFX I;

		/// <summary>
		/// Update process handler.
		/// </summary>
		private Updater updater;

		/// <summary>
		/// Overall speed of FateFX update.
		/// </summary>
		private float speed;


		/// <summary>
		/// The overall speed of FateFX update.
		/// Will affect all Fate Items globally.
		/// For Item-level speed tweaking, use FateItem.Speed property instead.
		/// </summary>
		public static float Speed {
			get { return I.speed; }
			set { I.speed = Mathf.Clamp(value, MinimumSpeed, float.MaxValue); }
		}


		/// <summary>
		/// Initializes FateFX module.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;

			I = RenkoLibrary.CreateModule<FateFX>();
			I.updater = new Updater();
			I.speed = 1f;

			// Dependency
			Easing.Initialize();
		}

		/// <summary>
		/// Destroys FateFX module.
		/// </summary>
		public static void Destroy() {
			if(I == null)
				return;

			Destroy(I.gameObject);
		}

		/// <summary>
		/// Registers the specified item to update queue.
		/// </summary>
		public static void RegisterItem(FateItem item) {
			I.updater.AddItem(item);
		}

		/// <summary>
		/// Removes the specified item from update queue.
		/// </summary>
		public static void RemoveItem(FateItem item) {
			I.updater.RemoveItem(item);
		}

		void Update() {
			DeltaTime = Time.deltaTime * speed;
			updater.Update();
		}
	}
}