using System;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    public static class Vector4Extensions {

        /// <summary>
        /// Sets the value of the specified axis in the vector.
        /// </summary>
        /// <param name="vector">The vector to modify.</param>
        /// <param name="axis">The axis index (0 for X-axis, 1 for Y-axis, 2 for Z-axis, 3 for W-axis).</param>
        /// <param name="value">The new value for the axis.</param>
        /// <returns>The modified vector.</returns>
        public static Vector4 SetAxis(this Vector4 vector, int axis, float value) {
            if(axis == 0) vector.x = value;
            else if(axis == 1) vector.y = value;
            else if(axis == 2) vector.z = value;
            else if(axis == 3) vector.w = value;
            return vector;
        }
        
        /// <summary>
        /// Gets the value of the specified axis from the vector.
        /// </summary>
        /// <param name="vector">The vector to retrieve the value from.</param>
        /// <param name="axis">The axis index (0 for X-axis, 1 for Y-axis, 2 for Z-axis, 3 for W-axis).</param>
        /// <returns>The value of the specified axis.</returns>
        public static float GetAxis(this Vector4 vector, int axis) {
            if(axis == 0) return vector.x;
            if(axis == 1) return vector.y;
            if(axis == 2) return vector.z;
            if(axis == 3) return vector.w;
            throw new IndexOutOfRangeException("A Vector4 is a four-axis vector.");
        }
    }
}