using Renko.Debug;

namespace Renko.Security
{
	public class CryptoNone : ICrypto {
		
		/// <summary>
		/// The key used for encryption / decryption.
		/// </summary>
		public void SetKey(string key) {
			RenLog.Log(LogLevel.Info, "CryptoNone.SetKey - Key: " + key);
		}

		/// <summary>
		/// Encrypts the given value.
		/// </summary>
		public string Encrypt(string value) {
			RenLog.Log(LogLevel.Info, "CryptoNone.Encrypt - Value: " + value);
			return value;
		}

		/// <summary>
		/// Decrypts the given value.
		/// </summary>
		public string Decrypt(string value) {
			RenLog.Log(LogLevel.Info, "CryptoNone.Decrypt - Value: " + value);
			return value;
		}
	}
}