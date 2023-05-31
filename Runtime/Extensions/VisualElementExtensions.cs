using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.Core.Extensions {
    public static class VisualElementExtensions {

        public static Vector2 GetFullLocation(this VisualElement element) {
            var current = element;
            var pos = Vector2.zero;
            while(current != null) {
                pos += current.contentRect.position;
                current = current.parent;
            }
            return pos;
        }
        
    }
}