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
using Amilious.Core.Extensions;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor {

    /// <summary>
    /// This class is used to manage define symbols.
    /// </summary>
    public static class Defines {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constant is used as the message displayed when a define is removed.
        /// </summary>
        private const string REMOVE_DEFINE_MESSAGE = "Removed {0} {1} Define Symbols from the {2} target group!";
        
        /// <summary>
        /// This constant is used as the message displayed when a define is added.
        /// </summary>
        private const string ADD_DEFINE_MESSAGE = "Added {0} {1} Define Symbols to the {2} target group!";
        
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
            foreach(var define in defineSymbols) {
                if(defines.Contains(define)) continue;
                if(!definesString.EndsWith(";", StringComparison.InvariantCulture)) definesString += ";";
                definesString += define;
                changed = true;
                added++;
            }
            if(!changed) return added;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, definesString);
            Debug.Log(Amilious.MakeHeader(ADD_DEFINE_MESSAGE,added,name,targetGroup));
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
            foreach(var define in defines) {
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
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
