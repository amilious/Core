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

namespace Amilious.Core.Editor {
    
    /// <summary>
    /// This class is used to add items to the game object menu.
    /// </summary>
    public static class SpawnMenuManager {
       
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string ROOT = "GameObject/UI/";
        public const string TAB_GROUP_HORIZONTAL = "Prefabs/Amilious/TabGroupHorizontal";
        public const string TAB_GROUP_VERTICAL = "Prefabs/Amilious/TabGroupVertical";
        public const string LINE_GRAPH = "Prefabs/Amilious/LineGraph";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Spawn Menu Methods /////////////////////////////////////////////////////////////////////////////////////
       
        [MenuItem(ROOT + "Tab Group (Horizontal)", false, -100)]
        public static void SpawnHorizontalTabGroup() =>
            Spawn.SpawnPrefab(TAB_GROUP_HORIZONTAL, "Tab Group (Horizontal)", true);
        
        [MenuItem(ROOT + "Tab Group (Vertical)", false, -100)]
        public static void SpawnVerticalTabGroup() =>
            Spawn.SpawnPrefab(TAB_GROUP_VERTICAL, "Tab Group (Vertical)", true);

        [MenuItem(ROOT + "Line Graph", false, -100)]
        public static void SpawnLineGraph() => Spawn.SpawnPrefab(LINE_GRAPH, "Line Graph", true);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}