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

using UnityEditor;
using UnityEngine;
using Amilious.Core.IO;
using UnityEngine.UIElements;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="EditorWindow"/> class.
    /// </summary>
    public static class EditorWindowExtensions {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set
        /// </summary>
        /// <param name="window"></param>
        /// <param name="assetPath">The path of the asset.  Will get the uxml file with the same name as the class if
        /// the value is null.</param>
        /// <returns>True if the asset was cloned, otherwise false.</returns>
        public static bool CloneAssetTree(this EditorWindow window, string assetPath = null) {
            //get window type
            var targetType = window.GetType();
            //get asset path
            if(assetPath == null) {
                var monoScript = MonoScript.FromScriptableObject(window);
                assetPath = AssetDatabase.GetAssetPath(monoScript);
                assetPath = FileHelper.GetSiblingFile(assetPath, targetType.Name + ".uxml");
            }
            Debug.Log(assetPath);
            //get asset
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            if(asset == null) return false;
            asset.CloneTree(window.rootVisualElement);
            return true;
        }

        /// <summary>
        /// This method is used to get elements by their given name.
        /// </summary>
        /// <param name="window">The window that you want to get the property for.</param>
        /// <param name="name">The property name.</param>
        /// <param name="field">The fields property.</param>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <returns>True if the element was found, otherwise false.</returns>
        public static bool Q<T>(this EditorWindow window,string name, out T field) where T : VisualElement {
            field = window.rootVisualElement.Q<T>(name);
            return field != null;
        }

        /// <summary>
        /// This method is used to replace the content within the holder element with the given content.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="holderName">The name of the holder element.</param>
        /// <param name="content">The content that you want to replace the holder element contents with.</param>
        /// <returns>True if the holder element exists, otherwise false.</returns>
        public static bool ReplaceContent(this EditorWindow window, string holderName, VisualElement content) {
            var holder = window.rootVisualElement.Q<VisualElement>(holderName);
            if(holder == null) return false;
            holder.Clear();
            holder.Add(content);
            return true;
        }

        /// <summary>
        /// This method is used to replace the content within the holder element with the given content.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="holderName">The name of the holder element.</param>
        /// <param name="content">The content that you want to replace the holder element contents with.</param>
        /// <param name="holder">The holder element.</param>
        /// <returns>True if the holder element exists, otherwise false.</returns>
        public static bool ReplaceContent(this EditorWindow window, string holderName, VisualElement content,
            out VisualElement holder) {
            holder = window.rootVisualElement.Q<VisualElement>(holderName);
            if(holder == null) return false;
            holder.Clear();
            holder.Add(content);
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}