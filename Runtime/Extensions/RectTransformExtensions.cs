
using System;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    public static class RectTransformExtensions {
        
        /// <summary>
        /// Tries to clamp the position of a UI element within the boundaries of a container RectTransform.
        /// </summary>
        /// <param name="movable">The RectTransform of the UI element to clamp.</param>
        /// <param name="container">The RectTransform of the container.</param>
        /// <param name="position">The desired position of the UI element.</param>
        /// <returns>True if the UI element was successfully clamped within the container; otherwise, false.</returns>
        public static bool TryClampUIToScreen(this RectTransform movable, RectTransform container, 
            Vector3 position) {
            movable.position = position;
            if(container == null) return false;
            var cornersCache = new Vector3[4];
            container.GetWorldCorners(cornersCache);
            Vector3 containerBL = cornersCache[0], containerTR = cornersCache[2];
            var containerSize = containerTR - containerBL;
            movable.GetWorldCorners(cornersCache);
            Vector3 movableBL = cornersCache[0], movableTR = cornersCache[2];
            var movableSize = movableTR - movableBL;
            Vector3 deltaBL = position - movableBL, deltaTR = movableTR - position;
            position.x = movableSize.x < containerSize.x
                ? Mathf.Clamp(position.x, containerBL.x + deltaBL.x, containerTR.x - deltaTR.x)
                : Mathf.Clamp(position.x, containerTR.x - deltaTR.x, containerBL.x + deltaBL.x);
            position.y = movableSize.y < containerSize.y
                ? Mathf.Clamp(position.y, containerBL.y + deltaBL.y, containerTR.y - deltaTR.y)
                : Mathf.Clamp(position.y, containerTR.y - deltaTR.y, containerBL.y + deltaBL.y);
            movable.position = position;
            return true;
        }

        /// <summary>
        /// This method is used to clam a position to the given rect transform area.
        /// </summary>
        /// <param name="transform">The transform that you want to bind the point to.</param>
        /// <param name="point">The position that you want to clamp to the transform.</param>
        /// <returns>The clamped point.</returns>
        public static Vector2 ClampToRect(this RectTransform transform, Vector2 point) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            point.x = Mathf.Clamp(point.x, corners[1].x, corners[3].x);
            point.y = Mathf.Clamp(point.y, corners[3].y, corners[1].y);
            return point;
        }
        
        /// <summary>
        /// Rotates a RectTransform around a specified corner by a given number of degrees.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to rotate.</param>
        /// <param name="corner">The corner to rotate around.</param>
        /// <param name="degrees">The number of degrees to rotate.</param>
        public static void RotateAroundCorner(this RectTransform rectTransform, MoveSource corner, float degrees) {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            switch(corner) {
                case MoveSource.Center: {
                    var center = corners[0] + (corners[2] - corners[0]) * .5f;
                    rectTransform.RotateAround(center, Vector3.forward, degrees);
                    return;
                }
                case MoveSource.Standard: {
                    rectTransform.Rotate(0, 0, degrees); return;
                }
                default: rectTransform.RotateAround(corners[(int)corner], Vector3.forward, degrees); return;
            }
        }

        /// <summary>
        /// Sets the rotation of a RectTransform around a specified corner to a given number of degrees.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to rotate.</param>
        /// <param name="corner">The corner to set the rotation for.</param>
        /// <param name="degrees">The number of degrees to set the rotation to.</param>
        public static void SetCornerRotation(this RectTransform rectTransform, MoveSource corner, float degrees) {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            //adjust for the current rotation
            degrees = (degrees + (360 - rectTransform.eulerAngles.z))%360;
            if(degrees < 0) degrees += 360;
            switch(corner) {
                case MoveSource.Center: {
                    var center = corners[0] + (corners[2] - corners[0]) * .5f;
                    rectTransform.RotateAround(center, Vector3.forward, degrees);
                    return;
                }
                case MoveSource.Standard: {
                    rectTransform.Rotate(0, 0, degrees); return;
                }
                default: rectTransform.RotateAround(corners[(int)corner], Vector3.forward, degrees); return;
            }
        }
        
        /// <summary>
        /// Moves the RectTransform such that the specified corner is at the specified position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="corner">The corner to move to the specified position.</param>
        /// <param name="position">The desired world position of the corner.</param>
        public static void SetCornerPosition(this RectTransform transform, MoveSource corner, 
            Vector3 position) {
            if(corner == MoveSource.Standard){ transform.position = position; return; }
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            if(corner == MoveSource.Center) {
                var offset = transform.position - (corners[0] + (corners[2] - corners[0]) * .5f);
                transform.position = position + offset;
            } else {
                var offset = transform.position - corners[(int)corner];
                transform.position = position + offset;
            }
        }

        /// <summary>
        /// Moves the RectTransform such that the specified corner is at the specified position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="corner">The corner to move to the specified position.</param>
        /// <param name="position">The desired world position of the corner.</param>
        public static void SetCornerLocalPosition(this RectTransform transform, MoveSource corner, 
            Vector3 position) {
            if(corner == MoveSource.Standard){ transform.localPosition = position; return; }
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            if(corner == MoveSource.Center) {
                var offset = transform.localPosition - (corners[0] + (corners[2] - corners[0]) * .5f);
                transform.localPosition = position + offset;
            } else {
                var offset = transform.localPosition - corners[(int)corner];
                transform.localPosition = position + offset;
            }
        }
        
        /// <summary>
        /// Returns the position of the specified corner of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="corner">The corner whose position to return.</param>
        /// <returns>The position of the specified corner in world space.</returns>
        public static Vector3 GetCornerPosition(this RectTransform transform, MoveSource corner) {
            if(corner == MoveSource.Standard) return transform.position;
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            if(corner == MoveSource.Center) return corners[0] + (corners[2] - corners[0]) * .5f;
            return corners[(int)corner];
        }

        /// <summary>
        /// Returns the position of the specified corner of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="corner">The corner whose position to return.</param>
        /// <returns>The position of the specified corner in local space.</returns>
        public static Vector3 GetCornerLocalPosition(this RectTransform transform, MoveSource corner) {
            if(corner == MoveSource.Standard) return transform.localPosition;
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            if(corner == MoveSource.Center) return corners[0] + (corners[2] - corners[0]) * .5f;
            return corners[(int)corner];
        }

        public static Vector3 GetOffScreenPosition(this RectTransform transform, Direction2D direction, 
            RectTransform container) {
            var conCorners = new Vector3[4];
            container.GetWorldCorners(conCorners);
            var tCorners = new Vector3[4];
            transform.GetWorldCorners(tCorners);
            var position = Vector3.zero;
            var offset = Vector3.zero;
            switch(direction) {
                case Direction2D.Up: //set rect bottom left corner above con top left corner
                    position = tCorners[0];
                    offset = transform.position - position;
                    position.y = conCorners[1].y;
                    return position + offset;
                case Direction2D.Down: //set the top left corner bellow con bottom left corner
                    position = tCorners[1];
                    offset = transform.position - position;
                    position.y = conCorners[0].y;
                    return position + offset;
                case Direction2D.Left: //set the top right corner to con top left corner
                    position = tCorners[2];
                    offset = transform.position - position;
                    position.x = conCorners[1].x;
                    return position + offset;
                case Direction2D.Right: //set the top left to con top right
                    position = tCorners[1];
                    offset = transform.position - position;
                    position.x = conCorners[2].x;
                    return position + offset;
                default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
        
    }
    
}