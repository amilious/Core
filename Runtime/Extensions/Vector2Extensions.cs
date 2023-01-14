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
    /// This class is used to add methods to the <see cref="Vector2"/> struct.
    /// </summary>
    public static class Vector2Extensions {
        
         #region Public Methods ////////////////////////////////////////////////////////////////////////////////////////
         
        /// <summary>
        /// This method is used to convert a Vector2 into a SerializableVector2.
        /// </summary>
        /// <param name="vector2">The Vector2 that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector2.</returns>
        public static SerializableVector2 ToSerializable(this Vector2 vector2) {
            return new SerializableVector2(vector2);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector2"/>
        /// </summary>
        /// <param name="vector2">The <see cref="Vector2"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetValues(this Vector2 vector2, out float x, out float y) {
            x = vector2.x;
            y = vector2.y;
        }

        /// <summary>
        /// This method is used to get the width or the x value.
        /// </summary>
        /// <param name="vector2">The vector that you want to get the x or width value of.</param>
        /// <returns>The width or x value of the vector.</returns>
        public static float Width(this Vector2 vector2) => vector2.x;

        /// <summary>
        /// This method is used to set the width or the x value.
        /// </summary>
        /// <param name="vector2">The vector that you want to set the x or width value of.</param>
        /// <param name="width">The width or x value.</param>
        public static void Width(this Vector2 vector2, int width) => vector2.x = width;

        /// <summary>
        /// This method is used to get the height or the y value.
        /// </summary>
        /// <param name="vector2">The vector that you want to get the y or height value of.</param>
        /// <returns>The height or y value of the vector.</returns>
        public static float Height(this Vector2 vector2) => vector2.y;

        /// <summary>
        /// This method is used to set the height or the y value.
        /// </summary>
        /// <param name="vector2">The vector that you want to set the y or height value of.</param>
        /// <param name="height">The height or y value.</param>
        public static void Height(this Vector2 vector2, int height) => vector2.y = height;

        /// <summary>
        /// This method is used to check if a vector2 is within the box made from two vectors.
        /// </summary>
        /// <param name="checkPoint">The point you want to check for.</param>
        /// <param name="pointA">A corner of the area.</param>
        /// <param name="pointB">The corner diagonal from pointB</param>
        /// <returns>True if the point is within the given points.</returns>
        public static bool IsWithin(this Vector2 checkPoint, Vector2 pointA, Vector2 pointB) {
            var minPoint = Vector2.Min(pointA, pointB);
            var maxPoint = Vector2.Max(pointA, pointB);
            return (checkPoint.x >= minPoint.x && checkPoint.x <= maxPoint.x && checkPoint.y >= minPoint.y &&
                checkPoint.y <= maxPoint.y);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

     }
     
}