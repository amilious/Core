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
using System.Text;
using Amilious.Core.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor {

    /// <summary>
    /// Defines the ODIN_INSPECTOR symbol.
    /// </summary>
    public static class Defines {

        #region Private Static Fields //////////////////////////////////////////////////////////////////////////////////

        private static readonly string[] RemoveSymbols = { };
        
        private static readonly string[] DefineSymbols = {
            "AMILIOUS_CORE", "AMILIOUS_CORE_1_0", "AMILIOUS_CORE_1_0_OR_NEWER"
        };
        
        private static readonly StringBuilder StringBuilder = new StringBuilder();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to make sure that the given define symbols are present.
        /// </summary>
        /// <param name="targetGroup">The target group that you want to add the symbols to.</param>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to be present.</param>
        /// <returns>The number of added define symbols.</returns>
        public static int TryAdd(BuildTargetGroup targetGroup, string name, params string[] defineSymbols) {
            var added = 0;
            if(targetGroup == BuildTargetGroup.Unknown) return added;
            if (!targetGroup.IsSupported()) return added;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            foreach(var define in defineSymbols) {
                if(defines.Contains(define)) continue;
                if(!definesString.EndsWith(";", StringComparison.InvariantCulture)) definesString += ";";
                definesString += define;
                changed = true;
                added++;
            }
            if(!changed) return added;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, definesString);
            Debug.Log(Amilious.MakeTitle($"Added {added} {name} Define Symbols to the {targetGroup.ToString()} target group"));
            return added;
        }

        /// <summary>
        /// This method is used to try add defines to all target groups.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to be present.</param>
        /// <returns>The number of added define symbols.</returns>
        public static int TryAddAll(string name, params string[] defineSymbols) {
            var added = 0;
            foreach (BuildTargetGroup targetGroup in Enum.GetValues(typeof(BuildTargetGroup)))
                added += TryAdd(targetGroup, name, defineSymbols);
            return added;
        }

        /// <summary>
        /// This method is used to make sure that the give define symbols are not present.
        /// </summary>
        /// <param name="targetGroup">The target group that you want to remove the symbols from.</param>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to not be present.</param>
        /// <returns>The number of removed define symbols.</returns>
        public static int TryRemove(BuildTargetGroup targetGroup, string name, params string[] defineSymbols) {
            var removed = 0;
            if(targetGroup == BuildTargetGroup.Unknown) return removed;
            if (!targetGroup.IsSupported()) return removed;
            StringBuilder.Clear();
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Trim();
            var defines = definesString.Split(';');
            var changed = false;
            foreach(var define in defines) {
                if(defineSymbols.Contains(define)) {
                    changed = true;
                    removed++;
                    continue;
                }
                if(StringBuilder.Length > 0) StringBuilder.Append(';');
                StringBuilder.Append(define);
            }
            if(!changed) return removed;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, StringBuilder.ToString());
            Debug.Log(Amilious.MakeTitle($"Removed {removed} {name} Define Symbols from the {targetGroup.ToString()} target group"));
            return removed;
        }
        
        /// <summary>
        /// This method is used to try remove the given define symbols from all target groups.
        /// </summary>
        /// <param name="name">The name of the project the define symbols are for.</param>
        /// <param name="defineSymbols">The define symbols that you want to not be present.</param>
        /// <returns>The number of removed define symbols.</returns>
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
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
