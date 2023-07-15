using System;
using Amilious.Core.Extensions;
using UnityEngine;

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
    
    public static class ComponentEdgeExtensions{

        public static bool HasTop(this ComponentEdge edge) => edge is ComponentEdge.Top or 
            ComponentEdge.TopLeft or ComponentEdge.TopRight;
        
        public static bool HasBottom(this ComponentEdge edge) => edge is ComponentEdge.Bottom or 
            ComponentEdge.BottomLeft or ComponentEdge.BottomRight;
        
        public static bool HasRight(this ComponentEdge edge) => edge is ComponentEdge.Right or 
            ComponentEdge.BottomRight or ComponentEdge.TopRight;
        
        public static bool HasLeft(this ComponentEdge edge) => edge is ComponentEdge.Left or 
            ComponentEdge.TopLeft or ComponentEdge.BottomLeft;
        
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

        public static Vector3 GetLocalPosition(this ComponentEdge edge, RectTransform transform) {
            return edge switch {
                ComponentEdge.None => transform.GetCenterLocalPosition(),
                ComponentEdge.TopLeft => transform.GetTopLeftLocalPosition(),
                ComponentEdge.Top => transform.GetTopLeftLocalPosition(),
                ComponentEdge.TopRight => transform.GetTopRightLocalPosition(),
                ComponentEdge.Left => transform.GetTopLeftLocalPosition(),
                ComponentEdge.Right => transform.GetTopRightLocalPosition(),
                ComponentEdge.BottomLeft => transform.GetBottomLeftLocalPosition(),
                ComponentEdge.Bottom => transform.GetBottomLeftLocalPosition(),
                ComponentEdge.BottomRight => transform.GetBottomRightLocalPosition(),
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };
        }

        public static void SetLocalPosition(this ComponentEdge edge, RectTransform transform, Vector3 localPosition) {
            switch(edge) {
                case ComponentEdge.None: transform.SetCenterLocalPosition(localPosition); break;
                case ComponentEdge.TopLeft: transform.SetTopLeftLocalPosition(localPosition); break;
                case ComponentEdge.Top: transform.SetTopLeftLocalPosition(localPosition); break;
                case ComponentEdge.TopRight: transform.SetTopRightLocalPosition(localPosition); break;
                case ComponentEdge.Left: transform.SetTopLeftLocalPosition(localPosition); break;
                case ComponentEdge.Right: transform.SetTopRightLocalPosition(localPosition); break;
                case ComponentEdge.BottomLeft: transform.SetBottomLeftLocalPosition(localPosition); break;
                case ComponentEdge.Bottom: transform.SetBottomLeftLocalPosition(localPosition); break;
                case ComponentEdge.BottomRight: transform.SetBottomRightLocalPosition(localPosition); break; default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }
        
        public static Vector3 GetPosition(this ComponentEdge edge, RectTransform transform) {
            return edge switch {
                ComponentEdge.None => transform.GetCenterPosition(),
                ComponentEdge.TopLeft => transform.GetTopLeftPosition(),
                ComponentEdge.Top => transform.GetTopLeftPosition(),
                ComponentEdge.TopRight => transform.GetTopRightPosition(),
                ComponentEdge.Left => transform.GetTopLeftPosition(),
                ComponentEdge.Right => transform.GetTopRightPosition(),
                ComponentEdge.BottomLeft => transform.GetBottomLeftPosition(),
                ComponentEdge.Bottom => transform.GetBottomLeftPosition(),
                ComponentEdge.BottomRight => transform.GetBottomRightPosition(),
                _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
            };
        }

        public static void SetPosition(this ComponentEdge edge, RectTransform transform, Vector3 position) {
            switch(edge) {
                case ComponentEdge.None: transform.SetCenterPosition(position); break;
                case ComponentEdge.TopLeft: transform.SetTopLeftPosition(position); break;
                case ComponentEdge.Top: transform.SetTopLeftPosition(position); break;
                case ComponentEdge.TopRight: transform.SetTopRightPosition(position); break;
                case ComponentEdge.Left: transform.SetTopLeftPosition(position); break;
                case ComponentEdge.Right: transform.SetTopRightPosition(position); break;
                case ComponentEdge.BottomLeft: transform.SetBottomLeftPosition(position); break;
                case ComponentEdge.Bottom: transform.SetBottomLeftPosition(position); break;
                case ComponentEdge.BottomRight: transform.SetBottomRightPosition(position); break; 
                default: throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }

    }
    
}