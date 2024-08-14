using System;
using System.Collections;
using System.Collections.Generic;

namespace Strangeman.Utils
{
    /// <summary>
    /// Represents a bidirectional dictionary that maps keys to values and values to keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class BiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _keyToValue = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> _valueToKey = new Dictionary<TValue, TKey>();

        /// <summary>
        /// Attempts to add a key-value pair to the dictionary. Throws an exception if either the key or value is already present.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentException">Thrown if the key or value already exists in the dictionary.</exception>
        public void TryAdd(TKey key, TValue value)
        {
            if (!_keyToValue.TryAdd(key, value) || !_valueToKey.TryAdd(value, key))
            {
                throw new ArgumentException("Both key and value must be unique.");
            }
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <param name="value">The value associated with the key, if found.</param>
        /// <returns>True if the key was found; otherwise, false.</returns>
        public bool TryGetValueByKey(TKey key, out TValue value)
        {
            return _keyToValue.TryGetValue(key, out value);
        }

        /// <summary>
        /// Attempts to get the key associated with the specified value.
        /// </summary>
        /// <param name="value">The value whose key to retrieve.</param>
        /// <param name="key">The key associated with the value, if found.</param>
        /// <returns>True if the value was found; otherwise, false.</returns>
        public bool TryGetKeyByValue(TValue value, out TKey key)
        {
            return _valueToKey.TryGetValue(value, out key);
        }

        /// <summary>
        /// Retrieves the value associated with the specified key. Throws an exception if the key is not found.
        /// </summary>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <returns>The value associated with the key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key is not found.</exception>
        public TValue GetValueByKey(TKey key)
        {
            if (_keyToValue.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        /// <summary>
        /// Retrieves the key associated with the specified value. Throws an exception if the value is not found.
        /// </summary>
        /// <param name="value">The value whose key to retrieve.</param>
        /// <returns>The key associated with the value.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the value is not found.</exception>
        public TKey GetKeyByValue(TValue value)
        {
            if (_valueToKey.TryGetValue(value, out var key))
            {
                return key;
            }
            throw new KeyNotFoundException($"Value '{value}' not found.");
        }

        /// <summary>
        /// Removes the entry associated with the specified key. Returns true if successful, otherwise false.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was removed; otherwise, false.</returns>
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

        /// <summary>
        /// Removes the entry associated with the specified value. Returns true if successful, otherwise false.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if the value was removed; otherwise, false.</returns>
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

        /// <summary>
        /// Gets the number of key-value pairs in the dictionary.
        /// </summary>
        public int Count => _keyToValue.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs in the dictionary.
        /// </summary>
        /// <returns>An enumerator for the dictionary.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _keyToValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets a read-only dictionary that provides access to the key-value mappings.
        /// </summary>
        public IReadOnlyDictionary<TKey, TValue> KeyToValue => _keyToValue;

        /// <summary>
        /// Gets a read-only dictionary that provides access to the value-key mappings.
        /// </summary>
        public IReadOnlyDictionary<TValue, TKey> ValueToKey => _valueToKey;
    }
}
