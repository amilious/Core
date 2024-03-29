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
using System.Collections.Generic;
using System.Linq;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the <see cref="IDictionary{TKey,TValue}"/> class.
    /// </summary>
    public static class DictionaryExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method will try to get a value from the dictionary using the provided key then cast
        /// the object as the <see cref="value"/> type.
        /// </summary>
        /// <param name="dictionary">The dictionary the value is in.</param>
        /// <param name="key">The key for the value.</param>
        /// <param name="value">The casted value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>Ture if the value for the given key exists and can be
        /// cast to the provided type, otherwise returns false.</returns>
        public static bool TryGetCastValue<T>(this IDictionary<string, object> dictionary, string key, out T value) {
            if(dictionary == null) {
                value = default(T);
                return false;
            }
            if(dictionary.TryGetValueFix(key, out var dicValue)) {
                if(dicValue is T value1){ 
                    value = value1;
                    return true;
                } 
                try {
                    value = (T) Convert.ChangeType(dicValue, typeof(T));
                    return true;
                }catch(InvalidCastException) {}
            }
            value = default(T);
            return false;
        }
        
        #if UNITY_2019 || UNITY_2020
        
        /// <summary>
        /// This method is used to add the TryAdd method to a dictionary for older versions of C#
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The dictionary key.</param>
        /// <param name="value">The dictionary value.</param>
        /// <typeparam name="T">The dictionary key type.</typeparam>
        /// <typeparam name="T2">The dictionary value type.</typeparam>
        /// <returns>True if the key and value were added to the dictionary, otherwise returns false if
        /// the dictionary already contains the given key.</returns>
        public static bool TryAdd<T, T2>(this Dictionary<T, T2> dictionary, T key, T2 value) {
            if(dictionary.ContainsKey(key)) return false;
            dictionary[key] = value;
            return true;
        }
        
        #endif
        
        /// <summary>
        /// Uses a hacky way to TryGetValue on a dictionary when using IL2CPP and on mobile.
        /// This is to support older devices that don't properly handle IL2CPP builds.
        /// </summary>
        public static bool TryGetValueFix<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, 
            out TValue value) {
            #if ENABLE_IL2CPP && UNITY_IOS || UNITY_ANDROID
            if (dict.ContainsKey(key)){
                value = dict[key];
                return true;
            }
            value = default;
            return false;
            #else
            return dict.TryGetValue(key, out value);
            #endif
        }
        
        /// <summary>
        /// This method is used to get the previous value in a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the next value in.</param>
        /// <param name="currentKey">The current value.</param>
        /// <typeparam name="TKey">The dictionary key type.</typeparam>
        /// <typeparam name="TValue">The dictionary value type.</typeparam>
        /// <returns>The next item or the first item.</returns>
        public static TKey GetNext<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey currentKey) {
            var keys = dictionary.Keys.ToList();
            return keys.GetNext(currentKey);
        }

        /// <summary>
        /// This method is used to get the previous value in a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the next value in.</param>
        /// <param name="currentKey">The current value.</param>
        /// <param name="extraKeys">Extra values to include.</param>
        /// <typeparam name="TKey">The dictionary key type.</typeparam>
        /// <typeparam name="TValue">The dictionary value type.</typeparam>
        /// <returns>The next item or the first item.</returns>
        public static TKey GetNext<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, 
            TKey currentKey, params TKey[] extraKeys) {
            var keys = dictionary.Keys.ToList();
            return keys.GetNext(currentKey, extraKeys);
        }

        /// <summary>
        /// This method is used to get the previous value in a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the previous value in.</param>
        /// <param name="currentKey">The current value.</param>
        /// <typeparam name="TKey">The dictionary key type.</typeparam>
        /// <typeparam name="TValue">The dictionary value type.</typeparam>
        /// <returns>The previous item or the first item.</returns>
        public static TKey GetPrevious<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey currentKey) {
            var keys = dictionary.Keys.ToList();
            return keys.GetPrevious(currentKey);
        }

        /// <summary>
        /// This method is used to get the previous value in a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the previous value in.</param>
        /// <param name="currentKey">The current value.</param>
        /// <param name="extraKeys">Extra values to include.</param>
        /// <typeparam name="TKey">The dictionary key type.</typeparam>
        /// <typeparam name="TValue">The dictionary value type.</typeparam>
        /// <returns>The previous item or the first item.</returns>
        public static TKey GetPrevious<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, 
            TKey currentKey, params TKey[] extraKeys) {
            var keys = dictionary.Keys.ToList();
            return keys.GetPrevious(currentKey, extraKeys);
        }

        /// <summary>
        /// This method is used to check if the dictionary contains a value for the given keys.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if the dictionary contains the given key pair, otherwise false.</returns>
        public static bool ContainsKeys<TValue>(this IDictionary<long, TValue> dictionary, int key1, int key2) {
            return dictionary.ContainsKey(key1.PackWith(key2));
        }

        /// <summary>
        /// This method is used to try get the value for the given key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the value from.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if the value exists, otherwise false.</returns>
        public static bool TryGetValue<TValue>(this IDictionary<long, TValue> dictionary, int key1, int key2, out TValue value) {
            return dictionary.TryGetValueFix(key1.PackWith(key2), out value);
        }

        /// <summary>
        /// This method is used to add a new value to the dictionary with the give key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to add the value to.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>Returns true if the value was added, otherwise false if the key pair was already in use.</returns>
        public static bool Add<TValue>(this IDictionary<long, TValue> dictionary, int key1, int key2, TValue value) {
            var key = key1.PackWith(key2);
            if(dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            return true;
        }

        /// <summary>
        /// This method is used to remove an entry from a dictionary using the given key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to remove the value from.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if a value was removed, otherwise false.</returns>
        public static bool Remove<TValue>(this IDictionary<long, TValue> dictionary, int key1, int key2) {
            return dictionary.Remove(key1.PackWith(key2));
        }
        
        /// <summary>
        /// This method is used to check if the dictionary contains a value for the given keys.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if the dictionary contains the given key pair, otherwise false.</returns>
        public static bool ContainsKeys<TValue>(this IDictionary<ulong, TValue> dictionary, uint key1, uint key2) {
            return dictionary.ContainsKey(key1.PackWith(key2));
        }

        /// <summary>
        /// This method is used to try get the value for the given key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to get the value from.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if the value exists, otherwise false.</returns>
        public static bool TryGetValue<TValue>(this IDictionary<ulong, TValue> dictionary, uint key1, uint key2, out TValue value) {
            return dictionary.TryGetValueFix(key1.PackWith(key2), out value);
        }

        /// <summary>
        /// This method is used to add a new value to the dictionary with the give key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to add the value to.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>Returns true if the value was added, otherwise false if the key pair was already in use.</returns>
        public static bool Add<TValue>(this IDictionary<ulong, TValue> dictionary, uint key1, uint key2, TValue value) {
            var key = key1.PackWith(key2);
            if(dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            return true;
        }

        /// <summary>
        /// This method is used to remove an entry from a dictionary using the given key pair.
        /// </summary>
        /// <param name="dictionary">The dictionary that you want to remove the value from.</param>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>True if a value was removed, otherwise false.</returns>
        public static bool Remove<TValue>(this IDictionary<ulong, TValue> dictionary, uint key1, uint key2) {
            return dictionary.Remove(key1.PackWith(key2));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}