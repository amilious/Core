using System;
using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.Component {
    
    [Serializable]
    public enum ComponentEdge {
        None,
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
    
    /// <summary>
    /// Extension methods for the <see cref="ComponentEdge"/> enum.
    /// </summary>
    public static class ComponentEdgeExtensions{

        /// <summary>
        /// Determines whether the edge has a top side.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if the edge has a top side; otherwise, false.</returns>
        public static bool HasTop(this ComponentEdge edge) => edge is ComponentEdge.Top or 
            ComponentEdge.TopLeft or ComponentEdge.TopRight;
        
        /// <summary>
        /// Determines whether the edge has a bottom side.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if the edge has a bottom side; otherwise, false.</returns>
        public static bool HasBottom(this ComponentEdge edge) => edge is ComponentEdge.Bottom or 
            ComponentEdge.BottomLeft or ComponentEdge.BottomRight;
        
        /// <summary>
        /// Determines whether the edge has a right side.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if the edge has a right side; otherwise, false.</returns>
        public static bool HasRight(this ComponentEdge edge) => edge is ComponentEdge.Right or 
            ComponentEdge.BottomRight or ComponentEdge.TopRight;
        
        /// <summary>
        /// Determines whether the edge has a left side.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if the edge has a left side; otherwise, false.</returns>
        public static bool HasLeft(this ComponentEdge edge) => edge is ComponentEdge.Left or 
            ComponentEdge.TopLeft or ComponentEdge.BottomLeft;
        
        /// <summary>
        /// Gets the opposite edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The opposite edge.</returns>
        public static ComponentEdge GetOpposite(this ComponentEdge edge) {
            return edge switch {
                ComponentEdge.None => ComponentEdge.None,
                ComponentEdge.TopLeft => ComponentEdge.BottomRight,
                ComponentEdge.Top => ComponentEdge.Bottom,
                ComponentEdge.TopRight => ComponentEdge.BottomLeft,
                ComponentEdge.Left => ComponentEdge.Right,
                ComponentEdge.Right => ComponentEdge.Left,
                ComponentEdge.BottomLeft => ComponentEdge.TopRight,
                ComponentEdge.Bottom => ComponentEdge.Top,
                ComponentEdge.BottomRight => ComponentEdge.TopLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };
        }

        /// <summary>
        /// Gets the local position of the edge on a RectTransform.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The local position of the edge.</returns>
        public static Vector3 GetLocalPosition(this ComponentEdge edge, RectTransform transform) {
            return edge switch {
                ComponentEdge.None => transform.GetCornerLocalPosition(MoveSource.Center),
                ComponentEdge.TopLeft => transform.GetCornerLocalPosition(MoveSource.TopLeft),
                ComponentEdge.Top => transform.GetCornerLocalPosition(MoveSource.TopLeft),
                ComponentEdge.TopRight => transform.GetCornerLocalPosition(MoveSource.TopRight),
                ComponentEdge.Left => transform.GetCornerLocalPosition(MoveSource.TopLeft),
                ComponentEdge.Right => transform.GetCornerLocalPosition(MoveSource.TopRight),
                ComponentEdge.BottomLeft => transform.GetCornerLocalPosition(MoveSource.BottomLeft),
                ComponentEdge.Bottom => transform.GetCornerLocalPosition(MoveSource.BottomLeft),
                ComponentEdge.BottomRight => transform.GetCornerLocalPosition(MoveSource.BottomRight),
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };
        }

        /// <summary>
        /// Sets the local position of the edge on a RectTransform.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The local position.</param>
        public static void SetLocalPosition(this ComponentEdge edge, RectTransform transform, Vector3 position) {
            switch(edge) {
                case ComponentEdge.None: transform.SetCornerLocalPosition(MoveSource.Center,position); break;
                case ComponentEdge.TopLeft: transform.SetCornerLocalPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.Top: transform.SetCornerLocalPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.TopRight: transform.SetCornerLocalPosition(MoveSource.TopRight,position);  break;
                case ComponentEdge.Left: transform.SetCornerLocalPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.Right: transform.SetCornerLocalPosition(MoveSource.TopRight,position);  break;
                case ComponentEdge.BottomLeft: transform.SetCornerLocalPosition(MoveSource.BottomLeft,position);  break;
                case ComponentEdge.Bottom: transform.SetCornerLocalPosition(MoveSource.BottomLeft,position);  break;
                case ComponentEdge.BottomRight: transform.SetCornerLocalPosition(MoveSource.BottomRight,position);  break;
                default: throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }
        
        /// <summary>
        /// Gets the position of the edge on a RectTransform.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the edge.</returns>
        public static Vector3 GetPosition(this ComponentEdge edge, RectTransform transform) {
            return edge switch {
                ComponentEdge.None => transform.GetCornerPosition(MoveSource.Center),
                ComponentEdge.TopLeft => transform.GetCornerPosition(MoveSource.TopLeft),
                ComponentEdge.Top => transform.GetCornerPosition(MoveSource.TopLeft),
                ComponentEdge.TopRight => transform.GetCornerPosition(MoveSource.TopRight),
                ComponentEdge.Left => transform.GetCornerPosition(MoveSource.TopLeft),
                ComponentEdge.Right => transform.GetCornerPosition(MoveSource.TopRight),
                ComponentEdge.BottomLeft => transform.GetCornerPosition(MoveSource.BottomLeft),
                ComponentEdge.Bottom => transform.GetCornerPosition(MoveSource.BottomLeft),
                ComponentEdge.BottomRight => transform.GetCornerPosition(MoveSource.BottomRight),
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };
        }

        /// <summary>
        /// Sets the position of the edge on a RectTransform.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The position.</param>
        public static void SetPosition(this ComponentEdge edge, RectTransform transform, Vector3 position) {
            switch(edge) {
                case ComponentEdge.None: transform.SetCornerPosition(MoveSource.Center,position); break;
                case ComponentEdge.TopLeft: transform.SetCornerPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.Top: transform.SetCornerPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.TopRight: transform.SetCornerPosition(MoveSource.TopRight,position);  break;
                case ComponentEdge.Left: transform.SetCornerPosition(MoveSource.TopLeft,position);  break;
                case ComponentEdge.Right: transform.SetCornerPosition(MoveSource.TopRight,position);  break;
                case ComponentEdge.BottomLeft: transform.SetCornerPosition(MoveSource.BottomLeft,position);  break;
                case ComponentEdge.Bottom: transform.SetCornerPosition(MoveSource.BottomLeft,position);  break;
                case ComponentEdge.BottomRight: transform.SetCornerPosition(MoveSource.BottomRight,position);  break;
                default: throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }

    }
    
}