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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Extensions;

namespace Amilious.Core.Editor.Modifiers {
    
    /// <summary>
    /// This modifier is used to display a warning message if an inspector field is left blank.
    /// </summary>
    [CustomPropertyDrawer(typeof(AmiRequiredAttribute))]
    public class RequiredModifier : AmiliousPropertyModifier<AmiRequiredAttribute> {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private AmiRequiredAttribute _attribute;
        private string _key;
        private static readonly Dictionary<string, bool> DisplayedWarning = new Dictionary<string, bool>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public AmiRequiredAttribute amiRequiredAttribute {
            get { return _attribute ??= (AmiRequiredAttribute)attribute; }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool CanCacheInspectorGUI(SerializedProperty property) => false;
        
        /// <inheritdoc />
        public override void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden, bool disabled) {
            if(ShowMessage(property)) {
                EditorGUILayout.HelpBox(amiRequiredAttribute.Message, MessageType.Error);
                if(HasDisplayedWarning()) return;
                SetDisplayedWarning(true);
                //select the property in the inspector
                GUI.FocusControl(property.name);
                //display warning
                Debug.LogWarningFormat("The {0} field is required but not present!\n{1}", property.name, 
                    amiRequiredAttribute.Message);
            }
            else SetDisplayedWarning(false);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        protected bool ShowMessage(SerializedProperty property) {
            if(property == null) return true;
            switch(property.propertyType) {
                case SerializedPropertyType.String: return string.IsNullOrWhiteSpace(property.stringValue);
                case SerializedPropertyType.ObjectReference: return property.objectReferenceValue == null;
                case SerializedPropertyType.Character: return string.IsNullOrEmpty(property.stringValue);
                case SerializedPropertyType.AnimationCurve: return property.animationCurveValue == null;
                #if UNITY_2021_2_OR_NEWER
                case SerializedPropertyType.ManagedReference: return property.managedReferenceValue == null;
                #else
                case SerializedPropertyType.ManagedReference: return property.objectReferenceValue == null;
                #endif
                default: return false;
            }
        }

        protected override void Initialize(SerializedProperty property) {
            _key = property.GetUniqueString();
        }

        private bool HasDisplayedWarning() {
            return DisplayedWarning.TryGetValueFix(_key, out var warn) && warn;
        }

        private void SetDisplayedWarning(bool displayed) {
            DisplayedWarning[_key] = displayed;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}