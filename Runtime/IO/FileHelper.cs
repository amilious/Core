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

using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Amilious.Core.Extensions;

namespace Amilious.Core.IO {
    
    /// <summary>
    /// This class provides some helper methods for dealing with files.
    /// </summary>
    // ReSharper disable MemberCanBePrivate.Global
    public static class FileHelper {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is true if the file system uses back slashes, otherwise it uses forward slashes.
        /// </summary>
        private static readonly bool UseBackSlash;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Constructor /////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to do some initial calculations.
        /// </summary>
        static FileHelper() {
            UseBackSlash = Application.streamingAssetsPath.Contains("\\");
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the paths of StreamingAssets.
        /// </summary>
        /// <param name="subFolder">The subfolder that you want to get the paths from.</param>
        /// <param name="searchPattern">A search pattern that you want to use within the path.</param>
        /// <returns>A collection of paths within the subfolder that match the search pattern.</returns>
        public static IEnumerable<string> GetStreamingAssetsPaths(string subFolder = null, 
            string searchPattern = null) {
            //build the root path
            var path = Application.streamingAssetsPath;
            if(!string.IsNullOrWhiteSpace(subFolder)) path = Path.Combine(path, subFolder);
            if(searchPattern == null) return Directory.GetFiles(path).Select(FixPath);
            return Directory.GetFiles(path, searchPattern).Select(FixPath);
        }

        /// <summary>
        /// This method is used to fix the slashes in the given path to match the systems requirement.
        /// </summary>
        /// <param name="path">The path that you want to fix.</param>
        /// <returns>The path with the slashes fixed.</returns>
        public static string FixPath(string path) =>
            UseBackSlash ? path.Replace("/", "\\") : path.Replace("\\", "/");

        /// <summary>
        /// This method is used to split the given path into an array.
        /// </summary>
        /// <param name="path">The path that you want to split.</param>
        /// <returns>The split path.</returns>
        public static string[] SplitPath(string path) {
            path = FixPath(path);
            return path.Split(UseBackSlash ? '\\' : '/');
        }

        public static string TrimEndDirectories(string path, int trimLength) {
            var seg = SplitPath(path);
            if(trimLength <= 0) return path;
            if(seg.Length < trimLength) return string.Empty;
            var sb = StringBuilderPool.Rent;
            for(var i = 0; i < seg.Length - trimLength; i++) {
                if(i != 0) sb.Append(UseBackSlash ? '\\' : '/');
                sb.Append(seg[i]);
            }
            return sb.ToStringAndReturnToPool();
        }

        public static string GetSiblingFile(string filePath, string siblingName) {
            return CreatePath(Path.GetDirectoryName(filePath)??string.Empty, siblingName);
        }
        
        public static string RemoveInvalidFileNameCharacters(string fileName) {
            var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var regex = new Regex($"[{Regex.Escape(invalidChars)}]");
            return regex.Replace(fileName, "");
        }
        
        public static string TrimStartDirectories(string path, int trimLength) {
            var seg = SplitPath(path);
            if(trimLength <= 0) return path;
            if(seg.Length < trimLength) return string.Empty;
            var sb = StringBuilderPool.Rent;
            for(var i = trimLength; i < seg.Length; i++) {
                if(i != trimLength) sb.Append(UseBackSlash ? '\\' : '/');
                sb.Append(seg[i]);
            }
            return sb.ToStringAndReturnToPool();
        }

        public static string GetDirectoryName(string path) => FixPath(Path.GetDirectoryName(path));

        /// <summary>
        /// This method is used to create a path.
        /// </summary>
        /// <param name="root">The root path.</param>
        /// <param name="segments">The sub path segments.</param>
        /// <returns>The path created from the root and the segments.</returns>
        public static string CreatePath(string root, params string[] segments) {
            var path = root;
            foreach(var segment in segments) path = Path.Combine(path, segment);
            return FixPath(path);
        }
        
        /// <summary>
        /// This method is used to create the full path from an asset path.
        /// </summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns>The full path.</returns>
        public static string GetFullPathFromAssetPath(string assetPath) {
            return CreatePath(Application.dataPath,assetPath.Substring(7));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}