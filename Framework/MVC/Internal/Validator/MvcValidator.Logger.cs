#if NGUI
#if UNITY_EDITOR
using RenkoEditor;
#endif
using System;
using UnityEngine;
using Renko.Diagnostics;

namespace Renko.MVCFramework.Internal
{
	public static partial class MvcValidator {
		
		public static class Output {

			/// <summary>
			/// Displays the validation result via editor alert message.
			/// </summary>
			public static void DisplayAlert(ValidationResult result) {
#if UNITY_EDITOR
				EditorDialog.OpenAlert(result.ToString(), ParseMessage(result));
#endif
			}

			/// <summary>
			/// Displays the validation result via console log.
			/// </summary>
			public static void DisplayLog(ValidationResult result) {
				RenLog.Log(LogLevel.Warning, result.ToString() + " - " + ParseMessage(result));
			}

			/// <summary>
			/// Parses the validation result to string value.
			/// </summary>
			private static string ParseMessage(ValidationResult result) {
				switch(result) {
				case ValidationResult.Success:
					return "Successful";
				case ValidationResult.TypeDoesntExist:
					return "The specified type doesn't exist.";
				case ValidationResult.TypeDoesntImplementViewInterface:
					return "The specified type doesn't implement IMvcView interface.";

				case ValidationResult.ViewNameConflicts:
					return "There are views with conflicting names.";
				case ValidationResult.ViewNameInvalid:
					return "The specified view name is invalid.";
				}
				return "Unknown result: " + result;
			}
		}
	}
}
#endif