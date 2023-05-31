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
using Amilious.Core.Localization;
using Amilious.Core.Editor.Windows;
using UnityEditor.Experimental.GraphView;
using Amilious.Core.Editor.SearchProviders;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This drawer is used to display localized strings in the editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringDrawer : AmiliousPropertyDrawer {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private float _height;
        private bool _dropdown;
        private static bool _init;
        private static GUIStyle _editStyle;
        private static GUIStyle _lookUpStyle;
        private static GUIContent _editContent;
        private static GUIContent _lookUpContent;
        private static GUIStyle _translationStyle;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override float AmiliousGetPropertyHeight(SerializedProperty property, GUIContent label) {
            if(_dropdown) return _height + 25;
            return 20;
        }
        
        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Init();
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var valueRect = new Rect(position);
            valueRect.x += 15;
            valueRect.width -= 15;
            position.width -= 34;
            position.height = 18;
           
            var foldButtonRect = new Rect(position);
            foldButtonRect.x += 11;
            foldButtonRect.width = 15;

            _dropdown = EditorGUI.Foldout(foldButtonRect, _dropdown, "");

            position.x += 12;
            position.width -= 15;

            var key = property.FindPropertyRelative("key");
            key.stringValue = EditorGUI.TextField(position, key.stringValue);

            position.x += position.width+1;
            position.width = 17;
            position.height = 17;

            
            if(GUI.Button(position, _lookUpContent, _lookUpStyle)) {
                var popupPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                var searchProvider = ScriptableObject.CreateInstance<LocalizationSearchProvider>();
                searchProvider.Initialize(key);
                SearchWindow.Open(new SearchWindowContext(popupPosition), searchProvider);
            }
                

            position.x += position.width+1;


            if(GUI.Button(position,_editContent ,_editStyle))
                LocalizationEntryWindow.Open(key.stringValue);

            if(_dropdown) {
                var value = AmiliousLocalization.GetTranslation(key.stringValue);
                valueRect.width = EditorGUIUtility.currentViewWidth-37;
                valueRect.x = 18;
                _height = _translationStyle.CalcHeight(new GUIContent(value), valueRect.width);

                valueRect.height = _height;
                valueRect.y += 21;
                EditorGUI.LabelField(valueRect, value, _translationStyle);
            }
            
            EditorGUI.EndProperty();

        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the field values statically.
        /// </summary>
        private static void Init() {
            if(_init) return;
            _init = true; 
            _editContent = EditorGUIUtility.IconContent("editicon.sml");
            _editContent.tooltip = "Edit key's localization.";
            _editStyle = new GUIStyle(EditorStyles.miniButton) {
                padding = new RectOffset(2, 2, 2, 2)
            };
            _lookUpContent = EditorGUIUtility.IconContent("d_Search Icon");
            _lookUpContent.tooltip = "Look up localization key.";
            _lookUpStyle = new GUIStyle(EditorStyles.miniButton) {
                padding = new RectOffset(2, 2, 2, 2)
            };
            _translationStyle = new GUIStyle(EditorStyles.helpBox) {
                wordWrap = true,
                richText = true,
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.yellow }
            };
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}