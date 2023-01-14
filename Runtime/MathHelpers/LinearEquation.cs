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
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to generate a linear equation from the two given points of the resulting line.
        /// </summary>
        /// <param name="pointA">The first point.</param>
        /// <param name="pointB">The second point.</param>
        public LinearEquation(Vector2 pointA, Vector2 pointB) {
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
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method uses the equation to calculate the x value for, the given y value.
        /// </summary>
        /// <param name="y">The y value.</param>
        /// <returns>The corresponding x value.</returns>
        public float FindX(float y) => (y - B) / M;

        /// <summary>
        /// This method uses the equation to calculate the y value from the given x value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <returns>The corresponding Y value.</returns>
        public float FindY(float x) => (M * x) + B;

        /// <inheritdoc />
        public override string ToString() => $"y=({M})x+{B}";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}