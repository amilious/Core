/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amilious.Core.Collections {
    
    public interface IDoubleDictionary<TKey1, TKey2, TValue> : 
        IEnumerable<KeyValuePair<(TKey1, TKey2), TValue>> {
        
        #region Properies //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set the value for the given keys.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        TValue this[TKey1 key1, TKey2 key2] { get; set; }
        /// <summary>
        /// This property contains the count of all the values within the dictionary.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// This property contains a collection contains the first keys.
        /// </summary>
        ICollection<TKey1> Keys { get; }
        /// <summary>
        /// This property contains a collection contains all of the values within the dictionary.
        /// </summary>
        ICollection<TValue> AllValues { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the number of sub keys within the first key.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <returns>The number of sub-keys contains in the main key.</returns>
        int SubCount(TKey1 key1);
        /// <summary>
        /// This method is used to get a collection of the sub-keys for the given key.
        /// </summary>
        /// <param name="key1">The key that you want to get the sub-keys for.</param>
        /// <returns>The sub-keys for the given main key.</returns>
        ICollection<TKey2> SubKeys(TKey1 key1);
        /// <summary>
        /// This method is used to get a collection of the values for the given main key.
        /// </summary>
        /// <param name="key1">The main key.</param>
        /// <returns>A collection of the values contained within the given main key.</returns>
        ICollection<TValue> SubValues(TKey1 key1);
        /// <summary>
        /// This method is used to add a value to the dictionary.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        void Add(TKey1 key1, TKey2 key2, TValue value);
        /// <summary>
        /// This method is used to check if the dictionary contains a value for the given keys.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns>True if the dictionary contains a value for the given keys, otherwise false.</returns>
        bool ContainsKey(TKey1 key1, TKey2 key2);
        /// <summary>
        /// This method is used to check if the dictionary contains the given main key.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <returns>True if the dictionary contains the given main key, otherwise false.</returns>
        bool ContainsKey(TKey1 key1);
        /// <summary>
        /// This method is used to remove all of the values for the given main key.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <returns>True if values were removed, otherwise false.</returns>
        bool Remove(TKey1 key1);
        /// <summary>
        /// This method is used to remove the value for the given keys.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns>The third key.</returns>
        bool Remove(TKey1 key1, TKey2 key2);
        /// <summary>
        /// This method is used to try get the value for the given key.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if a value exists for the given key, otherwise false.</returns>
        bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value);
        /// <summary>
        /// This method is used to remove all of the values within the dictionary.
        /// </summary>
        void Clear();
        /// <summary>
        /// This Enumerator is used to get the values for the given key.
        /// </summary>
        /// <param name="key1">The main key.</param>
        /// <returns>The values within the given main key.</returns>
        public IEnumerator<TValue> GetValuesEnumerator(TKey1 key1);
        /// <summary>
        /// This method is used to remove the keys that contain the given value.
        /// </summary>
        /// <param name="value">The value that you want to remove.</param>
        /// <returns>True if any values were removed, otherwise false.</returns>
        public bool RemoveValue(TValue value);
        /// <summary>
        /// This method is used to remove the given value from the given first key.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if any values were removed, otherwise false.</returns>
        public bool RemoveValue(TKey1 key1, TValue value);
        /// <summary>
        /// This method is used to check if the given value is within the dictionary.
        /// </summary>
        /// <param name="value">The value that you want to check for.</param>
        /// <returns>True if the value exists within the dictionary, otherwise false.</returns>
        public bool ContainsValue(TValue value);

        /// <summary>
        /// This method is used to get all of the primary keys that contain the give secondary key.
        /// </summary>
        /// <param name="key2">The second key.</param>
        /// <returns>All of the primary keys that contain the secondary key.</returns>
        public IEnumerable<TKey1> GetPrimaryKeysForSecondary(TKey2 key2);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    public interface IReadOnlyDoubleDictionary<TKey1, TKey2, TValue> : 
        IEnumerable<KeyValuePair<(TKey1, TKey2), TValue>> {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the value for the given keys.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        TValue this[TKey1 key1, TKey2 key2] { get; }
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.Count"/>
        int Count { get; }
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.Keys"/>
        ICollection<TKey1> Keys { get; }
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.AllValues"/>
        ICollection<TValue> AllValues { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubCount(TKey1)"/>
        int SubCount(TKey1 key1);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubKeys(TKey1)"/>
        ICollection<TKey2> SubKeys(TKey1 key1);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubValues(TKey1)"/>
        ICollection<TValue> SubValues(TKey1 key1);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsKey(TKey1,TKey2)"/>
        bool ContainsKey(TKey1 key1, TKey2 key2);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsKey(TKey1)"/>
        bool ContainsKey(TKey1 key1);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.TryGetValue(TKey1,TKey2,out TValue)"/>
        bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsValue"/>
        public bool ContainsValue(TValue value);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.GetValuesEnumerator"/>
        public IEnumerator<TValue> GetValuesEnumerator(TKey1 key1);
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.GetPrimaryKeysForSecondary" />
        public IEnumerable<TKey1> GetPrimaryKeysForSecondary(TKey2 key2);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    [DataContract, Serializable]
    public class DoubleDictionary<TKey1, TKey2, TValue> : IDoubleDictionary<TKey1,TKey2,TValue>, 
        IReadOnlyDoubleDictionary<TKey1,TKey2,TValue>, ISerializable {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        [DataMember] private readonly Dictionary<(TKey1, TKey2), TValue> _dictionary;
        private readonly Dictionary<TKey1, HashSet<TKey2>> _subDictionaryKeys;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.this" />
        public TValue this[TKey1 key1, TKey2 key2] {
            get => _dictionary[(key1, key2)];
            set => _dictionary[(key1, key2)] = value;
        }

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.Count" />
        public int Count => _dictionary.Count;

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.Keys" />
        public ICollection<TKey1> Keys => _subDictionaryKeys.Keys;

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.AllValues" />
        public ICollection<TValue> AllValues => _dictionary.Values;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor.
        /// </summary>
        public DoubleDictionary() {
            _dictionary = new Dictionary<(TKey1, TKey2), TValue>();
            _subDictionaryKeys = new Dictionary<TKey1, HashSet<TKey2>>();
        }

        /// <summary>
        /// This constructor is only used for deserialization.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The serialization context.</param>
        protected DoubleDictionary(SerializationInfo info, StreamingContext context)
        {
            var primaryKeysCount = info.GetInt32("primaryKeys");
            _dictionary = new Dictionary<(TKey1, TKey2), TValue>();
            _subDictionaryKeys = new Dictionary<TKey1, HashSet<TKey2>>();

            for (var pKey = 0; pKey < primaryKeysCount; pKey++)
            {
                var key1 = (TKey1)info.GetValue($"pKey_{pKey}", typeof(TKey1));
                var subKeysCount = info.GetInt32($"$pKey_{pKey}_count");

                if (!_subDictionaryKeys.ContainsKey(key1))
                    _subDictionaryKeys[key1] = new HashSet<TKey2>();

                for (var sKey = 0; sKey < subKeysCount; sKey++)
                {
                    var key2 = (TKey2)info.GetValue($"pKey_{pKey}_sKey_{sKey}", typeof(TKey2));
                    var value = (TValue)info.GetValue($"pKey_{pKey}_sKey_{sKey}_value", typeof(TValue));

                    _dictionary[(key1, key2)] = value;
                    _subDictionaryKeys[key1].Add(key2);
                }
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubCount" />
        public int SubCount(TKey1 key1) =>
            _subDictionaryKeys.TryGetValue(key1, out var subKeys) ? subKeys.Count : 0;

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubKeys" />
        public ICollection<TKey2> SubKeys(TKey1 key1) =>
            _subDictionaryKeys.TryGetValue(key1, out var subKeys) ? subKeys : new List<TKey2>();

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.SubValues" />
        public ICollection<TValue> SubValues(TKey1 key1) {
            var subValues = new List<TValue>();
            if(!_subDictionaryKeys.TryGetValue(key1, out var subKeys)) return subValues;
            foreach (var key2 in subKeys) subValues.Add(_dictionary[(key1, key2)]);
            return subValues;
        }

        /// <inheritdoc />
        public void Add(TKey1 key1, TKey2 key2, TValue value) {
            _dictionary[(key1, key2)] = value;
            if (!_subDictionaryKeys.ContainsKey(key1))
                _subDictionaryKeys[key1] = new HashSet<TKey2>();
            if (!_subDictionaryKeys[key1].Contains(key2))
                _subDictionaryKeys[key1].Add(key2);
        }

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsKey(TKey1,TKey2)" />
        public bool ContainsKey(TKey1 key1, TKey2 key2) => _dictionary.ContainsKey((key1, key2));

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsKey(TKey1)" />
        public bool ContainsKey(TKey1 key1) => _subDictionaryKeys.ContainsKey(key1);

        /// <inheritdoc />
        public bool Remove(TKey1 key1) {
            if (!_subDictionaryKeys.TryGetValue(key1, out var subKeys)) return false;
            foreach (var key2 in subKeys) _dictionary.Remove((key1, key2));
            _subDictionaryKeys.Remove(key1);
            return true;
        }

        /// <inheritdoc />
        public bool Remove(TKey1 key1, TKey2 key2) {
            if(!_dictionary.Remove((key1, key2))) return false;
            if(!_subDictionaryKeys.TryGetValue(key1, out var subKeys)) return true;
            subKeys.Remove(key2);
            if (subKeys.Count == 0) _subDictionaryKeys.Remove(key1);
            return true;
        }

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.TryGetValue" />
        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value) =>
            _dictionary.TryGetValueFix((key1, key2), out value);

        /// <inheritdoc />
        public void Clear() {
            _dictionary.Clear();
            _subDictionaryKeys.Clear();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<(TKey1, TKey2), TValue>> GetEnumerator() {
            foreach (var kvp in _dictionary)yield return kvp;
        }

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.GetValuesEnumerator" />
        public IEnumerator<TValue> GetValuesEnumerator(TKey1 key1) {
            if(!_subDictionaryKeys.TryGetValue(key1, out var subKeys)) yield break;
            foreach (var key2 in subKeys) {
                if (_dictionary.TryGetValue((key1, key2), out var value)) yield return value;
            }
        }

        /// <summary>
        /// This method is used to merge the values from another dictionary into this one.
        /// </summary>
        /// <param name="otherDictionary">The other dictionary.</param>
        public void Merge(DoubleDictionary<TKey1, TKey2, TValue> otherDictionary) {
            foreach (var kvp in otherDictionary._dictionary) {
                Add(kvp.Key.Item1, kvp.Key.Item2, kvp.Value);
            }
        }

        /// <inheritdoc />
        public bool RemoveValue(TValue value) {
            var keysToRemove = new List<(TKey1, TKey2)>();
            foreach (var kvp in _dictionary) {
                if (EqualityComparer<TValue>.Default.Equals(kvp.Value, value))
                    keysToRemove.Add(kvp.Key);
            }
            foreach (var key in keysToRemove) {
                Remove(key.Item1, key.Item2);
            }
            return keysToRemove.Count > 0;
        }

        /// <inheritdoc />
        public bool RemoveValue(TKey1 key1, TValue value) {
            if (!_subDictionaryKeys.TryGetValue(key1, out var subKeys)) return false;
            var removed = false;
            foreach (var key2 in subKeys) {
                if(!_dictionary.TryGetValue((key1, key2), out var storedValue) ||
                    !EqualityComparer<TValue>.Default.Equals(storedValue, value)) continue;
                _dictionary.Remove((key1, key2));
                removed = true;
            }
            if(!removed) return false;
            subKeys.RemoveWhere(key2 => !_dictionary.ContainsKey((key1, key2)));
            if (subKeys.Count == 0) _subDictionaryKeys.Remove(key1);
            return true;
        }

        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.ContainsValue" />
        public bool ContainsValue(TValue value) => _dictionary.ContainsValue(value);

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("primaryKeys",_subDictionaryKeys.Count);
            var pKey = 0;
            foreach(var key1 in Keys) {
                info.AddValue($"pKey_{pKey}", key1);
                info.AddValue($"$pKey_{pKey}_count", SubKeys(key1));
                var sKey = 0;
                foreach(var key2 in SubKeys(key1)) {
                    info.AddValue($"pKey_{pKey}_sKey_{sKey}",key2);
                    info.AddValue($"pKey_{pKey}_sKey_{sKey}_value",this[key1,key2]);
                    sKey++;
                }
                pKey++;
            }
        }
        
        /// <inheritdoc cref="IDoubleDictionary{TKey1,TKey2,TValue}.GetPrimaryKeysForSecondary" />
        public IEnumerable<TKey1> GetPrimaryKeysForSecondary(TKey2 key2) {
            foreach(var set in _subDictionaryKeys)
                if(set.Value.Contains(key2)) yield return set.Key;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}
