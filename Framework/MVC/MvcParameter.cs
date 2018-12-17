using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.MVCFramework
{
	/// <summary>
	/// A wrapper over C# dictionary for exclusive use in MVC framework.
	/// Because the dictionary uses the "object" class to store objects,
	/// the returned value's type must match the type it was stored with.
	/// For example: (float) value stored can't be retrieved as (double), but can only be done as (float).
	/// </summary>
	public class MvcParameter : Dictionary<string, object> {

		public new object this[string key]
		{
			get { return TryGetValueInternal(key); }
			set { Set(key, value); }
		}


		public T[] GetArray<T>(string key, T[] defaultValue = null) { return Get<T[]>(key, defaultValue); }

		public List<T> GetList<T>(string key, List<T> defaultValue = null) { return Get<List<T>>(key, defaultValue); }

		public string GetString(string key, string defaultValue = null) { return Get<string>(key, defaultValue); }

		public sbyte GetSbyte(string key, sbyte defaultValue = 0) { return GetValueType<sbyte>(key, defaultValue); }

		public byte GetByte(string key, byte defaultValue = 0) { return GetValueType<byte>(key, defaultValue); }

		public char GetChar(string key, char defaultValue = default(char)) { return GetValueType<char>(key, defaultValue); }

		public short GetShort(string key, short defaultValue = 0) { return GetValueType<short>(key, defaultValue); }

		public ushort GetUshort(string key, ushort defaultValue = 0) { return GetValueType<ushort>(key, defaultValue); }

		public int GetInt(string key, int defaultValue = 0) { return GetValueType<int>(key, defaultValue); }

		public uint GetUint(string key, uint defaultValue = 0) { return GetValueType<uint>(key, defaultValue); }

		public long GetLong(string key, long defaultValue = 0L) { return GetValueType<long>(key, defaultValue); }

		public ulong GetLong(string key, ulong defaultValue = 0L) { return GetValueType<ulong>(key, defaultValue); }

		public float GetFloat(string key, float defaultValue = 0f) { return GetValueType<float>(key, defaultValue); }

		public double GetDouble(string key, double defaultValue = 0d) { return GetValueType<double>(key, defaultValue); }

		public decimal GetDecimal(string key, decimal defaultValue = 0m) { return GetValueType<decimal>(key, defaultValue); }

		public bool GetBool(string key, bool defaultValue = false) { return GetValueType<bool>(key, defaultValue); }

		public Vector2 GetVector2(string key, Vector2 defaultValue = default(Vector2)) { return GetValueType<Vector2>(key, defaultValue); }

		public Vector3 GetVector3(string key, Vector3 defaultValue = default(Vector3)) { return GetValueType<Vector3>(key, defaultValue); }

		public Vector4 GetVector4(string key, Vector4 defaultValue = default(Vector4)) { return GetValueType<Vector4>(key, defaultValue); }

		public Quaternion GetQuaternion(string key, Quaternion defaultValue = default(Quaternion)) { return GetValueType<Quaternion>(key, defaultValue); }

		public T Get<T>(string key, T defaultValue = null) where T : class
		{
			object value = TryGetValueInternal(key);
			if(value != null)
				return value as T;
			return defaultValue;
		}

		public T GetValueType<T>(string key, T defaultValue = default(T)) where T : struct
		{
			object value = TryGetValueInternal(key);
			if(value.GetType() == typeof(T))
				return (T)value;
			return defaultValue;
		}

		public void Set(string key, object value)
		{
			if(ContainsKey(key))
				this[key] = value;
			else
				Add(key, value);
		}

		object TryGetValueInternal(string key)
		{
			object value = null;
			TryGetValue(key, out value);
			return value;
		}
	}
}

