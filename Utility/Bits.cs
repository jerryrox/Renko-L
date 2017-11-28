using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// A structure for bitwise operations using bytes.
	/// </summary>
	public struct Bits {
		
		/// <summary>
		/// The value.
		/// </summary>
		public int Value
		{
			get { return _value; }
			set { _value = value; }
		}
		private int _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="Renko.Utility.Bits"/> struct.
		/// </summary>
		public Bits(int b) { _value = b; }

		/// <summary>
		/// Returns whether this contains the given flag.
		/// </summary>
		public bool Contains (int flag)
		{
			return (_value & flag) != 0;
		}

		/// <summary>
		/// Returns a new Bits structure after xor-ing this.
		/// </summary>
		public Bits XOR (int value)
		{
			return new Bits(_value ^ value);
		}

		/// <summary>
		/// Returns a new Bits structure after or-ing this.
		/// </summary>
		public Bits OR (int value)
		{
			return new Bits(_value | value);
		}

		/// <summary>
		/// Returns a new Bits structure after and-ing this.
		/// </summary>
		public Bits AND (int value)
		{
			return new Bits(_value & value);
		}

		/// <summary>
		/// Returns a new Bits structure after shifting this by 'count' times to the left.
		/// </summary>
		public Bits ShiftLeft (int count)
		{
			return new Bits(_value << count);
		}

		/// <summary>
		/// Returns a new Bits structure after shifting this by 'count' times to the right.
		/// </summary>
		public Bits ShiftRight (int count)
		{
			return new Bits(_value >> count);
		}

		/// <summary>
		/// Returns a binary string representation of this value.
		/// </summary>
		public override string ToString ()
		{ return Convert.ToString(_value, 2); }


		#region Static helpers
		/// <summary>
		/// Returns a binary string representation of this value.
		/// </summary>
		public static string ToString(int value)
		{ return Convert.ToString(value, 2); }
		#endregion
	}
}