#if NGUI
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Renko.Diagnostics;
using Renko.IO;
using Renko.Utility;
using Renko.Data;
using Renko.MVCFramework.Internal;

namespace Renko.MVCFramework
{
	/// <summary>
	/// The configuration model for MVC.
	/// </summary>
	public class MvcConfig {
		
		/// <summary>
		/// The latest configuration file version.
		/// </summary>
		private const int LatestVersion = 0;

		/// <summary>
		/// Current config version.
		/// </summary>
		public int Version;

		/// <summary>
		/// The full name of default MVC view base class.
		/// </summary>
		public string BaseClassName;

		/// <summary>
		/// List of view configurations.
		/// </summary>
		public List<View> Views;


		/// <summary>
		/// Returns a new configuration model with default values.
		/// </summary>
		public static MvcConfig DefaultConfig {
			get { return new MvcConfig(); }
		}

		/// <summary>
		/// Returns the first occurance of view where IsInitial flag is true.
		/// </summary>
		public View InitialView {
			get { return Views.FirstOrDefault(view => view.IsInitial); }
		}


		public MvcConfig() {
			Version = LatestVersion;
			BaseClassName = BaseMvcView.ClassName;
			Views = new List<View>();
		}

		public MvcConfig(string text) : this() {
			Load(text);
		}

		/// <summary>
		/// Loads the config file from resources and returns it.
		/// </summary>
		public static MvcConfig LoadFromResources() {
			TextAsset file = ResourceLoader.LoadTextAsset(
				MvcResources.GetConfigFilePath(false)
			);
			if(file == null) {
				RenLog.Log(LogLevel.Info, "MvcConfig.LoadFromResources - Text asset not found. Returning default configuration.");
				return DefaultConfig;
			}
			return new MvcConfig(file.text);
		}

		/// <summary>
		/// Tries validating this config for further process.
		/// </summary>
		public bool IsConfigValid() {
			var result = MvcValidator.CheckConfigValidity(this);
			if(result != ValidationResult.Success)
				MvcValidator.Output.DisplayAlert(result);
			return result == ValidationResult.Success;
		}

		/// <summary>
		/// Sets the specified value to BaseClassName field, if valid.
		/// </summary>
		public void SetBaseClassName(string value) {
			if(string.IsNullOrEmpty(value)) {
				BaseClassName = BaseMvcView.ClassName;
				return;
			}
			
			var result = MvcValidator.CheckBaseViewClassName(value);
			if(result == ValidationResult.Success)
				BaseClassName = value;
			else
				BaseClassName = BaseMvcView.ClassName;
		}

		/// <summary>
		/// Toggles the IsInitial flag on specified view.
		/// </summary>
		public void ToggleInitial(View view) {
			for(int i=0; i<Views.Count; i++)
				Views[i].IsInitial = false;
			if(view != null)
				view.IsInitial = true;
		}

		/// <summary>
		/// Saves current instance to resources.
		/// </summary>
		public void Save() {
			MvcResources.SaveConfig(this);
		}

		/// <summary>
		/// Parses and applies values from specified config content.
		/// </summary>
		public void Load(string text) {
			JsonObject json = Json.Parse(text);

			Version = json["config_version"].AsInt(LatestVersion);
			BaseClassName = json["base_view_class_name"].AsString(BaseMvcView.ClassName);

			JsonArray views = json["views"].AsArray();
			if(views != null) {
				for(int i=0; i<views.Count; i++) {
					Views.Add(new View(this, views[i].AsObject()));
				}
			}
		}

		public override string ToString () {
			JsonObject json = new JsonObject();

			json["config_version"] = Version;
			json["base_view_class_name"] = BaseClassName;
			JsonArray ar = json["views"] = new JsonArray();
			for(int i=0; i<Views.Count; i++)
				ar.Add(Views[i].ToJson());

			return json.ToString();
		}

		/// <summary>
		/// Model that holds information of a single MVC view.
		/// </summary>
		public class View {

			/// <summary>
			/// The latest view config version.
			/// </summary>
			private const int LatestVersion = 1;

			/// <summary>
			/// The object that contains this view instance.
			/// </summary>
			public MvcConfig Owner;

			/// <summary>
			/// Current view config version.
			/// </summary>
			public int Version;

			/// <summary>
			/// The name of the view.
			/// </summary>
			public string Name;

			/// <summary>
			/// The original name loaded from resources.
			/// </summary>
			public string OriginalName;

