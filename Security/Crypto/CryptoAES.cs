using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Renko.Diagnostics;

namespace Renko.Security
{
	public class CryptoAES : ICrypto {

		private byte[] passByte;


		public CryptoAES() {
			SetKey("passssssssssword");
		}

		public CryptoAES(string key) {
			SetKey(key);
		}

		/// <summary>
		/// The key used for encryption / decryption.
		/// </summary>
		public void SetKey(string key) {
			if(string.IsNullOrEmpty(key)) {
				RenLog.Log(LogLevel.Warning, "CryptoAES.SetKey - Key can't be null or empty.");
				return;
			}
			if(key.Length != 16) {
				RenLog.Log(LogLevel.Warning, "CryptoAES.SetKey - Key length must be 16.");
				return;
			}

			passByte = Encoding.UTF8.GetBytes(key);
		}

		/// <summary>
		/// Encrypts the given value.
		/// </summary>
		public string Encrypt(string value) {
			byte[] valueByte = Encoding.UTF8.GetBytes(value);

			using(Aes aes = Aes.Create()) {
				using (ICryptoTransform encryptor = aes.CreateEncryptor(passByte, passByte)) {
					var ms = new MemoryStream();
					using(Stream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
						cs.Write(valueByte, 0, valueByte.Length);
					}
					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		/// <summary>
		/// Decrypts the given value.
		/// </summary>
		public string Decrypt(string value) {
			byte[] valueByte = Convert.FromBase64String(value);

			using(Aes aes = Aes.Create()) {
				using (ICryptoTransform encryptor = aes.CreateDecryptor(passByte, passByte)) {
					var ms = new MemoryStream();
					using(Stream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
						cs.Write(valueByte, 0, valueByte.Length);
					}
					return Encoding.UTF8.GetString(ms.ToArray());
				}
			}
		}
	}
}