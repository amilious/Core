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

using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This drawer is used instead of the default color drawer.
    /// </summary>
    [CustomPropertyDrawer(typeof(bool))]
    [CustomPropertyDrawer(typeof(AmiBoolAttribute))]
    public class AmiliousBoolDrawer : AmiliousPropertyDrawer {

        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var att = property.GetAttributes<AmiBoolAttribute>()
                .FirstOrDefault()??new AmiBoolAttribute();
            property.boolValue = att.ToggleLeft ? 
                EditorGUI.ToggleLeft(position, label, property.boolValue) : 
                EditorGUI.Toggle(position, label, property.boolValue);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}