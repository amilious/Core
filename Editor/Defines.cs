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
using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.Core.IO;
using UnityEditorInternal;
using Amilious.Core.Extensions;
using Amilious.Core.Definitions;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor {

    /// <summary>
    /// This class is used to manage define symbols.
    /// </summary>
    public class Defines : AssetPostprocessor {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constant is used as the message displayed when a define is removed.
        /// </summary>
        private const string REMOVE_DEFINE_MESSAGE = "Removed {0} {1} Define Symbols from the {2} target group!";
        
        /// <summary>
        /// This constant is used as the message displayed when a define is added.
        /// </summary>
        private const string ADD_DEFINE_MESSAGE = "Added {0} {1} Define Symbols to the {2} target group!";

        private const string REGISTER_ASSET = "<b><color={0}>[{1}]</color></b> Registered the {2} asset as {1}!";
        
        private const string REMOVING_MISSING_MESSAGE = "<b><color={0}>[{1}]</color></b> The {1} asset was removed!  Removing define symbols...";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Static Fields //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// These are the define symbols that are no longer valid for amilious core and should be removed if they exist.
        /// </summary>
        private static readonly string[] RemoveSymbols = { };
        
        /// <summary>
        /// These are the define symbols that should be added if they do not exist for amilious core.
        /// <remarks>Remember to update the assembly definitions when changing the active defines.</remarks>
        /// </summary>
        private static readonly string[] DefineSymbols = {
            "AMILIOUS_CORE", "AMILIOUS_CORE_1_0", "AMILIOUS_CORE_1_0_OR_NEWER"
        };

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
       
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to make sure that the given define symbols are present.
        /// </summary>
        /// <param name="targetGroup">The target group that you want to add the symbols to.</param>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to be present.</param>
        /// <returns>The number of added define symbols.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static int TryAdd(BuildTargetGroup targetGroup, string name, params string[] defineSymbols) {
            var added = 0;
            if(targetGroup == BuildTargetGroup.Unknown) return added;
            if (!targetGroup.IsSupported()) return added;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            BasicSave.TryGetDefineInfo(name, out var defineInfo);
            var defineInfoChanged = false;
            foreach(var define in defineSymbols) {
                if(defineInfo is not null &&!defineInfo.defines.Contains(define)) {
                    defineInfo.defines.Add(define);
                    defineInfoChanged = true;
                }
                if(defines.Contains(define)) continue;
                if(!definesString.EndsWith(";", StringComparison.InvariantCulture)) definesString += ";";
                definesString += define;
                changed = true;
                added++;
            }
            if(!changed) return added;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, definesString);
            Debug.Log(Amilious.MakeHeader(ADD_DEFINE_MESSAGE,added,name,targetGroup));
            if(defineInfo is not null && defineInfoChanged) 
                BasicSave.TryAddDefineInfo(name,defineInfo, true,true);
            return added;
        }

        /// <summary>
        /// This method is used to try add defines to all target groups.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to be present.</param>
        /// <returns>The number of added define symbols.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static int TryAddAll(string name, params string[] defineSymbols) {
            var added = 0;
            foreach (BuildTargetGroup targetGroup in Enum.GetValues(typeof(BuildTargetGroup)))
                added += TryAdd(targetGroup, name, defineSymbols);
            return added;
        }

        /// <summary>
        /// This method is used to register an amilious asset.
        /// </summary>
        /// <param name="name">The name of the asset.</param>
        /// <param name="color">The asset header color.</param>
        public static void RegisterAsset(string name, string color) {
            var path = System.Reflection.Assembly.GetCallingAssembly().GetDefinitionAssetPath();
            var exists = BasicSave.TryGetDefineInfo(name, out var defineInfo);
            if(exists&&defineInfo.assemblyDefinitionPath==path&&defineInfo.color==color) return;
            defineInfo.assemblyDefinitionPath = path;
            defineInfo.color = color;
            BasicSave.TryAddDefineInfo(name, defineInfo, true, true);
            Debug.LogFormat(REGISTER_ASSET,color,name,path);
        }

        /// <summary>
        /// This method is used to make sure that the give define symbols are not present.
        /// </summary>
        /// <param name="targetGroup">The target group that you want to remove the symbols from.</param>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to not be present.</param>
        /// <returns>The number of removed define symbols.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static int TryRemove(BuildTargetGroup targetGroup, string name, params string[] defineSymbols) {
            var removed = 0;
            if(targetGroup == BuildTargetGroup.Unknown) return removed;
            if (!targetGroup.IsSupported()) return removed;
            var stringBuilder = StringBuilderPool.Rent;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            BasicSave.TryGetDefineInfo(name, out var defineInfo);
            var defineInfoChanged = false;
            foreach(var define in defines) {
                if(defineInfo is not null&&defineInfo.defines.Contains(define)) {
                    defineInfo.defines.Remove(define);
                    defineInfoChanged = true;
                }
                if(defineSymbols.Contains(define)) {
                    changed = true;
                    removed++;
                    continue;
                }
                stringBuilder.AddIfNotEmpty(';').Append(define);
            }
            if(!changed) return removed;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, stringBuilder.ToStringAndReturnToPool());
            Debug.Log(Amilious.MakeHeader(REMOVE_DEFINE_MESSAGE,removed,name,targetGroup));
            if(defineInfo is not null&&defineInfoChanged) 
                BasicSave.TryAddDefineInfo(name, defineInfo, true, true);
            return removed;
        }
        
        /// <summary>
        /// This method is used to try remove the given define symbols from all target groups.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to not be present.</param>
        /// <returns>The number of removed define symbols.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static int TryRemoveAll(string name, params string[] defineSymbols) {
            var removed = 0;
            foreach (BuildTargetGroup targetGroup in Enum.GetValues(typeof(BuildTargetGroup)))
                removed += TryRemove(targetGroup, name, defineSymbols);
            return removed;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add the amilious core defines.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void AddCoreDefineSymbols() {
            TryRemoveAll("Amilious Core", RemoveSymbols);
            TryAddAll("Amilious Core", DefineSymbols);
            FixAssetDefineSymbols();
        }

        /// <summary>
        /// This method is used to remove asset defines for any missing assets.
        /// </summary>
        private static void FixAssetDefineSymbols() {
            //remove defines if invalid
            var removeList = new List<DefineInfo>();
            foreach (var defineInfo in BasicSave.GetAllDefineInfo()) {
                var assemblyDefinition = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(defineInfo.assemblyDefinitionPath);
                if(assemblyDefinition != null) {
                    //make sure that the defines have not been removed manually
                    TryAddAll(defineInfo.name, defineInfo.defines.ToArray());
                    continue;
                }
                Debug.LogFormat(REMOVING_MISSING_MESSAGE,defineInfo.color,defineInfo.name);
                //the definition was removed so remove the define symbols
                TryRemoveAll(defineInfo.name, defineInfo.defines.ToArray());
                removeList.Add(defineInfo);
            }
            foreach(var defineInfo in removeList) BasicSave.TryRemoveDefineInfo(defineInfo.name);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
