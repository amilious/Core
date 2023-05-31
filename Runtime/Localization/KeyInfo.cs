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

using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.IO;

namespace Amilious.Core.Localization {
    
    /// <summary>
    /// This class is used to hold information about a key.
    /// </summary>
    public class KeyInfo {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        
        private static readonly List<string> PathIndexer = new List<string>();
        private int _pathIndex = -1;
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the key.
        /// </summary>
        public string Key { get; private set; }
        
        /// <summary>
        /// This property contains the key's description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// This property is true if the key has a description, otherwise false.
        /// </summary>
        public bool HasDescription { get; private set; }
        
        #if UNITY_EDITOR ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the key's path.
        /// </summary>
        public string Path => _pathIndex < 0 || _pathIndex >= PathIndexer.Count ? null : PathIndexer[_pathIndex];

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to load a key info from a resource.
        /// </summary>
        /// <param name="asset">The asset resource.</param>
        /// <param name="key">The key value.</param>
        /// <param name="description">The key description.</param>
        public KeyInfo(Object asset, string key, string description) {
            Key = key;
            SetDescription(description);
            #if UNITY_EDITOR
            SetPath(asset.GetFilePath());
            #endif
        }

        #if UNITY_EDITOR ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to load a key info from a path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="key">The key value.</param>
        /// <param name="description">The key description.</param>
        public KeyInfo(string path, string key, string description) {
            Key = key;
            SetDescription(description);
            SetPath(path);
        }
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set the key's description.
        /// </summary>
        /// <param name="description">The new description.</param>
        /// <remarks>This method will only work in the editor and it will not update
        /// the key file.</remarks>
        public void SetDescription(string description) {
            Description = description ?? string.Empty;
            HasDescription = !string.IsNullOrEmpty(Description);
        }
        
        #if UNITY_EDITOR ///////////////////////////////////////////////////////////////////////////////////////////
        
        private void SetPath(string path) {
            var index = PathIndexer.IndexOf(path);
            if(index < 0) {
                PathIndexer.Add(path);
                _pathIndex = PathIndexer.IndexOf(path);
            }
            else _pathIndex = index;
        }

        public string GetLanguagePath(string language) => FileHelper.GetSiblingFile(Path, language+".csv");

        #endif

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}