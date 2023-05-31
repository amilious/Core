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
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using Amilious.Core.Localization;
using UnityEditor.Experimental.GraphView;

// ReSharper disable MemberCanBePrivate.Global
namespace Amilious.Core.Editor.SearchProviders {
    
    /// <summary>
    /// This search provider is used to return a localization key.
    /// </summary>
    public class LocalizationSearchProvider : ScriptableObject, ISearchWindowProvider {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the serialized property.
        /// </summary>
        public SerializedProperty Property { get; private set; }
        
        /// <summary>
        /// This property contains the callback.
        /// </summary>
        public Action<string> Callback { get; private set; }

        /// <summary>
        /// This property contains the valid paths.
        /// </summary>
        public string[] ValidPaths { get; private set; } = null;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the search provider with a property.
        /// </summary>
        /// <param name="property">The property that you want to change with the search provider.</param>
        public void Initialize(SerializedProperty property) {
            ValidPaths = null;
            Property = property;
        }

        /// <summary>
        /// This method is used to initialize the search provider with a property.
        /// </summary>
        /// <param name="validPaths">The valid key paths.</param>
        /// <param name="property">The property that you want to change with the search provider.</param>
        public void Initialize(IEnumerable<string> validPaths, SerializedProperty property) {
            ValidPaths = validPaths.ToArray();
            Property = property;
        }

        /// <summary>
        /// This method is used to initialize the search provider with a property.
        /// </summary>
        /// <param name="validPath">The valid key path.</param>
        /// <param name="property">The property that you want to change with the search provider.</param>
        public void Initialize(string validPath, SerializedProperty property) {
            ValidPaths = validPath == "All"?null: new string[] { validPath };
            Property = property;
        }

        /// <summary>
        /// This method is used to initialize the search provider with a callback method.
        /// </summary>
        /// <param name="callback">The method that will be called when a result is picked.</param>
        public void Initialize(Action<string> callback) {
            ValidPaths = null;
            Callback = callback;
        }

        /// <summary>
        /// This method is used to initialize the search provider with a callback method.
        /// </summary>
        /// <param name="validPaths">The valid key paths.</param>
        /// <param name="callback">The method that will be called when a result is picked.</param>
        public void Initialize(IEnumerable<string> validPaths, Action<string> callback) {
            ValidPaths = validPaths.ToArray();
            Callback = callback;
        }

        /// <summary>
        /// This method is used to initialize the search provider with a callback method.
        /// </summary>
        /// <param name="validPath">The valid key path.</param>
        /// <param name="callback">The method that will be called when a result is picked.</param>
        public void Initialize(string validPath, Action<string> callback) {
            ValidPaths = validPath == "All"?null: new string[] { validPath };
            Callback = callback;
        }

        /// <inheritdoc />
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
            var searchList = new List<SearchTreeEntry> {
                new SearchTreeGroupEntry(new GUIContent("Localization Keys"),0)
            };
            
            var sortedKeys = ValidPaths==null? AmiliousLocalization.Keys.ToList():
                AmiliousLocalization.GetValidKeys(ValidPaths).ToList();
            sortedKeys.Sort(Sort);
            var groups = new List<string>();
            foreach(var item in sortedKeys) {
                var entryTitle = item.Split('/');
                var groupName = "";
                for(var i = 0; i < entryTitle.Length - 1; i++) {
                    groupName += entryTitle[i];
                    if(!groups.Contains(groupName)) {
                        searchList.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]),i+1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }
                var entry = new SearchTreeEntry(new GUIContent(entryTitle.Last())) {
                    level = entryTitle.Length, userData = item
                };
                searchList.Add(entry);
            }

            return searchList;
        }

        /// <inheritdoc />
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            if(Property != null) {
                Property.stringValue = (string)searchTreeEntry.userData;
                Property.serializedObject.ApplyModifiedProperties();
            }
            Callback?.Invoke((string)searchTreeEntry.userData);
            return true;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to sort items in the search provider.
        /// </summary>
        /// <param name="a">The first item that you want to compare.</param>
        /// <param name="b">The second item that you want to compare.</param>
        /// <returns>The comparison result.</returns>
        private static int Sort(string a, string b) {
            var aLevels = a.Split('/');
            var bLevels = b.Split('/');
            for(var i = 0; i < aLevels.Length; i++) {
                if(i >= bLevels.Length) return 1;
                var value = string.Compare(aLevels[i], bLevels[i], StringComparison.Ordinal);
                if(value == 0) continue;
                if(aLevels.Length != bLevels.Length && (i == aLevels.Length - 1 || i == bLevels.Length - 1))
                    return aLevels.Length < bLevels.Length ? 1 : -1;
                return value;
            }
            return 0;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////        

    }
}