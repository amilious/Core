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

namespace Amilious.Core.Identity.User {
    
    [Flags, Serializable]
    public enum UserFilterFlags {
        
        #region Base Flags /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// If this flag is used, no user's will be valid.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// If this flag is used, only online user's will be valid.
        /// </summary>
        Online = 1 << 0,
        
        /// <summary>
        /// If this flag is used, only offline user's will be valid.
        /// </summary>
        Offline = 1 << 1,
        
        /// <summary>
        /// If this flag is used, only friends will be valid.
        /// </summary>
        Friend = 1 << 2,
        
        /// <summary>
        /// If this flag is used, only users with no friendship status (Friend, PendingFriend, or RequestingFriendship)
        /// will be valid.
        /// </summary>
        NoFriendship = 1 << 3,
        
        /// <summary>
        /// If this flag is used, only users that you have added and are pending acceptance will be valid.
        /// </summary>
        PendingFriend = 1 << 4,
        
        /// <summary>
        /// If this flag is used, only users that are requesting friendship will be valid.
        /// </summary>
        RequestingFriendship = 1<< 5,
        
        /// <summary>
        /// If this flag is used, only blocked users will be valid.
        /// </summary>
        Blocked = 1 << 6,
        
        /// <summary>
        /// If this flag is used, only non-blocked users will be valid.
        /// </summary>
        NotBlocked = 1 <<7,
        
        /// <summary>
        /// If this flag is used, the current user will not be valid.
        /// </summary>
        ExcludeSelf = 1 <<8,
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Convenience Flags //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// If this convenience flag is used, all users will be valid.
        /// </summary>
        AllIncludingSelf = Online | Offline | Friend | NoFriendship | PendingFriend | RequestingFriendship | 
                           NotBlocked | Blocked,
        
        /// <summary>
        /// If this convenience flag is used, all users excluding the current user will be valid.
        /// </summary>
        AllExcludingSelf = AllIncludingSelf | ExcludeSelf,
        
        /// <summary>
        /// If this convenience flag is used, only online friends will be valid.
        /// </summary>
        OnlineFriend = Online | Friend | ExcludeSelf,
        
        /// <summary>
        /// If this convenience flag is used, only offline friends will be valid.
        /// </summary>
        OfflineFriend = Offline | Friend | ExcludeSelf,
        
        /// <summary>
        /// If this convenience flag is used, all users excluding the current user will be valid if they are not a
        /// friend and you have not already requested friendship with them.
        /// </summary>
        NotBlockedNonFriend = NotBlocked | NoFriendship | ExcludeSelf | RequestingFriendship
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

}