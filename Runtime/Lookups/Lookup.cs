﻿#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToolBox.Runtime.Lookups
{
	[Serializable]
	public abstract class Lookup<K, V> : ISerializationCallbackReceiver
	{
#if ODIN_INSPECTOR
		[DictionaryDrawerSettings(IsReadOnly = true), HideLabel]
#endif
		[SerializeField] private SerializedDictionary<K, V> _lookup = new SerializedDictionary<K, V>();
		[SerializeField, HideInInspector] private K[] _keys = new K[0];
		[SerializeField, HideInInspector] private V[] _values = new V[0];

		public IReadOnlyCollection<K> Keys => Array.AsReadOnly(_keys);
		public V[] Values => _values;

		public V Get(K key)
		{
			for (int i = 0; i < _keys.Length; i++)
			{
				if (EqualityComparer<K>.Default.Equals(key, _keys[i]))
					return _values[i];
			}

			return default;
		}

		public void Set(K key, V value)
		{
			for (int i = 0; i < _keys.Length; i++)
			{
				if (EqualityComparer<K>.Default.Equals(key, _keys[i]))
					_values[i] = value;
			}
		}

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			var keys = GetKeys();

			foreach (var key in keys)
			{
				if (!_lookup.ContainsKey(key))
					_lookup.Add(key, default);
			}

			Process(_lookup);

			_keys = _lookup.Keys.ToArray();
			_values = _lookup.Values.ToArray();
#endif
		}

		public void OnAfterDeserialize() { }

		protected abstract K[] GetKeys();

		protected abstract void Process(SerializedDictionary<K, V> lookup);
	}
}
