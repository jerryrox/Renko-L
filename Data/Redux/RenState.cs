using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renko.Utility;

namespace Renko.Data
{
	/// <summary>
	/// A state manager used on a global scope.
	/// </summary>
	public class RenState : MonoBehaviour {
		
		private static RenState I;


		/// <summary>
		/// Initializes a new instance of the state manager.
		/// </summary>
		public static void Initialize() {
			if(I != null)
				return;

			I = RenkoLibrary.CreateModule<RenState>();
		}
	}
}