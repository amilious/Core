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
using System.Reflection;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="Assembly"/> class.
    /// </summary>
    public static class AssemblyExtensions {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Dictionary<string, string> AssemblyDefinitionPaths = new Dictionary<string, string>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        static AssemblyExtensions() {
            foreach(var guid in AssetDatabase.FindAssets("t:AssemblyDefinitionAsset")) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var assemblyDefinition = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
                AssemblyDefinitionPaths.Add(assemblyDefinition.name,path);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the asset path of the assembly definition for the assembly.
        /// </summary>
        /// <param name="assembly">The assembly that you want to get the asset path for.</param>
        /// <returns>The asset path of the assembly's assembly definition or the assembly path if there is not
        /// an assembly definition.</returns>
        public static string GetDefinitionAssetPath(this Assembly assembly) =>
            AssemblyDefinitionPaths.TryGetValue(assembly.GetName().Name, out var path) ? path : assembly.Location;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}