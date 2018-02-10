using System;
using System.Collections;
using System.Collections.Generic;

namespace Renko.MVCFramework
{
	public static class ConfigViewEditorFlags {

		public static List<bool> IsOpen;


		public static void Setup(MvcConfig config) {
			if(IsOpen == null) {
				IsOpen = new List<bool>();
			}

			for(int i=IsOpen.Count; i<config.Views.Count; i++)
				IsOpen.Add(false);
			for(int i=config.Views.Count; i<IsOpen.Count; i++) {
				IsOpen.RemoveAt(0);
			}
		}

		public static void ResetFlags() {
			for(int i=0; i<IsOpen.Count; i++)
				IsOpen[i] = false;
		}

		public static void SetOpen(int viewIndex) {
			ResetFlags();
			IsOpen[viewIndex] = true;
		}
	}
}

