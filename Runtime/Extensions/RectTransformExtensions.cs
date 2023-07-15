
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    public static class RectTransformExtensions {
        
        public static bool TryClampUIToScreen(this RectTransform movable, RectTransform container, Vector3 position) {
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
        /// Sets the position of the bottom-left corner of the RectTransform to the specified world position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The desired world position of the bottom-left corner.</param>
        public static void SetBottomLeftPosition(this RectTransform transform, Vector3 position) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var offset = transform.position - corners[0];
            transform.position = position + offset;
        }
        
        /// <summary>
        /// Returns the position of the bottom-left corner of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the bottom-left corner in world space.</returns>
        public static Vector3 GetBottomLeftPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            return corners[0];
        }
        
        /// <summary>
        /// Sets the position of the top-left corner of the RectTransform to the specified world position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The desired world position of the top-left corner.</param>
        public static void SetTopLeftPosition(this RectTransform transform, Vector3 position) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var offset = transform.position - corners[1];
            transform.position = position + offset;
        }

        /// <summary>
        /// Returns the position of the top-left corner of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the top-left corner in world space.</returns>
        public static Vector3 GetTopLeftPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            return corners[1];
        }

        /// <summary>
        /// Sets the position of the top-right corner of the RectTransform to the specified world position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The desired world position of the top-right corner.</param>
        public static void SetTopRightPosition(this RectTransform transform, Vector3 position) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var offset = transform.position - corners[2];
            transform.position = position + offset;
        }
        
        /// <summary>
        /// Returns the position of the top-right corner of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the top-right corner in world space.</returns>
        public static Vector3 GetTopRightPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            return corners[2];
        }
        
        /// <summary>
        /// Sets the position of the bottom-right corner of the RectTransform to the specified world position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The desired world position of the bottom-right corner.</param>
        public static void SetBottomRightPosition(this RectTransform transform, Vector3 position) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var offset = transform.position - corners[3];
            transform.position = position + offset;
        }
        
        /// <summary>
        /// Returns the position of the bottom-right corner of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the bottom-right corner in world space.</returns>
        public static Vector3 GetBottomRightPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            return corners[3];
        }
        
        /// <summary>
        /// Sets the position of the center of the RectTransform to the specified world position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="position">The desired world position of the center.</param>
        public static void SetCenterPosition(this RectTransform transform, Vector3 position) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var centerOffset = corners[3] - corners[1];
            var offset = transform.position - corners[1];
            transform.position = position + offset + centerOffset;
        }
        
        /// <summary>
        /// Returns the position of the center of the RectTransform in world space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the center in world space.</returns>
        public static Vector3 GetCenterPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var centerOffset = corners[3] - corners[1];
            return corners[1] + centerOffset;
        }
       
        /// <summary>
        /// Sets the position of the bottom-left corner of the RectTransform to the specified local position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="localPosition">The desired local position of the bottom-left corner.</param>
        public static void SetBottomLeftLocalPosition(this RectTransform transform, Vector3 localPosition) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var offset = transform.position - corners[0];
            transform.position = localPosition + offset;
        }

        /// <summary>
        /// Returns the position of the bottom-left corner of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the bottom-left corner in local space.</returns>
        public static Vector3 GetBottomLeftLocalPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            return corners[0];
        }
        
        /// <summary>
        /// Sets the position of the top-left corner of the RectTransform to the specified local position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="localPosition">The desired local position of the top-left corner.</param>
        public static void SetTopLeftLocalPosition(this RectTransform transform, Vector3 localPosition) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var offset = transform.position - corners[1];
            transform.position = localPosition + offset;
        }

        /// <summary>
        /// Returns the position of the top-left corner of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the top-left corner in local space.</returns>
        public static Vector3 GetTopLeftLocalPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            return corners[1];
        }

        /// <summary>
        /// Sets the position of the top-right corner of the RectTransform to the specified local position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="localPosition">The desired local position of the top-right corner.</param>
        public static void SetTopRightLocalPosition(this RectTransform transform, Vector3 localPosition) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var offset = transform.position - corners[2];
            transform.position = localPosition + offset;
        }
        
        /// <summary>
        /// Returns the position of the top-right corner of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the top-right corner in local space.</returns>
        public static Vector3 GetTopRightLocalPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            return corners[2];
        }
        
        /// <summary>
        /// Sets the position of the bottom-right corner of the RectTransform to the specified local position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="localPosition">The desired local position of the bottom-right corner.</param>
        public static void SetBottomRightLocalPosition(this RectTransform transform, Vector3 localPosition) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var offset = transform.position - corners[3];
            transform.position = localPosition + offset;
        }
        
        /// <summary>
        /// Returns the position of the bottom-right corner of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the bottom-right corner in local space.</returns>
        public static Vector3 GetBottomRightLocalPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            return corners[3];
        }
        
        /// <summary>
        /// Sets the position of the center of the RectTransform to the specified local position.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <param name="localPosition">The desired world local of the center.</param>
        public static void SetCenterLocalPosition(this RectTransform transform, Vector3 localPosition) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var centerOffset = corners[3] - corners[1];
            var offset = transform.position - corners[1];
            transform.position = localPosition + offset + centerOffset;
        }
        
        /// <summary>
        /// Returns the position of the center of the RectTransform in local space.
        /// </summary>
        /// <param name="transform">The RectTransform.</param>
        /// <returns>The position of the center in local space.</returns>
        public static Vector3 GetCenterLocalPosition(this RectTransform transform) {
            var corners = new Vector3[4];
            transform.GetLocalCorners(corners);
            var centerOffset = corners[3] - corners[1];
            return corners[1] + centerOffset;
        }
        
    }
    
}