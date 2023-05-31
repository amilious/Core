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
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to IEnumerables.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly MethodInfo RawConvertMethod;
        
        private static readonly Dictionary<Type, MethodInfo> ConvertMethods = 
            new Dictionary<Type, MethodInfo>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This static constructor is triggered the first time another class uses this class.
        /// </summary>
        static IEnumerableExtensions() {
            RawConvertMethod = typeof(IEnumerableExtensions).GetMethod(nameof(ConvertToGeneric),
                BindingFlags.Static | BindingFlags.NonPublic);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to convert a collection of objects to a params array of the given type.
        /// </summary>
        /// <param name="collection">The collection of same type values.</param>
        /// <param name="type">The type of the values in the collection.</param>
        /// <returns>An object that is a converted array for the given type.</returns>
        public static object ConvertToParamsArray(this IEnumerable<object> collection, Type type) {
            if(ConvertMethods.TryGetValueFix(type, out var converter))
                return converter.Invoke(null, new object[] { collection });
            converter = RawConvertMethod.MakeGenericMethod(type);
            ConvertMethods[type] = converter;
            return converter.Invoke(null,new object[]{collection});
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the base method that will be made generic for the <see cref="ConvertToParamsArray"/> method.
        /// </summary>
        /// <param name="collection">The collection of same type objects that you want to convert into a params
        /// array.</param>
        /// <typeparam name="T">The type of the params array!</typeparam>
        /// <returns>The collection converted into a params array!</returns>
        private static T[] ConvertToGeneric<T>(IEnumerable<object> collection) => collection.Cast<T>().ToArray();

        /// <summary>
        /// This method is used to find the maximum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMaximum">The starting maximum value.</param>
        /// <returns>The maximum x y values in the collection.</returns>
        /// <seealso cref="GetMaxXY(IEnumerable{UnityEngine.Vector2})"/>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector2 GetMaxXY(this IEnumerable<Vector2> collection, Vector2 startingMaximum) {
            var result = startingMaximum;
            foreach(var item in collection) {
                if(item.x > result.x) result.x = item.x;
                if(item.y > result.y) result.y = item.y;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the maximum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The maximum x y values in the collection.</returns>
        /// <seealso cref="GetMaxXY(IEnumerable{UnityEngine.Vector2},UnityEngine.Vector2)"/>
        public static Vector2 GetMaxXY(this IEnumerable<Vector2> collection) =>
            collection.GetMaxXY(Vector2.one * float.MinValue);
        
        /// <summary>
        /// This method is used to find the minimum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMinimum">The starting minimum value.</param>
        /// <returns>The minimum x y values in the collection.</returns>
        /// <seealso cref="GetMinXY(IEnumerable{UnityEngine.Vector2})"/>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector2 GetMinXY(this IEnumerable<Vector2> collection, Vector2 startingMinimum) {
            var result = startingMinimum;
            foreach(var item in collection) {
                if(item.x < result.x) result.x = item.x;
                if(item.y < result.y) result.y = item.y;
            }
            return result;
        }
        
        /// <summary>
        /// This method is used to find the minimum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y values in the collection.</returns>
        /// <seealso cref="GetMinXY(IEnumerable{UnityEngine.Vector2},UnityEngine.Vector2)"/>
        public static Vector2 GetMinXY(this IEnumerable<Vector2> collection) =>
            collection.GetMinXY(Vector2.one * float.MinValue); 
        
        /// <summary>
        /// This method is used to find the maximum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMaximum">The starting maximum value.</param>
        /// <returns>The maximum x y values in the collection.</returns>
        /// <seealso cref="GetMaxXY(IEnumerable{UnityEngine.Vector2Int})"/>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector2Int GetMaxXY(this IEnumerable<Vector2Int> collection, Vector2Int startingMaximum) {
            var result = startingMaximum;
            foreach(var item in collection) {
                if(item.x > result.x) result.x = item.x;
                if(item.y > result.y) result.y = item.y;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the maximum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The maximum x y values in the collection.</returns>
        /// <seealso cref="GetMaxXY(IEnumerable{UnityEngine.Vector2Int},UnityEngine.Vector2Int)"/>
        public static Vector2Int GetMaxXY(this IEnumerable<Vector2Int> collection) =>
            collection.GetMaxXY(Vector2Int.one * int.MinValue);
        
        /// <summary>
        /// This method is used to find the minimum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMinimum">The starting minimum value.</param>
        /// <returns>The minimum x y values in the collection.</returns>
        /// <seealso cref="GetMinXY(IEnumerable{UnityEngine.Vector2Int})"/>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector2Int GetMinXY(this IEnumerable<Vector2Int> collection, Vector2Int startingMinimum) {
            var result = startingMinimum;
            foreach(var item in collection) {
                if(item.x < result.x) result.x = item.x;
                if(item.y < result.y) result.y = item.y;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the minimum x y values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y values in the collection.</returns>
        /// <seealso cref="GetMinXY(IEnumerable{UnityEngine.Vector2Int},UnityEngine.Vector2Int)"/>
        public static Vector2Int GetMinXY(this IEnumerable<Vector2Int> collection) =>
            collection.GetMinXY(Vector2Int.one * int.MinValue);
        
        /// <summary>
        /// This method is used to find the maximum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMaximum">The starting maximum value.</param>
        /// <returns>The maximum x y z values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector3 GetMaxXYZ(this IEnumerable<Vector3> collection, Vector3 startingMaximum) {
            var result = startingMaximum;
            foreach(var item in collection) {
                if(item.x > result.x) result.x = item.x;
                if(item.y > result.y) result.y = item.y;
                if(item.z > result.z) result.z = item.z;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the maximum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The maximum x y z values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector3 GetMaxXYZ(this IEnumerable<Vector3> collection) =>
            collection.GetMaxXYZ(Vector3.one * float.MinValue);
        
        /// <summary>
        /// This method is used to find the minimum x y z values in the collection.
        /// </summary>
        /// <param name="startingMinimum">The starting minimum value.</param>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y z values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector3 GetMinXYZ(this IEnumerable<Vector3> collection, Vector3 startingMinimum) {
            var result = startingMinimum;
            foreach(var item in collection) {
                if(item.x < result.x) result.x = item.x;
                if(item.y < result.y) result.y = item.y;
                if(item.z < result.z) result.z = item.z;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the minimum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y z values in the collection.</returns>
        public static Vector3 GetMinXYZ(this IEnumerable<Vector3> collection) =>
            collection.GetMinXYZ(Vector3.one * float.MinValue);
        
        /// <summary>
        /// This method is used to find the maximum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMaximum">The starting maximum value.</param>
        /// <returns>The maximum x y z values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector3Int GetMaxXYZ(this IEnumerable<Vector3Int> collection, Vector3Int startingMaximum) {
            var result = startingMaximum;
            foreach(var item in collection) {
                if(item.x > result.x) result.x = item.x;
                if(item.y > result.y) result.y = item.y;
                if(item.z > result.z) result.z = item.z;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the maximum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The maximum x y z values in the collection.</returns>
        public static Vector3Int GetMaxXYZ(this IEnumerable<Vector3Int> collection) =>
            collection.GetMaxXYZ(Vector3Int.one * int.MinValue);
        
        /// <summary>
        /// This method is used to find the minimum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// 
        /// <param name="startingMinimum">The starting minimum value.</param>
        /// <returns>The minimum x y z values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        public static Vector3Int GetMinXYZ(this IEnumerable<Vector3Int> collection, Vector3Int startingMinimum) {
            var result = startingMinimum;
            foreach(var item in collection) {
                if(item.x < result.x) result.x = item.x;
                if(item.y < result.y) result.y = item.y;
                if(item.z < result.z) result.z = item.z;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the minimum x y z values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y z values in the collection.</returns>
        public static Vector3Int GetMinXYZ(this IEnumerable<Vector3Int> collection) =>
            collection.GetMinXYZ(Vector3Int.one * int.MinValue);
        
        /// <summary>
        /// This method is used to find the maximum x y z w values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="startingMaximum">The starting maximum value.</param>
        /// <returns>The maximum x y z w values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        /// ReSharper disable once InconsistentNaming
        /// ReSharper disable once IdentifierTypo
        public static Vector4 GetMaxXYZW(this IEnumerable<Vector4> collection, Vector4 startingMaximum) {
            var result = startingMaximum;
            foreach(var item in collection) {
                if(item.x > result.x) result.x = item.x;
                if(item.y > result.y) result.y = item.y;
                if(item.z > result.z) result.z = item.z;
                if(item.w > result.w) result.w = item.w;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the maximum x y z w values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The maximum x y z w values in the collection.</returns>
        /// ReSharper disable once InconsistentNaming
        /// ReSharper disable once IdentifierTypo
        public static Vector4 GetMaxXYZW(this IEnumerable<Vector4> collection) =>
            collection.GetMaxXYZW(Vector4.one * float.MinValue);
        
        /// <summary>
        /// This method is used to find the minimum x y z w values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// 
        /// <param name="startingMinimum">The starting minimum value.</param>
        /// <returns>The minimum x y z w values in the collection.</returns>
        /// ReSharper disable once MemberCanBePrivate.Global
        /// ReSharper disable once InconsistentNaming
        /// ReSharper disable once IdentifierTypo
        public static Vector4 GetMinXYZW(this IEnumerable<Vector4> collection,Vector4 startingMinimum) {
            var result = startingMinimum;
            foreach(var item in collection) {
                if(item.x < result.x) result.x = item.x;
                if(item.y < result.y) result.y = item.y;
                if(item.z < result.z) result.z = item.z;
                if(item.w < result.w) result.w = item.w;
            }
            return result;
        }

        /// <summary>
        /// This method is used to find the minimum x y z w values in the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The minimum x y z w values in the collection.</returns>
        /// ReSharper disable once InconsistentNaming
        /// ReSharper disable once IdentifierTypo
        public static Vector4 GetMinXYZW(this IEnumerable<Vector4> collection) =>
            collection.GetMinXYZW(Vector4.one * float.MinValue);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}