			/// <summary>
			/// The full name of custom MVC view base class.
			/// </summary>
			public string BaseClassName;

			/// <summary>
			/// Whether this view is the initial view on startup.
			/// </summary>
			public bool IsInitial;

			/// <summary>
			/// The custom lifecycle method of this view.
			/// </summary>
			public MvcLifeType LifeType;

			/// <summary>
			/// The custom rescaling mode of this view.
			/// </summary>
			public MvcRescaleType ViewRescaleMode = MvcRescaleType.Default;


			/// <summary>
			/// Returns whether this view config is loaded from resources.
			/// </summary>
			public bool IsFromResources {
				get { return !string.IsNullOrEmpty(OriginalName); }
			}


			public View(MvcConfig owner) {
				Owner = owner;
				Version = LatestVersion;
				BaseClassName = owner.BaseClassName;
			}

			public View(MvcConfig owner, JsonObject json) : this(owner) {
				Load(json);
			}

			/// <summary>
			/// Sets the specified value to Name field, if valid.
			/// Returns whether it was successful.
			/// </summary>
			public bool SetViewName(string value) {
				var result = MvcValidator.CheckClassName(value);
				if(result != ValidationResult.Success) {
					MvcValidator.Output.DisplayAlert(result);
					return false;
				}
				Name = value;
				return true;
			}

			/// <summary>
			/// Sets the specified value to BaseClassName field, if valid.
			/// Returns whether it was successful.
			/// </summary>
			public bool SetBaseClassName(string value) {
				if(string.IsNullOrEmpty(value)) {
					BaseClassName = BaseMvcView.ClassName;
					return true;
				}

				var result = MvcValidator.CheckBaseViewClassName(value);
				if(result != ValidationResult.Success) {
					MvcValidator.Output.DisplayAlert(result);
					BaseClassName = BaseMvcView.ClassName;
					return false;
				}
				BaseClassName = value;
				return true;
			}

			/// <summary>
			/// Returns the path to this view's prefab object.
			/// </summary>
			public string GetResourcePath(bool isFullPath, bool fromOriginal = false) {
				return MvcResources.GetViewPrefabPath(this, GetViewName(fromOriginal), isFullPath);
			}

			/// <summary>
			/// Returns the View file name
			/// </summary>
			public string GetViewName(bool fromOriginal = false) {
				return (fromOriginal ? OriginalName : Name) + "View";
			}

			/// <summary>
			/// Returns the Model file name
			/// </summary>
			public string GetModelName(bool fromOriginal = false) {
				return (fromOriginal ? OriginalName : Name) + "Model";
			}

			/// <summary>
			/// Returns the Controller file name
			/// </summary>
			public string GetControllerName(bool fromOriginal = false) {
				return (fromOriginal ? OriginalName : Name) + "Controller";
			}

			/// <summary>
			/// Returns whether all fields in this config view are valid.
			/// </summary>
			public bool IsAllFieldsValid() {
				var result = MvcValidator.CheckConfigViewValidity(Owner);
				if(result != ValidationResult.Success)
					MvcValidator.Output.DisplayAlert(result);
				return result == ValidationResult.Success;
			}

			/// <summary>
			/// Returns the actual MVC base class which will be applied on this view.
			/// </summary>
			public string GetBaseClass() {
				if(string.IsNullOrEmpty(BaseClassName))
					return Owner.BaseClassName;
				return BaseClassName;
			}

			/// <summary>
			/// Parses values from specified json object.
			/// </summary>
			public void Load(JsonObject json) {
				Version = json["version"].AsInt(LatestVersion);
				OriginalName = Name = json["name"].AsString();
				BaseClassName = json["base_class_name"].AsString(BaseMvcView.ClassName);
				IsInitial = json["is_initial"].AsBool(false);
				LifeType = (MvcLifeType)json["life_type"].AsInt();
				ViewRescaleMode = (MvcRescaleType)json["rescale_mode"].AsInt((int)MvcRescaleType.Default);
			}

			public JsonObject ToJson() {
				JsonObject json = new JsonObject();

				json["version"] = Version;
				json["name"] = Name;
				json["base_class_name"] = BaseClassName;
				json["is_initial"] = IsInitial;
				json["life_type"] = (int)LifeType;
				json["rescale_mode"] = (int)ViewRescaleMode;

				return json;
			}

			public override string ToString () {
				return ToJson().ToString();
			}
		}
	}
}
#endif