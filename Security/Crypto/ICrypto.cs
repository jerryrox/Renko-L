using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Security
{
	/// <summary>
	/// An interface that provides generic interaction with crypto objects.
	/// </summary>
	public interface ICrypto {

		/// <summary>
		/// The key used for encryption / decryption.
		/// </summary>
		void SetKey(string key);

		/// <summary>
		/// Encrypts the given value with specific crypto method.
		/// </summary>
		string Encrypt(string value);

		/// <summary>
		/// Decrypts the given value with specific crypto method.
		/// </summary>
		string Decrypt(string value);
	}
}