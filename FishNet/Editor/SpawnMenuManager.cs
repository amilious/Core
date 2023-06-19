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
using Amilious.Core.Editor;

namespace Amilious.Core.FishNet.Editor {
    
    public static class SpawnMenuManager {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string ROOT = "GameObject/Amilious/Core/FishNet/";
        public const string IDENTITY_MANAGERS = "Prefabs/Amilious/Identity Managers (FishNet)";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Spawn Menu Methods /////////////////////////////////////////////////////////////////////////////////////
       
        [MenuItem(ROOT + "Identity Managers", false, -100)]
        public static void SpawnIdentityManagers() =>
            Spawn.SpawnPrefab(IDENTITY_MANAGERS, FishNetExtensions.IDENTITY_MANAGERS_GAME_OBJECT_NAME, true);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}