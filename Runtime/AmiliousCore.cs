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
using System.Diagnostics;
using UnityEngine.WSA;

// ReSharper disable MemberCanBePrivate.Global

namespace Amilious.Core {
    
    /// <summary>
    /// This class is used to contain global values and methods.
    /// </summary>
    public static class AmiliousCore {

        public const string DOC_MENU_PATH = "Amilious/Documentation/";

        public const string DOCUMENTATION_URL = "https://amilious.gitbook.io/core";

        public const string NO_EXECUTOR = "No Amilious Executor exists in the scene.  Actions will not be invoked!";

        public const string MAIN_CONTEXT_MENU = "Amilious/";

        public const string THREADING_CONTEXT_MENU = MAIN_CONTEXT_MENU + "Threading/";

        public const string UI_CONTEXT_MENU = MAIN_CONTEXT_MENU + "UI/";
        
        public const string TAB_CONTEXT_MENU = UI_CONTEXT_MENU + "Tabs/";

        public const string GRAPH_CONTEXT_MENU = UI_CONTEXT_MENU + "Graph/";

        public const string PROGRESS_CONTEXT_MENU = UI_CONTEXT_MENU + "Progress/";

        public const string INVALID_SUCCESS = "The value property is not available unless state is Success.";

        public const string INVALID_ERROR = "The error property is not available unless state is Error.";

        public const string INVALID_PENDING = "Cannot process a future that isn't in the Pending state.";

        #region Menu Buttons ///////////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        public const int EDITOR_ID = 5000;
        public const int PACKAGE_ID = 1000;
        /// <inheritdoc cref="AmiliousScriptableObject.FixDuplicateIds"/>
        [UnityEditor.MenuItem("Amilious/Core/Amilious Scriptable Objects/Fix Duplicate Ids", false, 
            PACKAGE_ID)]
        private static void FixDuplicateIds() => AmiliousScriptableObject.FixDuplicateIds();
        
        /// <inheritdoc cref="AmiliousScriptableObject.RegenerateIds"/>
        [UnityEditor.MenuItem("Amilious/Core/Amilious Scriptable Objects/Regenerate Ids", false,
            PACKAGE_ID+1)]
        private static void RegenerateIds() => AmiliousScriptableObject.RegenerateIds();

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Arg Methods ////////////////////////////////////////////////////////////////////////////////////////////

        private static int? _instanceId;
        private static bool _checkedForInstanceId;
        
        /// <summary>
        /// This method is used to try the instance id from the command line arguments.
        /// </summary>
        /// <param name="id">The id of the instance.</param>
        /// <returns>True if has command line arg, otherwise false.</returns>
        public static bool TryGetInstanceId(out int id) {
            if(_checkedForInstanceId) {
                id = _instanceId.GetValueOrDefault();
                return _instanceId.HasValue;
            }
            _checkedForInstanceId = true;
            var args = Environment.GetCommandLineArgs();
            for(var i = 0; i<args.Length;i++) {
                if(!args[i].Equals("-instance-id", StringComparison.CurrentCultureIgnoreCase)) continue;
                if(i + 1 >= args.Length) continue;
                if(!int.TryParse(args[1 + 1], out id)) continue;
                _instanceId = id;
                return true;
            }
            id = 0;
            return false;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}