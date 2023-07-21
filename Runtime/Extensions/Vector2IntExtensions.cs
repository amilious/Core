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
using Amilious.Core.Serializables;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="Vector2Int"/> struct.
    /// </summary>
    public static class Vector2IntExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to convert a Vector2Int into a SerializableVector2Int.
        /// </summary>
        /// <param name="vector2Int">The Vector2 that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector2Int.</returns>
        public static SerializableVector2Int ToSerializable(this Vector2Int vector2Int) {
            return new SerializableVector2Int(vector2Int);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector2Int"/>
        /// </summary>
        /// <param name="vector2Int">The <see cref="Vector2Int"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetValues(this Vector2Int vector2Int, out int x, out int y) {
            x = vector2Int.x;
            y = vector2Int.y;
        }
        
        /// <summary>
        /// Sets the value of the specified axis in the vector.
        /// </summary>
        /// <param name="vector">The vector to modify.</param>
        /// <param name="axis">The axis index (0 for X-axis, 1 for Y-axis).</param>
        /// <param name="value">The new value for the axis.</param>
        /// <returns>The modified vector.</returns>
        public static Vector2Int SetAxis(this Vector2Int vector, int axis, int value) {
            if(axis == 0) vector.x = value;
            else if(axis == 1) vector.y = value;
            return vector;
        }
        
        /// <summary>
        /// Gets the value of the specified axis from the vector.
        /// </summary>
        /// <param name="vector">The vector to retrieve the value from.</param>
        /// <param name="axis">The axis index (0 for X-axis, 1 for Y-axis).</param>
        /// <returns>The value of the specified axis.</returns>
        public static int GetAxis(this Vector2Int vector, int axis) {
            if(axis == 0) return vector.x;
            if(axis == 1) return vector.y;
            throw new IndexOutOfRangeException("A Vector2Int is a two-axis vector.");
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}