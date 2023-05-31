using UnityEngine.UIElements;

namespace Amilious.Core.Editor.Extensions {
    public static class VisualElementExtensions {
        
        /// <summary>
        /// This method is used to get elements by their given name.
        /// </summary>
        /// <param name="element">The element that you want to get the property for.</param>
        /// <param name="name">The property name.</param>
        /// <param name="field">The fields property.</param>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <returns>True if the element was found, otherwise false.</returns>
        public static bool Q<T>(this VisualElement element,string name, out T field) where T : VisualElement {
            field = element.Q<T>(name);
            return field != null;
        }
        
    }
}