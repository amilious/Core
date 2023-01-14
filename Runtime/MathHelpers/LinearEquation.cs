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

namespace Amilious.Core.MathHelpers {
    
    /// <summary>
    /// This structure is used to represent a linear equation.
    /// </summary>
    public readonly struct LinearEquation {

        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The value represents the slope of the equation.
        /// </summary>
        /// ReSharper disable once MemberCanBePrivate.Global
        public readonly float M;

        /// <summary>
        /// The constant value that is added as the last part of the equation.
        /// </summary>
        /// ReSharper disable once MemberCanBePrivate.Global
        public readonly float B;

        /// <summary>
        /// This property is true 
        /// </summary>
        public readonly bool IsVertical;

        public readonly bool IsHorizontal;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to generate a linear equation from the two given points of the resulting line.
        /// </summary>
        /// <param name="pointA">The first point.</param>
        /// <param name="pointB">The second point.</param>
        public LinearEquation(Vector2 pointA, Vector2 pointB) {
            IsVertical = pointA.x == pointB.x;
            IsHorizontal = pointA.y == pointB.y;
            if(IsHorizontal || IsVertical) {
                M = 1;
                B = IsVertical ? pointA.x : pointA.y;
            }
            M = (pointB.y - pointA.y) / (pointB.x - pointA.x);
            B = pointA.y - (M * pointA.x);
        }

        /// <summary>
        /// This constructor is used to generate a linear equation from the given slope and constant value. y=mx+b
        /// </summary>
        /// <param name="m">The slope of the equation.</param>
        /// <param name="b">The constant that is added at the end of the equations.</param>
        public LinearEquation(float m, float b) {
            M = m;
            B = b;
            IsVertical = false;
            IsHorizontal = false;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method uses the equation to calculate the x value for, the given y value.
        /// </summary>
        /// <param name="y">The y value.</param>
        /// <param name="x">The resulting x value if it can be calculated.</param>
        /// <returns>True if the x value can be calculated, otherwise false.</returns>
        public bool TryFindX(float y, out float x) {
            if(IsHorizontal) { x = 0; return false; }
            x = IsVertical ? B : (y - B) / M;
            return true;
        }

        /// <summary>
        /// This property is used to return the point at the given y value;
        /// </summary>
        /// <param name="y">The given y value.</param>
        /// <param name="point">The point at the given y value.</param>
        /// <returns>True if the y value can be calculated, otherwise false.</returns>
        public bool FindPointFromY(float y, out Vector2 point) {
            var result = TryFindX(y, out var x);
            point = new Vector2(x, y);
            return result;
        }

        /// <summary>
        /// This method uses the equation to calculate the y value for, the given x value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The resulting y value if it can be calculated.</param>
        /// <returns>True if the y value can be calculated, otherwise false.</returns>
        public bool TryFindY(float x, out float y) {
            if(IsVertical) { y = 0; return false; }
            y = IsHorizontal ? B : (M * x) + B;
            return true;
        }

        /// <summary>
        /// This property is used to return the point at the given x value;
        /// </summary>
        /// <param name="x">The given y value.</param>
        /// <param name="point">The point at the given x value.</param>
        /// <returns>True if the x value can be calculated, otherwise false.</returns>
        public bool FindPointFromX(float x, out Vector2 point) {
            var result = TryFindY(x, out var y);
            point = new Vector2(x, y);
            return result;
        }
        /// <inheritdoc />
        public override string ToString() => IsVertical? $"x={B}" : IsHorizontal? $"y={B}" : $"y=({M})x+{B}";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}