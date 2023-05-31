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
using System.Linq;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Tabs {
    
    /// <summary>
    /// This class is used to represent a tab group in the inspector.
    /// </summary>
    public class TabGroup {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly Dictionary<string, Tab> _tabs = new Dictionary<string, Tab>();
        private readonly string _targetName;
        private static readonly Dictionary<string, int> SelectedIds = new Dictionary<string, int>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the properties within the tab group.
        /// </summary>
        public IEnumerable<TabProperty> Properties {
            get {
                foreach(var tab in _tabs.Values) {
                    for(var i = 0; i < tab.Count; i++)
                        yield return tab[i];
                }
            }
        }

        /// <summary>
        /// This property contains the name of the tab group
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// This property contains the menu for the tab group.
        /// </summary>
        public GenericMenu Menu { get; private set; } = new GenericMenu();
        
        /// <summary>
        /// This property contains the with of all the tabs as buttons in this tab group.
        /// </summary>
        public float TabsWidth { get; private set; }
        
        /// <summary>
        /// This property contains the maximum width for a tab in this tab group.
        /// </summary>
        public float MaxWidth { get; private set; }

        /// <summary>
        /// This property contains the selected index of this tab group. 
        /// </summary>
        public int SelectedIndex {
            get => Mathf.Clamp(SelectedIds.TryGetValueFix(_targetName, out var i) ? i : 0,0,Count);
            set => SelectedIds[_targetName] = Mathf.Clamp(value, 0, _tabs.Count - 1);
        }

        /// <summary>
        /// The currently selected tab.
        /// </summary>
        public Tab SelectedTab => this[SelectedIndex];

        /// <summary>
        /// This property is true if there is a previous tab before the selected tab, otherwise false.
        /// </summary>
        public bool HasPrevious => SelectedIndex > 0;

        /// <summary>
        /// This property is true if there is a next tab after the selected tab, otherwise false.
        /// </summary>
        public bool HasNext => SelectedIndex + 1 < Count;

        /// <summary>
        /// This property is used to get the tab with the given name.
        /// </summary>
        /// <param name="tabName">The name of the tab.</param>
        public Tab this[string tabName] => _tabs.TryGetValueFix(tabName, out var tab) ? tab : null;

        /// <summary>
        /// This property is used to get the tab with the given index.
        /// </summary>
        /// <param name="index">The index of the tab.</param>
        public Tab this[int index] => _tabs.ElementAt(index).Value;

        /// <summary>
        /// This property is used to get the number of tabs within the tab group.
        /// </summary>
        public int Count => _tabs.Count;

        /// <summary>
        /// This property contains the properties within the selected tab of the tab group.
        /// </summary>
        public IEnumerable<TabProperty> SelectedTabProperties {
            get {
                if(SelectedIndex>=Count) yield break;
                var tab = this[SelectedIndex];
                for(var i = 0; i < tab.Count; i++) {
                    yield return tab[i];
                }
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constructor is used to create a new tab group with the given name.
        /// </summary>
        /// <param name="name">The name of the tab group.</param>
        /// <param name="serializedObject">The serialized object that contains the tab group</param>
        public TabGroup(string name, SerializedObject serializedObject) {
            Name = name;
            _targetName = serializedObject.GetScriptFullName();
            RecalculateTabs();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a property to the tab group.
        /// </summary>
        /// <param name="attribute">The tab attribute.</param>
        /// <param name="property">The property that you want to add.</param>
        /// <returns>True if the property was added, otherwise false.</returns>
        public bool TryAddProperty(AmiTabAttribute attribute, SerializedProperty property) {
            var tabInfo = new TabProperty(attribute, property);
            return TryAddProperty(tabInfo);
        }

        /// <summary>
        /// This method is used to add a property to the tab group.
        /// </summary>
        /// <param name="tabName">The name of the tab.</param>
        /// <param name="property">The property that you want to add.</param>
        /// <param name="order">The properties order.</param>
        /// <returns>True if the property was added, otherwise false.</returns>
        public bool TryAddProperty(string tabName, SerializedProperty property, int order = 0) {
            var tabInfo = new TabProperty(Name, tabName, property, order);
            return TryAddProperty(tabInfo);
        }

        /// <summary>
        /// This method is used to add a property to the tab group.
        /// </summary>
        /// <param name="tabProperty">The tab property that you want to add.</param>
        /// <returns>True if the property was added, otherwise false.</returns>
        public bool TryAddProperty(TabProperty tabProperty) {
            //make sure the property belongs to the tab group
            if(tabProperty.TabGroup != Name) return false;
            //the tab does not exist create it now
            if(_tabs.TryGetValue(tabProperty.TabName, out var tab)) return tab.AddProperty(tabProperty);
            _tabs[tabProperty.TabName] = new Tab(tabProperty.TabName);
            RecalculateTabs();
            return _tabs[tabProperty.TabName].AddProperty(tabProperty);
        }

        /// <summary>
        /// This method is used to check if the given property exists in the tab group.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>True if the property exists within the tab group, otherwise false.</returns>
        public bool ContainsProperty(SerializedProperty property) => ContainsProperty(property.name);

        /// <summary>
        /// This method is used to check if the given property name exists in the tab group.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the property with the given name exists within the tab group, otherwise false.</returns>
        public bool ContainsProperty(string propertyName) {
            foreach(var tab in _tabs.Values) {
                if(tab.ContainsProperty(propertyName)) return true;
            }
            return false;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to recalculate tab values.
        /// </summary>
        private void RecalculateTabs() {
            TabsWidth = 0;
            MaxWidth = 0;
            foreach(var tab in _tabs.Values) {
                TabsWidth += tab.ContentSize;
                MaxWidth = Mathf.Max(MaxWidth, tab.ContentSize);
            }
            Menu = new GenericMenu();
            for(var i = 0; i < Count; i++) {
                var i1 = i;
                Menu.AddItem(this[i].Content,false, () => {
                    SelectedIndex = i1;
                    GUI.FocusControl(null);
                });
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}