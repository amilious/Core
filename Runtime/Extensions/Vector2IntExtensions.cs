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
        /// This method is used to get the width or the x value.
        /// </summary>
        /// <param name="vector2Int">The vector that you want to get the x or width value of.</param>
        /// <returns>The width or x value of the vector.</returns>
        public static int Width(this Vector2Int vector2Int) => vector2Int.x;

        /// <summary>
        /// This method is used to set the width or the x value.
        /// </summary>
        /// <param name="vector2Int">The vector that you want to set the x or width value of.</param>
        /// <param name="width">The width or x value.</param>
        public static void Width(this Vector2Int vector2Int, int width) => vector2Int.x = width;

        /// <summary>
        /// This method is used to get the height or the y value.
        /// </summary>
        /// <param name="vector2Int">The vector that you want to get the y or height value of.</param>
        /// <returns>The height or y value of the vector.</returns>
        public static int Height(this Vector2Int vector2Int) => vector2Int.y;

        /// <summary>
        /// This method is used to set the height or the y value.
        /// </summary>
        /// <param name="vector2Int">The vector that you want to set the y or height value of.</param>
        /// <param name="height">The height or y value.</param>
        public static int Height(this Vector2Int vector2Int, int height) => vector2Int.y = height;
       
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}