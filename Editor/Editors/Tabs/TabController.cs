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
using Amilious.Core.Extensions;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Editors.Tabs {
    
    /// <summary>
    /// This class is used to handle drawing tabs
    /// </summary>
    public class TabController {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        private readonly AmiliousEditor _editor;
        private readonly GUIStyle _tabBoxStyle;
        private readonly GUIContent _nextContent;
        private readonly GUIContent _dropdownContent;
        private readonly GUIContent _previousContent;
        private readonly float _nextWidth;
        private readonly float _previousWidth;
        private readonly float _dropdownWidth;
        private readonly HashSet<string> _drawnTabs = new HashSet<string>();
        private readonly Dictionary<string, TabGroup> _tabGroups = new Dictionary<string, TabGroup>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the number of tab groups.
        /// </summary>
        public int Count => _tabGroups.Count;

        /// <summary>
        /// This property is used to get the tab group with the given name.
        /// </summary>
        /// <param name="tabGroupName">The name of the tab group.</param>
        public TabGroup this[string tabGroupName] =>
            _tabGroups.TryGetValueFix(tabGroupName, out var group) ? group : null;

        /// <summary>
        /// This property is used to get the tab group with the given index.
        /// </summary>
        /// <param name="groupIndex">The index of the tab group.</param>
        public TabGroup this[int groupIndex] => _tabGroups.ElementAt(groupIndex).Value;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new tab controller.
        /// </summary>
        /// <param name="editor">The amilious editor that is using the tab controller.</param>
        public TabController(AmiliousEditor editor) {
            _editor = editor;
            _tabBoxStyle = new GUIStyle(EditorStyles.helpBox) { margin = new RectOffset(50,5,5,5) };
            
            _previousContent = new GUIContent("<");
            _previousWidth = Tab.Style.CalcSize(_previousContent).x;
            
            _nextContent = new GUIContent(">");
            _nextWidth = Tab.Style.CalcSize(_nextContent).x;
            
            _dropdownContent = new GUIContent("...");
            _dropdownWidth = Tab.Style.CalcSize(_dropdownContent).x;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to reset the tabs and tab groups.
        /// </summary>
        public void Reset() => _tabGroups.Clear();

        /// <summary>
        /// This method is used to reset the drawn tabs before drawing.
        /// </summary>
        public void ClearDraw() => _drawnTabs?.Clear();

        /// <summary>
        /// This method is used to add a serialized property to a tab.
        /// </summary>
        /// <param name="tabProperty">The tab info.</param>
        public void AddToGroup(TabProperty tabProperty) {
            if(!_tabGroups.ContainsKey(tabProperty.TabGroup))
                _tabGroups[tabProperty.TabGroup] = new TabGroup(tabProperty.TabGroup,_editor.serializedObject);
            _tabGroups[tabProperty.TabGroup].TryAddProperty(tabProperty);
            _editor.SkipPropertyDraw(tabProperty.Property);
        }
        
        /// <summary>
        /// This method is used to add the property and its attribute to the appropriate tab.
        /// </summary>
        /// <param name="attribute">The tab attribute.</param>
        /// <param name="property">The property that you want to add.</param>
        public void AddToGroup(AmiTabAttribute attribute, SerializedProperty property) {
            var tabProperty = new TabProperty(attribute, property);
            AddToGroup(tabProperty);
        }

        /// <summary>
        /// This method is used to add the property and its attribute to the appropriate tab.
        /// </summary>
        /// <param name="attribute">The tab attribute.</param>
        /// <param name="buttonAttribute">The button attribute.</param>
        public void AddToGroup(AmiTabAttribute attribute, AmiButtonAttribute buttonAttribute) {
            var tabProperty = new TabProperty(attribute, buttonAttribute);
            AddToGroup(tabProperty);
        }

        /// <summary>
        /// This method is used to add a property to a tab group.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="tabName">The name of the tab.</param>
        /// <param name="property">The property that you want to add.</param>
        /// <param name="order">The property order.</param>
        public void AddToGroup(string groupName, string tabName, SerializedProperty property, int order = 0) {
            var tabProperty = new TabProperty(groupName, tabName, property, order);
            AddToGroup(tabProperty);
        }
        
        /// <summary>
        /// This method is used to try get the tab group from the given property name.
        /// </summary>
        /// <param name="propertyName">The property that you want to get the group for.</param>
        /// <param name="tabGroup">The tab group for the given property name.</param>
        /// <returns>True if the tab group was found, otherwise false.</returns>
        public bool TryGetTabGroup(string propertyName, out TabGroup tabGroup) {
            foreach(var item in _tabGroups.Values) {
                if(!item.ContainsProperty(propertyName)) continue;
                tabGroup = item;
                return true;
            }
            tabGroup = null;
            return false;
        }

        /// <summary>
        /// This method is used to try get the tab group from the given property.
        /// </summary>
        /// <param name="property">The property that you want to get the group for.</param>
        /// <param name="tabGroup">The tab group for the given property.</param>
        /// <returns>True if the tab group was found, otherwise false.</returns>
        public bool TryGetTabGroup(SerializedProperty property, out TabGroup tabGroup) {
            return TryGetTabGroup(property.name, out tabGroup);
        }

        /// <summary>
        /// This method is used to draw the tab group for the given property if it exist and hasn't been drawn.
        /// </summary>
        /// <param name="property">The property that you want to draw the tab group for.</param>
        /// <returns>True if the tab group was drawn, otherwise false.</returns>
        public bool TryDrawTabGroup(SerializedProperty property) {
            return TryDrawTabGroup(property.name);
        }

        /// <summary>
        /// This method is used to draw the tab group for the given property name if it exist and hasn't been drawn.
        /// </summary>
        /// <param name="propertyName">The name property that you want to draw the tab group for.</param>
        /// <param name="editor">The calling editor.</param>
        /// <returns>True if the tab group was drawn, otherwise false.</returns>
        public bool TryDrawTabGroup(string propertyName) {
            if(_drawnTabs.Contains(propertyName)) return false;
            if(!TryGetTabGroup(propertyName, out var tabGroup)) return false;
            //mark all of the properties in the tab group as read
            foreach(var prop in tabGroup.Properties) _drawnTabs.Add(prop.Property.name);
            //draw tabs
            EditorGUILayout.Separator();
            //draw title
            DrawTitle(tabGroup);
            //draw tabs
            DrawTabButtons(tabGroup);
            //draw current tab content
            DrawCurrentTabContent(tabGroup);
            return true;
        }
        
        
        
        
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        private void DrawTitle(TabGroup tabGroup) {
            if(tabGroup == null||string.IsNullOrWhiteSpace(tabGroup.Name)) return;
            EditorGUILayout.BeginHorizontal();
            //move the title up
            GUILayout.Space(-15f);
            EditorGUILayout.BeginHorizontal(_tabBoxStyle);
            EditorGUILayout.LabelField(tabGroup.Name);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            //remove the space after the title
            GUILayout.Space(-7f);
        }

        /// <summary>
        /// This method is used to draw the tab buttons.
        /// </summary>
        /// <param name="tabGroup">The tab group for which the buttons will be drawn.</param>
        private void DrawTabButtons(TabGroup tabGroup) {
            var totalWidth = EditorGUIUtility.currentViewWidth+13;
            var tabsWidth = tabGroup.MaxWidth * tabGroup.Count;
            if(tabGroup.HasPrevious) tabsWidth += _previousWidth;
            if(tabGroup.HasNext) tabsWidth += _nextWidth;
            
            var startIndex = 0;
            var endIndex = tabGroup.Count-1;
            var dropDown = false;
            var current = tabGroup.SelectedIndex;
            var currentWidth = tabGroup.MaxWidth;
            if(tabGroup.HasPrevious) currentWidth += _previousWidth;
            if(tabGroup.HasNext) currentWidth += _nextWidth;
            
            if(tabsWidth >= totalWidth) {
                dropDown = true;
                currentWidth += _dropdownWidth;
                if(tabGroup.HasPrevious && currentWidth + tabGroup.MaxWidth <= totalWidth) {
                    startIndex = current - 1;
                    endIndex = current;
                    currentWidth += tabGroup.MaxWidth;
                }
                if(tabGroup.HasNext && currentWidth + tabGroup.MaxWidth <= totalWidth) {
                    endIndex = current + 1;
                    currentWidth += tabGroup.MaxWidth;
                }
                for(var i = startIndex - 1; i >= 0 && currentWidth + tabGroup.MaxWidth <= totalWidth; i--) {
                    currentWidth += tabGroup.MaxWidth;
                    startIndex = i;
                }
                for(var i = endIndex + 1; endIndex < tabGroup.Count && currentWidth + tabGroup.MaxWidth <= totalWidth; i++) {
                    currentWidth += tabGroup.MaxWidth;
                    endIndex = i;
                }
            }

            //get the tab names
            var names = new string[endIndex - startIndex + 1];
            for(var i = 0; i < names.Length; i++) names[i] = tabGroup[i + startIndex].Name;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(-13f);
            if(DrawPreviousButton(tabGroup)) return;
            EditorGUI.BeginChangeCheck();
            //draw buttons
            tabGroup.SelectedIndex = GUILayout.Toolbar(current - startIndex, names, Tab.Style) + startIndex;
            if (EditorGUI.EndChangeCheck()) { GUI.FocusControl(null); }
            if(dropDown) {
                var dropdownRect = GUILayoutUtility.GetRect(_dropdownContent, Tab.Style, GUILayout.Width(_dropdownWidth));
                if (GUI.Button(dropdownRect, _dropdownContent, Tab.Style)) tabGroup.Menu.ShowAsContext();
            }
            if(DrawNextButton(tabGroup)) return;
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// This method is used to display the previous button.
        /// </summary>
        /// <param name="tabGroup">The tab group that the button is for.</param>
        /// <returns>True if the button was clicked, otherwise false.</returns>
        private bool DrawPreviousButton(TabGroup tabGroup) {
            if(!tabGroup.HasPrevious) return false;
            var previousRect = GUILayoutUtility.GetRect(_previousContent, Tab.Style, 
                GUILayout.Width(_previousWidth));
            if(!GUI.Button(previousRect, _previousContent, Tab.Style)) return false;
            tabGroup.SelectedIndex -= 1;
            GUI.FocusControl(null);
            return true;
        }

        /// <summary>
        /// This method is used to display the next button.
        /// </summary>
        /// <param name="tabGroup">The tab group that the button is for.</param>
        /// <returns>True if the button was clicked, otherwise false.</returns>
        private bool DrawNextButton(TabGroup tabGroup) {
            if(!tabGroup.HasNext) return false;
            var nextRect = GUILayoutUtility.GetRect(_nextContent, Tab.Style, GUILayout.Width(_nextWidth));
            if(!GUI.Button(nextRect, _nextContent, Tab.Style)) return false;
            tabGroup.SelectedIndex += 1;
            GUI.FocusControl(null);
            return true;
        }

        /// <summary>
        /// This method is used to draw the content of the current tab.
        /// </summary>
        /// <param name="tabGroup">The tab group that you want to draw the content for.</param>
        private void DrawCurrentTabContent(TabGroup tabGroup) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(-15f);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(-3);
            EditorGUILayout.BeginHorizontal(_tabBoxStyle);
            GUILayout.Space(8);
            EditorGUILayout.BeginVertical(); 
            var first = true;
            foreach(var tabItem in tabGroup.SelectedTabProperties) {
                if(tabItem.ButtonAttribute != null) {
                    if(tabItem.ButtonAttribute.OnlyWhenPlaying && !Application.isPlaying) continue;
                    //only add the separator if the first property does not have a header.
                    if(first&&!tabItem.HasHeader) EditorGUILayout.Separator();
                    if(tabItem.ButtonAttribute.Modifiers.Any(x => x.ShouldHide(_editor.serializedObject))) continue;
                    var bdisable = tabItem.ButtonAttribute.Modifiers.Any(x => x.ShouldDisable(_editor.serializedObject));
                    if(bdisable)EditorGUI.BeginDisabledGroup(true);
                    if(GUILayout.Button(tabItem.ButtonAttribute.Name)) {
                        if (tabItem.ButtonAttribute.MethodInfo.IsStatic) { tabItem.ButtonAttribute.MethodInfo.Invoke(null, null); }
                        else { tabItem.ButtonAttribute.MethodInfo.Invoke(_editor.target, null); }
                    }
                    if(bdisable)EditorGUI.EndDisabledGroup();
                    first = false;
                    continue;
                }
                
                var modifiers = tabItem.Property.GetAttributes<AmiModifierAttribute>();
                var amiModifierAttributes = modifiers as AmiModifierAttribute[] ?? modifiers.ToArray();
                var hide = amiModifierAttributes.Any(a => a.ShouldHide(tabItem.Property));
                var disable = amiModifierAttributes.Any(a => a.ShouldDisable(tabItem.Property));
                //skipp hidden
                if(hide) continue;
                //only add the separator if the first property does not have a header.
                if(first&&!tabItem.HasHeader) EditorGUILayout.Separator();

                if(disable) EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(tabItem.Property, true);
                if(disable) EditorGUI.EndDisabledGroup();
                
                first = false;
            }

            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}