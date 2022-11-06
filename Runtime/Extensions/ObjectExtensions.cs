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
using System.Reflection;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to objects.
    /// </summary>
    public static class ObjectExtensions {
        
        /// <summary>
        /// This method is used to try get the value of a field or property by name.
        /// </summary>
        /// <param name="obj">The object that contains the field or property.</param>
        /// <param name="name">The name of the field or property.</param>
        /// <param name="value">The value of the found field or property.</param>
        /// <typeparam name="T">The type of value of the field or property.</typeparam>
        /// <returns>True if the field or property exists and is of the given type.</returns>
        public static bool TryGetFieldOrPropertyValue<T>(this Object obj, string name, out T value) {
            //try load field value
            var field = obj.GetType().GetField(name,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if(field != null) {
                if(field.FieldType != typeof(T)) {
                    value = default;
                    return false;
                }
                value = (T)field.GetValue(obj);
                return true;
            }
            //try load property value
            var prop = obj.GetType().GetProperty(name,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if(prop != null) {
                if(prop.PropertyType != typeof(T)) {
                    value = default;
                    return false;
                }
                value = (T)prop.GetValue(obj);
                return true;
            }
            value = default;
            return false;
        }
        
    }
}