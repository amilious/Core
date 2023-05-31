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
using System.Collections.Generic;

namespace Amilious.Core.Editor.Tabs {
    
    /// <summary>
    /// This class is used to hold all of the tab values.
    /// </summary>
    public class Tab {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private readonly List<TabProperty> _properties = new List<TabProperty>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// This property contains the content size when used in the editor.
        /// </summary>
        public float ContentSize { get; }
        
        /// <summary>
        /// This property contains a content that can be used in a dropdown.
        /// </summary>
        public GUIContent Content { get; }

        /// <summary>
        /// This property can be used to get the tab property at the given index.
        /// </summary>
        /// <param name="index"></param>
        public TabProperty this[int index] => _properties[index];

        /// <summary>
        /// This property can be used to get the number of properties that belong to this tab.
        /// </summary>
        public int Count => _properties.Count;
        
        /// <summary>
        /// This static property contains the tab style.
        /// </summary>
        public static GUIStyle Style { get; }  = 
            new GUIStyle(EditorStyles.miniButtonMid) { fontSize = 10, fontStyle = FontStyle.Bold };

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new tab.
        /// </summary>
        /// <param name="name">The name of the tab.</param>
        public Tab(string name) {
            Name = name;
            Content = new GUIContent(name);
            ContentSize = Style.CalcSize(Content).x + Style.padding.left + Style.padding.right;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a property to the tab.
        /// </summary>
        /// <param name="property">The property that you want to add to the tab.</param>
        /// <returns>True if the property was added, otherwise false if the tab already contained the tab.</returns>
        public bool AddProperty(TabProperty property) {
            if(_properties.Contains(property)) return false;
            _properties.Add(property);
            _properties.Sort(SortTabs);
            return true;
        }

        /// <summary>
        /// This method is used to check if the tab contains the given property.
        /// </summary>
        /// <param name="propertyName">The name of the property that you want to check for.</param>
        /// <returns>True if the tab contains the property with the given name, otherwise false.</returns>
        public bool ContainsProperty(string propertyName) {
            foreach(var prop in _properties) {
                if(prop.Property.name == propertyName) return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used to check if the tab contains the given property.
        /// </summary>
        /// <param name="property">The property that you want to check for.</param>
        /// <returns>True if the tab contains the given property, otherwise false.</returns>
        public bool ContainsProperty(SerializedProperty property) => ContainsProperty(property.name);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to sort tab items based on priority.
        /// </summary>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <returns>The sorted value.</returns>
        protected virtual int SortTabs(TabProperty a, TabProperty b) {
            return a.Order.CompareTo(b.Order);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}