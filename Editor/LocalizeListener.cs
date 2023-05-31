
using UnityEditor;
using UnityEngine;
using Amilious.Core.IO;
using Amilious.Core.Localization;

namespace Amilious.Core.Editor {
    public class LocalizeListener : AssetPostprocessor {

        private const string LANGUAGE_DIRECTORY = "Resources/Languages";

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when files are updated from code in unity
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, 
            string[] movedAssets, string[] movedFromAssetPaths) {

            var found = false;
            found = CheckForMatch(importedAssets,IgnoreType.Add);
            found = found || CheckForMatch(deletedAssets,IgnoreType.Remove);
            found = found || CheckForMatch(movedAssets,IgnoreType.Move);
            if(!found) return;
            AmiliousLocalization.ReloadData();
            Debug.Log("Reloading data");

        }

        /// <summary>
        /// This method is used to check all of the assets in the given array.
        /// </summary>
        /// <param name="assets">The assets that you want to check.</param>
        private static bool CheckForMatch(string[] assets, IgnoreType ignoreType) {
            //check the added assets
            var found = false;
            foreach(var asset in assets) {
                if(AmiliousLocalization.CheckIgnoreFileChange(asset,ignoreType)) continue;
                if(!IsMatch(asset, out _, out _)) continue;
                found = true;
            }

            return found;
        }

        /// <summary>
        /// This method is used to check if an asset is a match for the localization.
        /// </summary>
        /// <param name="asset">The asset that you want to check.</param>
        /// <param name="isAsset">True if the asset is an asset, otherwise false for a directory.</param>
        /// <param name="directory">The assets directory.</param>
        /// <returns>True if the asset is a match, otherwise false.</returns>
        private static bool IsMatch(string asset, out bool isAsset, out string directory) {
            isAsset = !AssetDatabase.IsValidFolder(asset);
            directory = isAsset? FileHelper.GetDirectoryName(asset).Replace("\\", "/") : asset;
            asset = asset.Replace("\\", "/");
            if(!isAsset && directory.EndsWith(LANGUAGE_DIRECTORY)) return true;
            return asset.EndsWith(".csv") && directory.EndsWith(LANGUAGE_DIRECTORY);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
    
    }
}