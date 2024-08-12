using System;
using System.Collections;
using System.Collections.Generic;

namespace Strangeman.Utils
{
    public class BiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _keyToValue = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> _valueToKey = new Dictionary<TValue, TKey>();

        public void TryAdd(TKey key, TValue value)
        {
            if (!_keyToValue.TryAdd(key, value) || !_valueToKey.TryAdd(value, key))
            {
                throw new ArgumentException("Both key and value must be unique.");
            }
        }

        public bool TryGetValueByKey(TKey key, out TValue value)
        {
            return _keyToValue.TryGetValue(key, out value);
        }

        public bool TryGetKeyByValue(TValue value, out TKey key)
        {
            return _valueToKey.TryGetValue(value, out key);
        }

        public TValue GetValueByKey(TKey key)
        {
            if (_keyToValue.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        public TKey GetKeyByValue(TValue value)
        {
            if (_valueToKey.TryGetValue(value, out var key))
            {
                return key;
            }
            throw new KeyNotFoundException($"Value '{value}' not found.");
        }

        public bool RemoveByKey(TKey key)
        {
            if (_keyToValue.TryGetValue(key, out var value))
            {
                _keyToValue.Remove(key);
                _valueToKey.Remove(value);
                return true;
            }
            return false;
        }

        public bool RemoveByValue(TValue value)
        {
            if (_valueToKey.TryGetValue(value, out var key))
            {
                _valueToKey.Remove(value);
                _keyToValue.Remove(key);
                return true;
            }
            return false;
        }

        public int Count => _keyToValue.Count;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _keyToValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IReadOnlyDictionary<TKey, TValue> KeyToValue => _keyToValue;
        public IReadOnlyDictionary<TValue, TKey> ValueToKey => _valueToKey;
    }
}