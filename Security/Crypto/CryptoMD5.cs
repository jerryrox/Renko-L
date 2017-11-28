using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Renko.Debug;

namespace Renko.Security
{
	public class CryptoMD5 : ICrypto {
		
		private DESCryptoServiceProvider cryptoProvider;


		public CryptoMD5() {
			//Initialize
			InitializeProvider();
		}

		public CryptoMD5(string key) {
			//Initialize
			InitializeProvider();

			//Set the key
			SetKey(key);
		}

		/// <summary>
		/// The key used for encryption / decryption.
		/// </summary>
		public void SetKey(string key) {
			if(string.IsNullOrEmpty(key)) {
				RenLog.Log(LogLevel.Warning, "CryptoMD5.SetKey - Key can't be null or empty.");
				return;
			}
			cryptoProvider.Key = Encoding.UTF8.GetBytes(key);
		}

		/// <summary>
		/// Encrypts the given value.
		/// </summary>
		public string Encrypt(string value) {
			using (MemoryStream stream = new MemoryStream()) {
				using (CryptoStream cs = new CryptoStream(stream, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write)) {
					byte[] data = Encoding.UTF8.GetBytes(value);
					cs.Write(data, 0, data.Length);
					cs.FlushFinalBlock();
					return Convert.ToBase64String(stream.ToArray());
				}
			}
		}

		/// <summary>
		/// Decrypts the given value.
		/// </summary>
		public string Decrypt(string value) {
			using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(value))) {
				using (CryptoStream cs = new CryptoStream(stream, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read)) {
					using (StreamReader sr = new StreamReader(cs, Encoding.UTF8)) {
						return sr.ReadToEnd();
					}
				}
			}
		}

		/// <summary>
		/// Instantiates and initializes a new instance of des crypto service provider.
		/// </summary>
		void InitializeProvider() {
			cryptoProvider = new DESCryptoServiceProvider();
			cryptoProvider.Mode = CipherMode.ECB;
			cryptoProvider.Padding = PaddingMode.PKCS7;
		}
	}
}