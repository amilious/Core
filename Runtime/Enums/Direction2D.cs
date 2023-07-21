using System;
using UnityEngine;

namespace Amilious.Core {

    [Serializable]
    public enum Direction2D {
        Up, Down, Left, Right
    }
    
    public static class Direction2DExtensions{

        public static Vector2 ToVector(this Direction2D direction) {
            return direction switch {
                Direction2D.Up => Vector2.up,
                Direction2D.Down => Vector2.down,
                Direction2D.Left => Vector2.left,
                Direction2D.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction2D GetOpposite(this Direction2D direction2D) {
            return direction2D switch {
                Direction2D.Up => Direction2D.Down,
                Direction2D.Down => Direction2D.Up,
                Direction2D.Left => Direction2D.Right,
                Direction2D.Right => Direction2D.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction2D), direction2D, null)
            };
        }

    }
    
}