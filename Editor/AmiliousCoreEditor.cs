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
using UnityEditor;
using Amilious.Core.Editor.Drawers;

namespace Amilious.Core.Editor {
    
    public static class AmiliousCoreEditor {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string SHOW_SCRIPTS_MENU = "Amilious/Editor/Show Scripts In Inspector";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when the <see cref="ShowScripts"/> value is changed.
        /// </summary>
        public static Action<bool> OnShowScriptsChanged;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set weather or not scripts should be displayed when using the amilious
        /// editor.
        /// </summary>
        /// <remarks>This property gets and sets a EditorPrefs value.</remarks>
        public static bool ShowScripts {
            get {
                if(!EditorPrefs.HasKey(SHOW_SCRIPTS_MENU)) 
                    EditorPrefs.SetBool(SHOW_SCRIPTS_MENU,false);
                return EditorPrefs.GetBool(SHOW_SCRIPTS_MENU);
            }
            set => EditorPrefs.SetBool(SHOW_SCRIPTS_MENU, value);
        }
        
        #endregion
        
        #region Menu Buttons ///////////////////////////////////////////////////////////////////////////////////////////
        
        [MenuItem(AmiliousCore.DOC_MENU_PATH+"Core", false,0)]
        public static void OpenCoreDocumentation() => Process.Start(AmiliousCore.DOCUMENTATION_URL);
        
        
        /// <inheritdoc cref="AmiliousPropertyDrawer.ReInitialize"/>
        [MenuItem("Amilious/Editor/Reinitialize Property Drawers", false, AmiliousCore.EDITOR_ID+10)]
        private static void ReInitializePropertyDrawers() => AmiliousPropertyDrawer.ReInitialize();

        [MenuItem(SHOW_SCRIPTS_MENU, true, AmiliousCore.EDITOR_ID)]
        private static bool ShowScriptsValidator() {
            Menu.SetChecked(SHOW_SCRIPTS_MENU,ShowScripts);
            return true;
        }
        
        [MenuItem(SHOW_SCRIPTS_MENU)]
        private static void ShowScriptsClicked() {
            ShowScripts = !ShowScripts;
            OnShowScriptsChanged?.Invoke(ShowScripts);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}