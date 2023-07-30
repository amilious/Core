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

using System;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Editors.Tabs {
    
    /// <summary>
    /// This class is used to hold information about a tab item.
    /// </summary>
    public class TabProperty {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab group.
        /// </summary>
        public string TabGroup { get;}
        
        /// <summary>
        /// This property contains the name of the tab.
        /// </summary>
        public string TabName { get;}
        
        /// <summary>
        /// This property contains the serialized property that should be added to the tab.
        /// </summary>
        public SerializedProperty Property { get;}
        
        public AmiButtonAttribute ButtonAttribute { get; }
        
        /// <summary>
        /// This property is true if the serialized property has a header, otherwise false.
        /// </summary>
        public bool HasHeader { get; }
        
        /// <summary>
        /// This property contains the draw order for the property on the tab.
        /// </summary>
        public int Order { get; set; }
        
        public int TabOrder { get; set; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constructor is used to create a new tab property from the given attribute and property.
        /// </summary>
        /// <param name="attribute">The tab attribute that is used for the property.</param>
        /// <param name="property">The property.</param>
        public TabProperty(AmiTabAttribute attribute, SerializedProperty property) {
            TabGroup = attribute.TabGroup;
            TabName = attribute.TabName;
            Order = attribute.Order;
            TabOrder = attribute.TabOrder;
            Property = property;
            HasHeader = property.GetAttribute<HeaderAttribute>()!=null;
        }
        
        /// <summary>
        /// This constructor is used to create a new tab property from the given attribute and property.
        /// </summary>
        /// <param name="attribute">The tab attribute that is used for the property.</param>
        /// <param name="buttonAttribute">The button attribute.</param>
        public TabProperty(AmiTabAttribute attribute, AmiButtonAttribute buttonAttribute) {
            TabGroup = attribute.TabGroup;
            TabName = attribute.TabName;
            Order = attribute.Order;
            TabOrder = attribute.TabOrder;
            Property = null;
            HasHeader = false;
            ButtonAttribute = buttonAttribute;
        }
        

        /// <summary>
        /// This constructor is used to create a new tab property from the given values.
        /// </summary>
        /// <param name="tabGroup">The tab group name.</param>
        /// <param name="tabName">The tab name.</param>
        /// <param name="property">The property.</param>
        /// <param name="order">The order of the property.</param>
        public TabProperty(string tabGroup, string tabName, SerializedProperty property, int order = 0) {
            TabGroup = tabGroup;
            TabName = tabName;
            Property = property;
            TabOrder = 0;
            Order = order;
            HasHeader = property.GetAttribute<HeaderAttribute>()!=null;
        }
        
        /// <summary>
        /// This constructor is used to create a new tab property from the given values.
        /// </summary>
        /// <param name="tabGroup">The tab group name.</param>
        /// <param name="tabName">The tab name.</param>
        /// <param name="buttonAttribute">The button attribute.</param>
        /// <param name="order">The order of the property.</param>
        public TabProperty(string tabGroup, string tabName, AmiButtonAttribute buttonAttribute, int order = 0) {
            TabGroup = tabGroup;
            TabName = tabName;
            Property = null;
            Order = order;
            TabOrder = 0;
            HasHeader = false;
            ButtonAttribute = buttonAttribute;
        }
       
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
     
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
                               
        /// <inheritdoc />
        public override bool Equals(object obj) {
            return obj is TabProperty tabProperty && Equals(tabProperty);
        }

        /// <summary>
        /// This method is used to check if two tab properties are equal.
        /// </summary>
        /// <param name="other">The other tab property.</param>
        /// <returns>True if the given tab is equal to this tab, otherwise false.</returns>
        public bool Equals(TabProperty other) {
            return TabGroup == other.TabGroup && TabName == other.TabName && Equals(Property, other.Property);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return HashCode.Combine(TabGroup, TabName, Property);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}