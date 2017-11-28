using UnityEngine;
using System.Collections;
using Renko.Security;
using Renko.Debug;

namespace Renko.Data
{
	/// <summary>
	/// A wrapper over Unity's PlayerPrefs with a small encryption feature.
	/// </summary>
	public static class SecurePrefs {

		/// <summary>
		/// The crypto method to use.
		/// Default encryptor/decryptor is MD5.
		/// </summary>
		private static ICrypto Crypto = new CryptoMD5();


		/// <summary>
		/// Specifies which crypto method to use.
		/// </summary>
		public static void SetCryptoMethod(ICrypto crypto) {
			if(crypto == null) {
				RenLog.Log(LogLevel.Warning, "SecurePrefs.SetCryptoMethod - Parameter 'crypto' may not be null!");
				return;
			}
			Crypto = crypto;
		}

		/// <summary>
		/// Specifies the key to use for encryption.
		/// Only use this if the specified crypto requires a key.
		/// </summary>
		public static void SetCryptoKey(string key) {
			Crypto.SetKey(key);
		}

		/// <summary>
		/// Sets an integer value to the specified key.
		/// </summary>
		public static void SetInt(string key, int val) {
			PlayerPrefs.SetString(key, GetEncryptedValue(val.ToString()));
		}

		/// <summary>
		/// Gets an integer value from the specified key.
		/// </summary>
		public static int GetInt(string key, int defaultValue = 0) {
			return GetDecryptedInt(PlayerPrefs.GetString(key, null), defaultValue);
		}

		/// <summary>
		/// Sets a boolean value to the specified key.
		/// </summary>
		public static void SetBool(string key, bool val) {
			PlayerPrefs.SetString(key, GetEncryptedValue(val ? "1" : "0"));
		}

		/// <summary>
		/// Gets a boolean value from the specified key.
		/// </summary>
		public static bool GetBool(string key, bool defaultValue = false) {
			return GetDecryptedBool(PlayerPrefs.GetString(key, null), defaultValue);
		}

		/// <summary>
		/// Sets a float value to the specified key.
		/// </summary>
		public static void SetFloat(string key, float val) {
			PlayerPrefs.SetString(key, GetEncryptedValue(val.ToString()));
		}

		/// <summary>
		/// Gets a floag value from the specified key.
		/// </summary>
		public static float GetFloat(string key, float defaultValue = 0f) {
			return GetDecryptedFloat(PlayerPrefs.GetString(key, null), defaultValue);
		}

		/// <summary>
		/// Sets a string value to the specified key.
		/// </summary>
		public static void SetString(string key, string val) {
			PlayerPrefs.SetString(key, GetEncryptedValue(val));
		}

		/// <summary>
		/// Gets a string value from the specified key.
		/// </summary>
		public static string GetString(string key, string defaultValue = null) {
			return GetDecryptedString(PlayerPrefs.GetString(key, null), defaultValue);
		}

		/// <summary>
		/// Returns an encrypted string from given value.
		/// </summary>
		private static string GetEncryptedValue(string value) {
			return Crypto.Encrypt(value);
		}

		/// <summary>
		/// Returns an integer value from an encrypted string.
		/// Will return a default value if failed.
		/// </summary>
		private static int GetDecryptedInt(string val, int defaultValue) {
			int i = defaultValue;
			if(!int.TryParse(Crypto.Decrypt(val), out i))
				i = defaultValue;
			return i;
		}

		/// <summary>
		/// Returns a boolean value from an encrypted string.
		/// Will return a default value if failed.
		/// </summary>
		private static bool GetDecryptedBool(string val, bool defaultValue) {
			int i = 0;
			if(!int.TryParse(Crypto.Decrypt(val), out i))
				i = (defaultValue ? 1 : 0);
			return i == 1;
		}

		/// <summary>
		/// Returns a float value from an encrypted string.
		/// Will return a default value if failed.
		/// </summary>
		private static float GetDecryptedFloat(string val, float defaultValue) {
			float i = defaultValue;
			if(!float.TryParse(Crypto.Decrypt(val), out i))
				i = defaultValue;
			return i;
		}

		/// <summary>
		/// Returns a string value from an encrypted string.
		/// Will return a default value if failed.
		/// </summary>
		private static string GetDecryptedString(string val, string defaultValue) {
			if(string.IsNullOrEmpty(val))
				return defaultValue;
			return Crypto.Decrypt(val);
		}
	}
}
