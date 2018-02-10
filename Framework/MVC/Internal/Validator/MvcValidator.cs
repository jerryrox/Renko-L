using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Renko.Extensions;

namespace Renko.MVCFramework
{
	public static partial class MvcValidator {

		/// <summary>
		/// Checks whether specified config values are setup correctly.
		/// </summary>
		public static ValidationResult CheckConfigValidity(MvcConfig config) {
			var results = new ValidationResult[] {
				CheckConfigViewValidity(config),
				CheckNameConflicts(config),
				CheckBaseViewClassName(config.BaseClassName)
			};

			for(int i=0; i<results.Length; i++) {
				if(results[i] != ValidationResult.Success)
					return results[i];
			}
			return ValidationResult.Success;
		}

		/// <summary>
		/// Checks whether all config views' values are setup correctly.
		/// </summary>
		public static ValidationResult CheckConfigViewValidity(MvcConfig config) {
			var configViews = config.Views;
			for(int i=0; i<configViews.Count; i++) {
				var curView = configViews[i];

				var curResult = ValidationResult.Success;
				if((curResult = CheckClassName(curView.Name)) != ValidationResult.Success)
					return curResult;
				if((curResult = CheckBaseViewClassName(curView.BaseClassName)) != ValidationResult.Success)
					return curResult;
			}

			return ValidationResult.Success;
		}

		/// <summary>
		/// Checks whether there is any config view whose class name conflicts with the other.
		/// </summary>
		public static ValidationResult CheckNameConflicts(MvcConfig config) {
			List<string> viewNames = new List<string>(config.Views.Count);
			for(int i=0; i<config.Views.Count; i++) {
				var curName = config.Views[i].Name;
				if(viewNames.Contains(curName))
					return ValidationResult.ViewNameConflicts;
				viewNames.Add(curName);
			}
			return ValidationResult.Success;
		}

		/// <summary>
		/// Checks whether specified value is valid as base MVC view class name.
		/// </summary>
		public static ValidationResult CheckBaseViewClassName(string value) {
			if(!value.Equals(BaseMvcView.ClassName)) {
				// We must check whether the type acually exists.
				Type type = Type.GetType(value);
				if(type == null) {
					return ValidationResult.TypeDoesntExist;
				}
				else {
					// And the class must implement the IMvcView interface.
					var interfaces = type.GetInterfaces();
					if(!interfaces.Contains(typeof(IMvcView))) {
						return ValidationResult.TypeDoesntImplementViewInterface;
					}
				}
			}

			return ValidationResult.Success;
		}

		/// <summary>
		/// Checks whether specified value is a valid class name.
		/// </summary>
		public static ValidationResult CheckClassName(string value) {
			if(string.IsNullOrEmpty(value))
				return ValidationResult.ViewNameInvalid;
			if(Regex.IsMatch(value, @"^[_a-zA-Z]") && Regex.IsMatch(value, @"^[_a-zA-Z0-9]*$"))
				return ValidationResult.Success;
			return ValidationResult.ViewNameInvalid;
		}
	}
}

