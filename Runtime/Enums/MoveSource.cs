using System;
using UnityEngine;

namespace Amilious.Core {

    [Serializable]
    public enum MoveSource { BottomLeft = 0, TopLeft = 1, TopRight = 2, BottomRight = 3, Standard =-1, Center = -2 }
    
    public static class PositionType2DExtensions {

        public static Vector2 ToVector(this MoveSource positionType) {
            switch (positionType) {
                case MoveSource.Standard:
                case MoveSource.Center:
                    return new Vector2(0.5f, 0.5f);
                case MoveSource.TopLeft:
                    return new Vector2(0, 1);
                case MoveSource.TopRight:
                    return new Vector2(1, 1);
                case MoveSource.BottomLeft:
                    return new Vector2(0, 0);
                case MoveSource.BottomRight:
                    return new Vector2(1, 0);
                default: throw new ArgumentOutOfRangeException(nameof(positionType), positionType, null);
            }
        }
        
    }
    
}